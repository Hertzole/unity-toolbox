using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Hertzole.UnityToolbox.Generator.Data;
using Hertzole.UnityToolbox.Shared;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Hertzole.UnityToolbox.Generator;

[Generator(LanguageNames.CSharp)]
internal sealed class InputCallbacksGenerator : IIncrementalGenerator
{
	public const string ATTRIBUTE_NAME = "Hertzole.UnityToolbox.GenerateInputCallbacksAttribute";

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		Log.LogInfo("==== INITIALIZE INPUT CALLBACKS ====");

		IncrementalValuesProvider<TypeDeclarationSyntax> provider = context.SyntaxProvider.CreateSyntaxProvider(
			                                                                   IsTypeTargetForGeneration,
			                                                                   GetMemberDeclarationsForSourceGen)
		                                                                   .Where(t => t.reportAttributeFound)
		                                                                   .Select((t, _) => t.Item1);

		context.RegisterSourceOutput(context.CompilationProvider.Combine(provider.Collect()), (ctx, t) => GenerateCode(ctx, t.Left, t.Right));
	}

	private static bool IsTypeTargetForGeneration(SyntaxNode node, CancellationToken cancellationToken)
	{
		if (node is not TypeDeclarationSyntax typeDeclaration)
		{
			return false;
		}

		cancellationToken.ThrowIfCancellationRequested();

		if (typeDeclaration.Members.Count == 0)
		{
			return false;
		}

		cancellationToken.ThrowIfCancellationRequested();

		foreach (MemberDeclarationSyntax member in typeDeclaration.Members)
		{
			if (member is FieldDeclarationSyntax or PropertyDeclarationSyntax && member.AttributeLists.Count > 0)
			{
				return true;
			}
		}

		return false;
	}

	private static (TypeDeclarationSyntax, bool reportAttributeFound) GetMemberDeclarationsForSourceGen(GeneratorSyntaxContext context,
		CancellationToken cancellationToken)
	{
		TypeDeclarationSyntax typeDeclaration = (TypeDeclarationSyntax) context.Node;

		if (context.SemanticModel.GetDeclaredSymbol(typeDeclaration) is not INamedTypeSymbol symbol)
		{
			return (typeDeclaration, false);
		}

		foreach (ISymbol? memberDeclaration in symbol.GetMembers())
		{
			cancellationToken.ThrowIfCancellationRequested();

			if (memberDeclaration is not IFieldSymbol or IPropertySymbol)
			{
				continue;
			}

			foreach (AttributeData? attributeData in memberDeclaration.GetAttributes())
			{
				cancellationToken.ThrowIfCancellationRequested();

				if (attributeData.AttributeClass is null)
				{
					continue;
				}

				string attributeName = attributeData.AttributeClass.ToDisplayString();

				if (string.Equals(ATTRIBUTE_NAME, attributeName, StringComparison.Ordinal))
				{
					return (typeDeclaration, true);
				}
			}
		}

		return (typeDeclaration, false);
	}

	private static void GenerateCode(SourceProductionContext context, Compilation compilation, ImmutableArray<TypeDeclarationSyntax> typesList)
	{
		if (typesList.IsDefaultOrEmpty)
		{
			return;
		}

		foreach (TypeDeclarationSyntax syntax in typesList)
		{
			context.CancellationToken.ThrowIfCancellationRequested();

			SemanticModel semanticModel = compilation.GetSemanticModel(syntax.SyntaxTree);

			if (semanticModel.GetDeclaredSymbol(syntax) is not INamedTypeSymbol symbol)
			{
				continue;
			}

			using (ListPool<InputCallbackField>.Get(out List<InputCallbackField> validFields))
			{
				validFields.Clear();

				GetValidFields<IFieldSymbol>(symbol, validFields, context.CancellationToken);
				GetValidFields<IPropertySymbol>(symbol, validFields, context.CancellationToken);

				using (SourceScope source = new SourceScope($"{symbol.Name}.InputCallbacks", context))
				{
					source.WriteUsing("Hertzole.UnityToolbox");

					using (TypeScope type = source.WithClass(symbol.GetGenericFriendlyName()).WithAccessor(TypeAccessor.None)
					                              .WithNamespace(symbol.ContainingNamespace)
					                              .Partial())
					{
						foreach (InputCallbackField field in validFields)
						{
							type.AddField(TypeAccessor.Private, "bool", field.HasSubscribedField, "false");

							using (MethodScope subscribe = type.WithMethod(field.SubscribeToField).WithAccessor(MethodAccessor.Private))
							{
								subscribe.WriteLine($"if (!{field.HasSubscribedField} && {field.Name} != null && {field.InputName} != null)");
								subscribe.WriteLine("{");
								source.Indent++;
								if ((field.CallbackType & InputCallbackType.Started) != 0)
								{
									subscribe.WriteLine($"{field.InputName}.AddStartedListener({field.Name}, {field.StartedMethod});");

									source.Indent--;

									type.WithMethod(field.StartedMethod).WithAccessor(MethodAccessor.Private)
									    .WithParameter("global::UnityEngine.InputSystem.InputAction.CallbackContext", "context").Partial().Dispose();

									source.Indent++;
								}

								if ((field.CallbackType & InputCallbackType.Performed) != 0)
								{
									subscribe.WriteLine($"{field.InputName}.AddPerformedListener({field.Name}, {field.PerformedMethod});");

									source.Indent--;

									type.WithMethod(field.PerformedMethod).WithAccessor(MethodAccessor.Private)
									    .WithParameter("global::UnityEngine.InputSystem.InputAction.CallbackContext", "context").Partial().Dispose();

									source.Indent++;
								}

								if ((field.CallbackType & InputCallbackType.Canceled) != 0)
								{
									subscribe.WriteLine($"{field.InputName}.AddCanceledListener({field.Name}, {field.CanceledMethod});");

									source.Indent--;

									type.WithMethod(field.CanceledMethod).WithAccessor(MethodAccessor.Private)
									    .WithParameter("global::UnityEngine.InputSystem.InputAction.CallbackContext", "context").Partial().Dispose();

									source.Indent++;
								}

								if ((field.CallbackType & InputCallbackType.All) != 0)
								{
									subscribe.WriteLine($"{field.InputName}.AddAllListeners({field.Name}, {field.AllMethod});");

									source.Indent--;

									type.WithMethod(field.AllMethod).WithAccessor(MethodAccessor.Private)
									    .WithParameter("global::UnityEngine.InputSystem.InputAction.CallbackContext", "context").Partial().Dispose();

									source.Indent++;
								}

								subscribe.WriteLine($"{field.HasSubscribedField} = true;");
								source.Indent--;
								subscribe.WriteLine("}");
							}

							using (MethodScope unsubscribe = type.WithMethod(field.UnsubscribeFromField).WithAccessor(MethodAccessor.Private))
							{
								unsubscribe.WriteLine($"if ({field.HasSubscribedField} && {field.Name} != null && {field.InputName} != null)");
								unsubscribe.WriteLine("{");
								source.Indent++;

								if ((field.CallbackType & InputCallbackType.Started) != 0)
								{
									unsubscribe.WriteLine($"{field.InputName}.RemoveStartedListener({field.Name}, {field.StartedMethod});");
								}

								if ((field.CallbackType & InputCallbackType.Performed) != 0)
								{
									unsubscribe.WriteLine($"{field.InputName}.RemovePerformedListener({field.Name}, {field.PerformedMethod});");
								}

								if ((field.CallbackType & InputCallbackType.Canceled) != 0)
								{
									unsubscribe.WriteLine($"{field.InputName}.RemoveCanceledListener({field.Name}, {field.CanceledMethod});");
								}

								if ((field.CallbackType & InputCallbackType.All) != 0)
								{
									unsubscribe.WriteLine($"{field.InputName}.RemoveAllListeners({field.Name}, {field.AllMethod});");
								}

								unsubscribe.WriteLine($"{field.HasSubscribedField} = false;");
								source.Indent--;
								unsubscribe.WriteLine("}");
							}
						}

						using (MethodScope subscribe = type.WithMethod("SubscribeToAllInputCallbacks").WithAccessor(MethodAccessor.Private))
						{
							foreach (InputCallbackField field in validFields)
							{
								subscribe.WriteLine($"{field.SubscribeToField}();");
							}
						}

						using (MethodScope unsubscribe = type.WithMethod("UnsubscribeFromAllInputCallbacks").WithAccessor(MethodAccessor.Private))
						{
							foreach (InputCallbackField field in validFields)
							{
								unsubscribe.WriteLine($"{field.UnsubscribeFromField}();");
							}
						}
					}
				}
			}
		}
	}

	private static void GetValidFields<T>(INamespaceOrTypeSymbol symbol, ICollection<InputCallbackField> fields, CancellationToken cancellationToken)
		where T : ISymbol
	{
		foreach (T fieldSymbol in symbol.GetMembers().OfType<T>())
		{
			cancellationToken.ThrowIfCancellationRequested();

			ReadOnlySpan<AttributeData> attributes = fieldSymbol.GetAttributes().AsSpan();

			foreach (AttributeData attribute in attributes)
			{
				cancellationToken.ThrowIfCancellationRequested();

				if (attribute.AttributeClass is null)
				{
					continue;
				}

				string attributeName = attribute.AttributeClass.ToDisplayString();

				Log.LogInfo($"[INPUT CALLBACKS] : {attributeName} | {attribute.ConstructorArguments.Length}");

				if (attribute.ConstructorArguments.Length == 1 && string.Equals(attributeName, ATTRIBUTE_NAME, StringComparison.Ordinal))
				{
					Log.LogInfo($"Found input callback field on {symbol.Name}.");

					if (attribute.ConstructorArguments[0].Value is string inputName)
					{
						InputCallbackType callbackType = InputCallbackType.None;

						ImmutableArray<KeyValuePair<string, TypedConstant>> namedArguments = attribute.NamedArguments;
						foreach (KeyValuePair<string, TypedConstant> argumentPair in namedArguments)
						{
							cancellationToken.ThrowIfCancellationRequested();

							if (argumentPair.Value.Value is bool value && value)
							{
								switch (argumentPair.Key)
								{
									case "GenerateStarted":
										callbackType |= InputCallbackType.Started;
										break;
									case "GeneratePerformed":
										callbackType |= InputCallbackType.Performed;
										break;
									case "GenerateCanceled":
										callbackType |= InputCallbackType.Canceled;
										break;
									case "GenerateAll":
										callbackType |= InputCallbackType.All;
										break;
								}
							}
						}

						ITypeSymbol? typeSymbol = fieldSymbol switch
						{
							IFieldSymbol field => field.Type,
							IPropertySymbol property => property.Type,
							_ => null
						};

						INamedTypeSymbol? addressableType = typeSymbol != null ? AddressablesHelper.GetAddressableType(typeSymbol) : null;
						bool isAddressable = addressableType != null;
						string name = fieldSymbol.Name;

						if (isAddressable)
						{
							name = TextUtility.FormatAddressableName(name);
						}

						fields.Add(new InputCallbackField(name, inputName, callbackType, isAddressable));
					}
				}
			}
		}
	}
}