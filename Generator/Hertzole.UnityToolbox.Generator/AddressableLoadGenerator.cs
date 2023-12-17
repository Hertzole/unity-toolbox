using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using Hertzole.UnityToolbox.Generator.Data;
using Hertzole.UnityToolbox.Generator.NewScopes;
using Hertzole.UnityToolbox.Shared;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Hertzole.UnityToolbox.Generator;

[Generator(LanguageNames.CSharp)]
public sealed class AddressableLoadGenerator : IIncrementalGenerator
{
	private readonly struct AddressableLoadType : IEquatable<AddressableLoadType>
	{
		public readonly INamedTypeSymbol type;
		public readonly ImmutableArray<AddressableLoadField> fields;

		public static IEqualityComparer<AddressableLoadType> TypeComparer { get; } = new TypeEqualityComparer();

		public AddressableLoadType(INamedTypeSymbol type, ImmutableArray<AddressableLoadField> fields)
		{
			this.type = type;
			this.fields = fields;
		}

		private sealed class TypeEqualityComparer : IEqualityComparer<AddressableLoadType>
		{
			public bool Equals(AddressableLoadType x, AddressableLoadType y)
			{
				return x.Equals(y);
			}

			public int GetHashCode(AddressableLoadType obj)
			{
				return obj.GetHashCode();
			}
		}

		public bool Equals(AddressableLoadType other)
		{
			return type.ToDisplayString(NullableFlowState.NotNull, SymbolDisplayFormat.FullyQualifiedFormat).Equals(
				other.type.ToDisplayString(NullableFlowState.NotNull, SymbolDisplayFormat.FullyQualifiedFormat), StringComparison.Ordinal);
		}

		public override bool Equals(object? obj)
		{
			return obj is AddressableLoadType other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = 0;
				if (type != null)
				{
					hashCode = type.Name.GetHashCode();
				}

				return hashCode;
			}
		}

		public static bool operator ==(AddressableLoadType left, AddressableLoadType right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(AddressableLoadType left, AddressableLoadType right)
		{
			return !left.Equals(right);
		}
	}

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		Log.LogInfo("=== INITIALIZE ADDRESSABLE LOAD GENERATOR ===");

		IncrementalValuesProvider<AddressableLoadType> provider = context.SyntaxProvider.CreateSyntaxProvider(
			                                                                 IsTypeTargetForGeneration,
			                                                                 GetMemberDeclarationsForSourceGen)
		                                                                 .Where(t => t.reportAttributeFound)
		                                                                 .Select((t, _) => t.Item1);

		IncrementalValueProvider<(Compilation Left, ImmutableArray<AddressableLoadType> Right)> compilation =
			context.CompilationProvider.Combine(provider.Collect());

		Log.LogInfo("Registering source output...");

