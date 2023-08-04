using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Hertzole.UnityToolbox.Generator.Data;
using Hertzole.UnityToolbox.Generator.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Hertzole.UnityToolbox.Generator;

[Generator(LanguageNames.CSharp)]
public sealed class AddressableLoadGenerator : IIncrementalGenerator
{
	private ReferenceSymbols? referenceSymbols;

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		Log.LogInfo("== INITIALIZE ADDRESSABLE LOAD GENERATOR ==");

		IncrementalValuesProvider<ClassDeclarationSyntax> provider = context.SyntaxProvider.CreateSyntaxProvider(
			                                                                    IsClassTargetForGeneration,
			                                                                    (n, _) => (ClassDeclarationSyntax) n.Node)
		                                                                    .Where(syntax => syntax is not null);

		IncrementalValueProvider<(Compilation Left, ImmutableArray<ClassDeclarationSyntax> Right)> compilation =
			context.CompilationProvider.Combine(provider.Collect());

		Log.LogInfo("Registering source output...");

		context.RegisterSourceOutput(compilation, (productionContext, tuple) => Execute(productionContext, tuple.Left, tuple.Right));
	}

	private static bool IsClassTargetForGeneration(SyntaxNode c, CancellationToken cancellationToken)
	{
		if (c is not ClassDeclarationSyntax classSyntax)
		{
			return false;
		}

		try
		{
			if (classSyntax.Members.Count == 0)
			{
				return false;
			}
		}
		catch (Exception e)
		{
			Log.LogError($"Failed to check if class '{classSyntax.Identifier.Text}' has any members. {e.Message}");
			return false;
		}

		return true;
	}

	private void Execute(SourceProductionContext context, Compilation compilation, ImmutableArray<ClassDeclarationSyntax> typeList)
	{
		Log.LogInfo($"Execute on {typeList.Length} items");
		
		if (typeList.IsDefaultOrEmpty)
		{
			Log.LogInfo("No classes found.");
			return;
		}

		List<AddressableLoadField> fields = new List<AddressableLoadField>();
		referenceSymbols ??= new ReferenceSymbols(compilation);

		INamedTypeSymbol? generateLoadAttribute = referenceSymbols.GenerateLoadAttribute;
		INamedTypeSymbol? assetReferenceT = referenceSymbols.AssetReferenceT;

		if (generateLoadAttribute == null)
		{
			Log.LogError($"Could not get symbol for GenerateLoadAttribute for assembly '{compilation.AssemblyName}'.");
			return;
		}

		if (assetReferenceT == null)
		{
			Log.LogError($"Could not get symbol for AssetReferenceT for assembly '{compilation.AssemblyName}'.");
			return;
		}

		foreach (ClassDeclarationSyntax typeSyntax in typeList)
		{
			if (typeSyntax == null)
			{
				Log.LogInfo("Class is null.");
				continue;
			}

			if (compilation.GetSemanticModel(typeSyntax.SyntaxTree).GetDeclaredSymbol(typeSyntax) is not INamedTypeSymbol typeSymbol)
			{
				Log.LogWarning("Failed to get type symbol.");
				continue;
			}

			fields.Clear();
			
			Log.LogInfo($"Get members of {typeSymbol.Name}");

			foreach (ISymbol member in typeSymbol.GetMembers())
			{
				try
				{
					if (member is not IFieldSymbol field || field.IsStatic || field.IsReadOnly || !field.TryGetAttribute(generateLoadAttribute, out _))
					{
						Log.LogInfo($"Member {member.Name} is not a valid field. Is field: {member is IFieldSymbol}. Is static: {member.IsStatic}.  Has attribute: {member.TryGetAttribute(generateLoadAttribute, out _)}.");
						continue;
					}

					Log.LogInfo($"Found valid field {field.Name}.");

					INamedTypeSymbol? addressableType = AddressablesHelper.GetAddressableType(field, assetReferenceT);

					if (addressableType == null)
					{
						Log.LogWarning($"Could not get addressable type for field {field.Name} in {typeSymbol.Name}");
						continue;
					}

					bool generateSubscribeMethods = referenceSymbols.GenerateSubscribeMethodsAttribute != null &&
					                                field.TryGetAttribute(referenceSymbols.GenerateSubscribeMethodsAttribute, out _);

					// If the field name ends with "reference", remove it.
					// Otherwise, add "Value" to the end.
					string valueName = TextUtility.FormatAddressableName(field.Name);

					fields.Add(new AddressableLoadField(field, addressableType, valueName, generateSubscribeMethods));

					Log.LogInfo($"Added field {field.Name} to list.");
				}
				catch (Exception e)
				{
					Log.LogError($"Failed to check field. {e.Message}\n{e.StackTrace}");
				}
			}

			if (fields.Count > 0)
			{
				Log.LogInfo($"Write source for {typeSymbol.Name}");

				try
				{
					using (SourceScope source = new SourceScope($"{typeSymbol.Name}.Addressables", context))
					{
						using (TypeScope type = source.WithClass(typeSymbol.Name).WithAccessor(TypeAccessor.None).WithNamespace(typeSymbol.ContainingNamespace)
						                              .Partial())
						{
							foreach ((IFieldSymbol _, INamedTypeSymbol addressableType, string valueName, bool generateSubscribeMethods) in fields)
							{
								type.WriteLine("[global::JetBrains.Annotations.CanBeNull]");
								type.WriteLine($"private global::{addressableType.ToDisplayString()} {valueName} = null;");
							}

							foreach ((IFieldSymbol field, INamedTypeSymbol addressableType, string _, bool _) in fields)
							{
								type.AddField(TypeAccessor.Private,
									$"global::UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<global::{addressableType.ToDisplayString()}>",
									$"{field.Name}Handle");
							}

							using (MethodScope load = type.WithMethod("LoadAssets").WithAccessor(MethodAccessor.Private))
							{
								foreach ((IFieldSymbol field, INamedTypeSymbol addressableType, string valueName, bool generateSubscribeMethods) in fields)
								{
									load.WriteLine(
										$"{field.Name}Handle = global::UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<global::{addressableType.ToDisplayString()}>({field.Name});");

									load.WriteLine($"{field.Name}Handle.Completed += (op) =>");
									load.WriteLine("{");
									source.Indent++;
									load.WriteLine("if (op.Status == global::UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)");
									load.WriteLine("{");
									source.Indent++;
									load.WriteLine($"{valueName} = op.Result;");

									if (generateSubscribeMethods)
									{
										load.WriteLine($"SubscribeTo{TextUtility.FormatVariableLabel(valueName)}();");
									}

									load.WriteLine($"On{TextUtility.FormatVariableLabel(valueName)}Loaded(op.Result);");
									source.Indent--;
									load.WriteLine("}");
									load.WriteLine("else");
									load.WriteLine("{");
									source.Indent++;
									load.WriteLine(
										$"global::UnityEngine.Debug.LogError($\"Failed to load reference {field.Name} with error: {{op.OperationException}}\");");

									source.Indent--;
									load.WriteLine("}");
									source.Indent--;
									load.WriteLine("};");
								}
							}

							using (MethodScope release = type.WithMethod("ReleaseAssets").WithAccessor(MethodAccessor.Private))
							{
								foreach ((IFieldSymbol field, INamedTypeSymbol _, string _, bool _) in fields)
								{
									release.WriteLine($"if ({field.Name}Handle.IsValid())");
									release.WriteLine("{");
									source.Indent++;
									release.WriteLine($"global::UnityEngine.AddressableAssets.Addressables.Release({field.Name}Handle);");
									source.Indent--;
									release.WriteLine("}");
								}
							}

							foreach ((IFieldSymbol _, INamedTypeSymbol addressableType, string valueName, bool _) in fields)
							{
								type.WithMethod($"On{TextUtility.FormatVariableLabel(valueName)}Loaded").WithAccessor(MethodAccessor.None)
								    .WithParameter($"global::{addressableType.ToDisplayString()}", "value").Partial().Dispose();
							}
						}
					}
				}
				catch (Exception e)
				{
					Log.LogError($"Failed to generate source. {e.Message}\n{e.StackTrace}");
				}
			}
		}
	}
}