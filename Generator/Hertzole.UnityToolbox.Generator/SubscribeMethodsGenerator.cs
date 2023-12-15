using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Hertzole.UnityToolbox.Generator.Data;
using Hertzole.UnityToolbox.Generator.Helpers;
using Hertzole.UnityToolbox.Shared;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Hertzole.UnityToolbox.Generator;

[Generator(LanguageNames.CSharp)]
public sealed class SubscribeMethodsGenerator : IIncrementalGenerator
{
	private ReferenceSymbols? referenceSymbols;

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		IncrementalValuesProvider<ClassDeclarationSyntax> provider = context.SyntaxProvider.CreateSyntaxProvider(
			                                                                    IsClassTargetForGeneration,
			                                                                    (n, _) => (ClassDeclarationSyntax) n.Node)
		                                                                    .Where(syntax => syntax is not null);

		IncrementalValueProvider<(Compilation Left, ImmutableArray<ClassDeclarationSyntax> Right)> compilation =
			context.CompilationProvider.Combine(provider.Collect());

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
		catch (Exception)
		{
			return false;
		}

		return true;
	}

	private void Execute(SourceProductionContext context, Compilation compilation, ImmutableArray<ClassDeclarationSyntax> typeList)
	{
		if (typeList.IsDefaultOrEmpty)
		{
			return;
		}

		List<SubscribeField> fields = new List<SubscribeField>();

		referenceSymbols ??= new ReferenceSymbols(compilation);

		INamedTypeSymbol? generateSubscribeMethodsAttribute = referenceSymbols.GenerateSubscribeMethodsAttribute;
		INamedTypeSymbol? generateLoadAttribute = referenceSymbols.GenerateLoadAttribute;

		if (generateSubscribeMethodsAttribute == null)
		{
			Log.LogError($"Could not get symbol for GenerateLoadAttribute for assembly '{compilation.AssemblyName}'.");
			return;
		}

		foreach (TypeDeclarationSyntax? typeSyntax in typeList.Distinct(TypeNameDeclarationComparer.Instance))
		{
			if (typeSyntax == null)
			{
				continue;
			}

			if (compilation.GetSemanticModel(typeSyntax.SyntaxTree).GetDeclaredSymbol(typeSyntax) is not INamedTypeSymbol typeSymbol)
			{
				continue;
			}

			fields.Clear();

			if (referenceSymbols.GenerateSubscribeMethodsAttribute == null)
			{
				return;
			}

			foreach (ISymbol member in typeSymbol.GetMembers())
			{
				try
				{
					// Log.LogInfo($"Checking field {member.Name} in {typeSymbol.Name}...");

					if (member is not IFieldSymbol field || field.IsStatic || field.IsReadOnly ||
					    !field.TryGetAttribute(referenceSymbols.GenerateSubscribeMethodsAttribute!, out _))
					{
						continue;
					}

					INamedTypeSymbol? addressableSymbol = null;
					string fieldName = field.Name;

					if (generateLoadAttribute != null && referenceSymbols.AssetReferenceT != null && field.TryGetAttribute(generateLoadAttribute, out _))
					{
						addressableSymbol = AddressablesHelper.GetAddressableType(field, referenceSymbols.AssetReferenceT);
						if (addressableSymbol != null)
						{
							fieldName = TextUtility.FormatAddressableName(field.Name);
						}
					}

					fields.Add(new SubscribeField(field, fieldName));
				}
				catch (Exception e)
				{
					Log.LogError($"Failed to check field. {e.Message}{Environment.NewLine}{e.StackTrace}");
				}
			}

			if (fields.Count > 0)
			{
				int fieldsCount = fields.Count;

				try
				{
					using (SourceScope source = new SourceScope($"{typeSymbol.Name}.SubscribeMethods", context))
					{
						using (TypeScope type = source.WithClass(typeSymbol.GetGenericFriendlyName()).WithAccessor(TypeAccessor.None)
						                              .WithNamespace(typeSymbol.ContainingNamespace).Partial())
						{
							using (MethodScope subscribeAll = type.WithMethod("SubscribeToAll").WithAccessor(MethodAccessor.Private))
							{
								for (int i = 0; i < fieldsCount; i++)
								{
									subscribeAll.WriteLine($"SubscribeTo{TextUtility.FormatVariableLabel(fields[i].FieldName)}();");
								}
							}

							using (MethodScope unsubscribeAll = type.WithMethod("UnsubscribeFromAll").WithAccessor(MethodAccessor.Private))
							{
								for (int i = 0; i < fieldsCount; i++)
								{
									unsubscribeAll.WriteLine($"UnsubscribeFrom{TextUtility.FormatVariableLabel(fields[i].FieldName)}();");
								}
							}

							for (int i = 0; i < fieldsCount; i++)
							{
								GenerateSubscribeMethods(type, fields[i].Field.Type as INamedTypeSymbol, fields[i].FieldName, referenceSymbols);
							}
						}
					}
				}
				catch (Exception e)
				{
					Log.LogError($"Failed to generate subscribe methods for {typeSymbol.Name}. {e}");
				}
			}
		}
	}

	public static void GenerateSubscribeMethods(TypeScope type, INamedTypeSymbol? typeSymbol, string valueName, ReferenceSymbols referenceSymbols)
	{
		string formattedValueName = TextUtility.FormatVariableLabel(valueName);
		type.AddField(TypeAccessor.Private, "bool", $"hasSubscribedTo{formattedValueName}");
		if (!ScriptableValueHelper.TryGetScriptableType(typeSymbol, out ScriptableType scriptableType, out ITypeSymbol? genericType, referenceSymbols))
		{
			return;
		}

		using (MethodScope subscribe = type.WithMethod($"SubscribeTo{formattedValueName}").WithAccessor(MethodAccessor.Private))
		{
			subscribe.WriteLine($"if ({valueName} != null && !hasSubscribedTo{formattedValueName})");
			subscribe.WriteLine("{");
			type.Source.Indent++;

			switch (scriptableType)
			{
				case ScriptableType.None:
					break;
				case ScriptableType.Event:
				case ScriptableType.GenericEvent:
					subscribe.WriteLine($"{valueName}.OnInvoked += On{formattedValueName}Invoked;");
					break;
				case ScriptableType.Value:
					subscribe.WriteLine($"{valueName}.OnValueChanged += On{formattedValueName}Changed;");
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			subscribe.WriteLine($"hasSubscribedTo{formattedValueName} = true;");
			type.Source.Indent--;
			subscribe.WriteLine("}");
		}

		using (MethodScope unsubscribe = type.WithMethod($"UnsubscribeFrom{formattedValueName}").WithAccessor(MethodAccessor.Private))
		{
			unsubscribe.WriteLine($"if ({valueName} != null && hasSubscribedTo{formattedValueName})");
			unsubscribe.WriteLine("{");
			type.Source.Indent++;
			switch (scriptableType)
			{
				case ScriptableType.None:
					break;
				case ScriptableType.Event:
				case ScriptableType.GenericEvent:
					unsubscribe.WriteLine($"{valueName}.OnInvoked -= On{formattedValueName}Invoked;");
					break;
				case ScriptableType.Value:
					unsubscribe.WriteLine($"{valueName}.OnValueChanged -= On{formattedValueName}Changed;");
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			unsubscribe.WriteLine($"hasSubscribedTo{formattedValueName} = false;");
			type.Source.Indent--;
			unsubscribe.WriteLine("}");
		}

		switch (scriptableType)
		{
			case ScriptableType.None:
				break;
			case ScriptableType.Event:
				type.WithMethod($"On{formattedValueName}Invoked").WithAccessor(MethodAccessor.Private).WithParameter("object", "sender")
				    .WithParameter("global::System.EventArgs", "e").Partial().Dispose();

				break;
			case ScriptableType.GenericEvent:
				type.WithMethod($"On{formattedValueName}Invoked").WithAccessor(MethodAccessor.Private).WithParameter("object", "sender")
				    .WithParameter(genericType!.ToDisplayString(), "args").Partial().Dispose();

				break;
			case ScriptableType.Value:
				type.WithMethod($"On{formattedValueName}Changed").WithAccessor(MethodAccessor.Private)
				    .WithParameter(genericType!.ToDisplayString(), "previousValue").WithParameter(genericType!.ToDisplayString(), "newValue").Partial()
				    .Dispose();

				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
}