		context.RegisterSourceOutput(compilation, (productionContext, tuple) => Execute(productionContext, tuple.Right));
	}

	private static bool IsTypeTargetForGeneration(SyntaxNode c, CancellationToken cancellationToken)
	{
		// If it's not a type declaration, we don't care.
		if (c is not TypeDeclarationSyntax typeDeclaration)
		{
			return false;
		}

		cancellationToken.ThrowIfCancellationRequested();

		// If there are no members, we don't care.
		if (typeDeclaration.Members.Count == 0)
		{
			return false;
		}

		cancellationToken.ThrowIfCancellationRequested();

		foreach (MemberDeclarationSyntax member in typeDeclaration.Members)
		{
			// If the member is a field or property and has any attribute, we care.
			if (member is FieldDeclarationSyntax or PropertyDeclarationSyntax && member.AttributeLists.Count > 0)
			{
				return true;
			}
		}

		// If we got here, we don't care.
		return false;
	}

	private static (AddressableLoadType, bool reportAttributeFound) GetMemberDeclarationsForSourceGen(GeneratorSyntaxContext context,
		CancellationToken cancellationToken)
	{
		TypeDeclarationSyntax typeDeclaration = (TypeDeclarationSyntax) context.Node;

		if (context.SemanticModel.GetDeclaredSymbol(typeDeclaration) is not INamedTypeSymbol typeSymbol)
		{
			return (default, false);
		}

		ImmutableArray<AddressableLoadField>.Builder fields = ImmutableArray.CreateBuilder<AddressableLoadField>();

		foreach (ISymbol member in typeSymbol.GetMembers())
		{
			cancellationToken.ThrowIfCancellationRequested();

			if (member is not IFieldSymbol fieldSymbol)
			{
				continue;
			}

			if (!AddressablesHelper.TryGetAddressableType(fieldSymbol.Type, out INamedTypeSymbol? addressableType) || addressableType == null)
			{
				continue;
			}

			bool hasGenerateLoadAttribute = false;

			foreach (AttributeData attribute in member.GetAttributes())
			{
				cancellationToken.ThrowIfCancellationRequested();

				if (attribute.AttributeClass is null)
				{
					continue;
				}

				string attributeName = attribute.AttributeClass.ToDisplayString();

				if (string.Equals(Attributes.GENERATE_LOAD, attributeName, StringComparison.Ordinal))
				{
					hasGenerateLoadAttribute = true;
				}
			}

			string valueName = TextUtility.FormatAddressableName(member.Name);
			bool fieldExists = false;
			foreach (ISymbol member2 in typeSymbol.GetMembers())
			{
				if (member2 is not IFieldSymbol field2 || field2.IsStatic || field2.IsReadOnly)
				{
					continue;
				}

				if (field2.Name.Equals(valueName, StringComparison.Ordinal))
				{
					fieldExists = true;
					break;
				}
			}

			bool generateSubscribeMethods = fieldSymbol.TryGetAttribute(Attributes.GENERATE_SUBSCRIBE_METHODS, out _);
			bool generateInputCallbacks = fieldSymbol.TryGetAttribute(Attributes.GENERATE_INPUT_CALLBACKS, out _);

			if (hasGenerateLoadAttribute)
			{
				fields.Add(new AddressableLoadField(fieldSymbol, addressableType, valueName, generateSubscribeMethods, fieldExists, generateInputCallbacks));
			}
		}

		return fields.Count > 0 ? (new AddressableLoadType(typeSymbol, fields.ToImmutable()), true) : (default, false);
	}

	private static void Execute(SourceProductionContext context, ImmutableArray<AddressableLoadType> typeList)
	{
		if (typeList.IsDefaultOrEmpty)
		{
			Log.LogInfo("No classes found.");
			return;
		}

		foreach (AddressableLoadType typeSyntax in typeList.Distinct(AddressableLoadType.TypeComparer))
		{
			bool isStruct = typeSyntax.type.IsValueType;

			if (typeSyntax.fields.Length > 0)
			{
				Log.LogInfo($"Generating source for {typeSyntax.type.Name}");

				try
				{
					using PoolHandle<StringBuilder> handle = StringBuilderPool.Get(out StringBuilder? typeNameBuilder);
					typeNameBuilder.Append(typeSyntax.type.Name);
					typeNameBuilder.Append(".Addressables");

					using (NewScopes.SourceScope source = NewScopes.SourceScope.Create(typeNameBuilder.ToString(), context)
					                                               .WithNamespace(typeSyntax.type.ContainingNamespace))
					{
						using (NewScopes.TypeScope type = source.WithType(typeSyntax.type.GetGenericFriendlyName(), isStruct ? TypeType.Struct : TypeType.Class)
						                                        .WithAccessor(TypeAccessor.None)
						                                        .Partial())
						{
							foreach ((IFieldSymbol _, INamedTypeSymbol addressableType, string valueName, bool _, bool fieldExists, bool _) in
							         typeSyntax.fields)
							{
								if (!fieldExists)
								{
									using (FieldScope field = type.AddField(valueName, addressableType.ToDisplayString(NullableFlowState.None, SymbolDisplayFormat.FullyQualifiedFormat), "null"))
									{
										field.AddAttribute("JetBrains.Annotations.CanBeNull");
									}
								}
							}

							foreach ((IFieldSymbol field, INamedTypeSymbol addressableType, string _, bool _, bool _, bool _) in typeSyntax.fields)
							{
								using (StringBuilderPool.Get(out StringBuilder typeBuilder))
								{
									typeBuilder.Append("global::UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<");
									typeBuilder.Append(addressableType.ToDisplayString(NullableFlowState.None, SymbolDisplayFormat.FullyQualifiedFormat));
									typeBuilder.Append(">");

									using (StringBuilderPool.Get(out StringBuilder nameBuilder))
									{
										nameBuilder.Append(field.Name);
										nameBuilder.Append("Handle");

										using (type.AddField(nameBuilder.ToString(), typeBuilder.ToString())) { }
									}
								}
							}

							using (NewScopes.MethodScope load = type.AddMethod("LoadAssets").WithAccessor(MethodAccessor.Private))
							{
								int i = 0;
								foreach ((IFieldSymbol field, INamedTypeSymbol addressableType, string valueName, bool generateSubscribeMethods,
								          bool _, bool generateInputCallbacks) in typeSyntax.fields)
								{
									load.Append(field.Name);
									load.Append("Handle = global::UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<");
									load.Append(addressableType.ToDisplayString(NullableFlowState.None, SymbolDisplayFormat.FullyQualifiedFormat));
									load.Append(">(");
									load.Append(field.Name);
									load.AppendLine(");");

									load.Append(field.Name);
									load.AppendLine("Handle.Completed += (op) =>");
									using (BlockScope lambdaBlock = load.AddBlock().AsLambda())
									{
										lambdaBlock.AppendLine(
											"if (op.Status == global::UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)");

										using (BlockScope ifBlock = lambdaBlock.AddBlock())
										{
											ifBlock.Append(valueName);
											ifBlock.AppendLine(" = op.Result;");

											if (generateSubscribeMethods)
											{
												ifBlock.Append("SubscribeTo");
												ifBlock.Append(TextUtility.FormatVariableLabel(valueName));
												ifBlock.AppendLine("();");
											}

											if (generateInputCallbacks)
											{
												ifBlock.Append("SubscribeTo");
												ifBlock.Append(TextUtility.FormatVariableLabel(valueName));
												ifBlock.AppendLine("();");
											}

											ifBlock.Append("On");
											ifBlock.Append(TextUtility.FormatVariableLabel(valueName));
											ifBlock.Append("Loaded(op.Result);");
										}

										lambdaBlock.AppendLine();
										lambdaBlock.AppendLine("else");

										using (BlockScope elseBlock = lambdaBlock.AddBlock())
										{
											elseBlock.Append("global::UnityEngine.Debug.LogError($\"Failed to load reference ");
											elseBlock.Append(field.Name);
											elseBlock.Append(" with error: {op.OperationException}\");");
										}
									}
									
									if (i < typeSyntax.fields.Length - 1)
									{
										load.AppendLine();
									}

									i++;
								}
							}

							using (NewScopes.MethodScope release = type.AddMethod("ReleaseAssets").WithAccessor(MethodAccessor.Private))
							{
								int i = 0;
								foreach ((IFieldSymbol field, INamedTypeSymbol _, string _, bool _, bool _, bool _) in typeSyntax.fields)
								{
									release.Append("if (");
									release.Append(field.Name);
									release.AppendLine("Handle.IsValid())");
									using (BlockScope ifBlock = release.AddBlock())
									{
										ifBlock.Append("global::UnityEngine.AddressableAssets.Addressables.Release(");
										ifBlock.Append(field.Name);
										ifBlock.Append("Handle);");
									}
									
									if (i < typeSyntax.fields.Length - 1)
									{
										release.AppendLine();
									}
									
									i++;
								}
							}

							foreach ((IFieldSymbol _, INamedTypeSymbol addressableType, string valueName, bool _, bool _, bool _) in typeSyntax.fields)
							{
								using (StringBuilderPool.Get(out StringBuilder? nameBuilder))
								{
									nameBuilder.Append("On");
									nameBuilder.Append(TextUtility.FormatVariableLabel(valueName));
									nameBuilder.Append("Loaded");

									using (NewScopes.MethodScope onLoadedMethod =
									       type.AddMethod(nameBuilder.ToString()).WithAccessor(MethodAccessor.None).Partial())
									{
										using (StringBuilderPool.Get(out StringBuilder? parameterBuilder))
										{
											parameterBuilder.Append("global::");
											parameterBuilder.Append(addressableType.ToDisplayString());
											onLoadedMethod.AddParameter(parameterBuilder.ToString(), "value");
										}
									}
								}
							}
						}
					}
				}
				catch (Exception e)
				{
					Log.LogError($"Failed to generate source. {e.Message}{Environment.NewLine}{e.StackTrace}");
				}
			}
		}
	}
}