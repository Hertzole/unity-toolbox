﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading;
using Hertzole.UnityToolbox.Generator.Data;
using Hertzole.UnityToolbox.Shared;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Hertzole.UnityToolbox.Generator;

[Generator(LanguageNames.CSharp)]
public sealed class AddressableLoadGenerator : IIncrementalGenerator
{
	private static readonly ObjectPool<HashSet<string>> hashSetPool =
		new ObjectPool<HashSet<string>>(() => new HashSet<string>(StringComparer.OrdinalIgnoreCase), null, set => set.Clear());

	private readonly struct AddressableLoadType : IEquatable<AddressableLoadType>
	{
		public readonly INamedTypeSymbol? type;
		public readonly ImmutableArray<AddressableLoadField> fields;

		public static IEqualityComparer<AddressableLoadType> TypeComparer { get; } = new TypeEqualityComparer();

		public AddressableLoadType(INamedTypeSymbol type, ImmutableArray<AddressableLoadField> fields)
		{
			this.type = type ?? throw new ArgumentNullException(nameof(type));
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
			if (type == null)
			{
				return false;
			}

			if (other.type == null)
			{
				return false;
			}

			return type.StringEquals(other.type) && fields.Equals(other.fields);
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
		                                                                 .Select((t, _) => t.Item1)
		                                                                 .WithComparer(AddressableLoadType.TypeComparer);

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

		using PoolHandle<HashSet<string>> handle = hashSetPool.Get(out HashSet<string> fieldNames);

		ImmutableArray<AddressableLoadField>.Builder fields = ImmutableArray.CreateBuilder<AddressableLoadField>();

		foreach (ISymbol member in typeSymbol.GetMembers())
		{
			cancellationToken.ThrowIfCancellationRequested();

			INamedTypeSymbol? memberType = member switch
			{
				IFieldSymbol field => field.Type as INamedTypeSymbol,
				IPropertySymbol property => property.Type as INamedTypeSymbol,
				_ => null
			};

			if (memberType == null)
			{
				continue;
			}

			if (!AddressablesHelper.TryGetAddressableType(memberType, out INamedTypeSymbol? addressableType) || addressableType == null)
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
			string uniqueName = valueName;
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

			bool generateSubscribeMethods = member.TryGetAttribute(Attributes.GENERATE_SUBSCRIBE_METHODS, out _);
			bool generateInputCallbacks = member.TryGetAttribute(Attributes.GENERATE_INPUT_CALLBACKS, out _);

			// Make sure the field name is unique.
			int counter = 0;
			while (!fieldNames.Add(uniqueName))
			{
				uniqueName = $"{valueName}_{++counter}";
			}

			if (hasGenerateLoadAttribute)
			{
				AddressableNameFields names = new AddressableNameFields(member.Name, valueName, uniqueName);
				fields.Add(new AddressableLoadField(memberType, addressableType, names, generateSubscribeMethods, fieldExists, generateInputCallbacks));
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

		foreach (AddressableLoadType typeSyntax in typeList)
		{
			bool isStruct = typeSyntax.type!.IsValueType;

			if (typeSyntax.fields.IsDefaultOrEmpty)
			{
				continue;
			}

			using PoolHandle<StringBuilder> handle = StringBuilderPool.Get(out StringBuilder? typeNameBuilder);
			typeNameBuilder.Append(typeSyntax.type.Name);
			typeNameBuilder.Append(".Addressables");

			using (SourceScope source = SourceScope.Create(typeNameBuilder.ToString(), context)
			                                       .WithNamespace(typeSyntax.type.ContainingNamespace))
			{
				using (TypeScope type = source.WithType(typeSyntax.type.GetGenericFriendlyName(), isStruct ? TypeType.Struct : TypeType.Class)
				                              .WithAccessor(TypeAccessor.None)
				                              .Partial())
				{
					foreach ((INamedTypeSymbol _, INamedTypeSymbol addressableType, AddressableNameFields names, bool _, bool fieldExists, bool _) in
					         typeSyntax.fields)
					{
						if (!fieldExists)
						{
							using (FieldScope field = type.WithField(names.UniqueName,
								       addressableType.ToDisplayString(NullableFlowState.None, SymbolDisplayFormat.FullyQualifiedFormat), "null"))
							{
								field.AddAttribute("JetBrains.Annotations.CanBeNull");
							}
						}
					}

					foreach ((INamedTypeSymbol _, INamedTypeSymbol addressableType, AddressableNameFields names, bool _, bool _, bool _) in typeSyntax.fields)
					{
						using (StringBuilderPool.Get(out StringBuilder typeBuilder))
						{
							typeBuilder.Append("global::UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<");
							typeBuilder.Append(addressableType.ToDisplayString(NullableFlowState.None, SymbolDisplayFormat.FullyQualifiedFormat));
							typeBuilder.Append(">");

							using (StringBuilderPool.Get(out StringBuilder nameBuilder))
							{
								nameBuilder.Append(names.UniqueName);
								nameBuilder.Append("Handle");

								using (type.WithField(nameBuilder.ToString(), typeBuilder.ToString())) { }
							}
						}
					}

					using (MethodScope load = type.WithMethod("LoadAssets").WithAccessor(MethodAccessor.Private))
					{
						int i = 0;
						foreach ((INamedTypeSymbol _, INamedTypeSymbol addressableType, AddressableNameFields names, bool generateSubscribeMethods,
						          bool _, bool generateInputCallbacks) in typeSyntax.fields)
						{
							load.Append(names.UniqueName);
							load.Append("Handle = global::UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<");
							load.Append(addressableType.ToDisplayString(NullableFlowState.None, SymbolDisplayFormat.FullyQualifiedFormat));
							load.Append(">(");
							load.Append(names.FieldName);
							load.AppendLine(");");

							load.Append(names.UniqueName);
							load.AppendLine("Handle.Completed += (op) =>");
							using (BlockScope lambdaBlock = load.WithBlock().AsLambda())
							{
								lambdaBlock.AppendLine(
									"if (op.Status == global::UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)");

								using (BlockScope ifBlock = lambdaBlock.AddBlock())
								{
									ifBlock.Append(names.UniqueName);
									ifBlock.AppendLine(" = op.Result;");

									if (generateSubscribeMethods)
									{
										ifBlock.Append("SubscribeTo");
										ifBlock.Append(TextUtility.FormatVariableLabel(names.UniqueName));
										ifBlock.AppendLine("();");
									}

									if (generateInputCallbacks)
									{
										ifBlock.Append("SubscribeTo");
										ifBlock.Append(TextUtility.FormatVariableLabel(names.UniqueName));
										ifBlock.AppendLine("();");
									}

									ifBlock.Append("On");
									ifBlock.Append(TextUtility.FormatVariableLabel(names.UniqueName));
									ifBlock.Append("Loaded(op.Result);");
								}

								lambdaBlock.AppendLine();
								lambdaBlock.AppendLine("else");

								using (BlockScope elseBlock = lambdaBlock.AddBlock())
								{
									elseBlock.Append("global::UnityEngine.Debug.LogError($\"Failed to load reference ");
									elseBlock.Append(names.FieldName);
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

					using (MethodScope release = type.WithMethod("ReleaseAssets").WithAccessor(MethodAccessor.Private))
					{
						int i = 0;
						foreach ((INamedTypeSymbol _, INamedTypeSymbol _, AddressableNameFields names, bool _, bool _, bool _) in typeSyntax.fields)
						{
							release.Append("if (");
							release.Append(names.UniqueName);
							release.AppendLine("Handle.IsValid())");
							using (BlockScope ifBlock = release.WithBlock())
							{
								ifBlock.Append("global::UnityEngine.AddressableAssets.Addressables.Release(");
								ifBlock.Append(names.UniqueName);
								ifBlock.Append("Handle);");
							}

							if (i < typeSyntax.fields.Length - 1)
							{
								release.AppendLine();
							}

							i++;
						}
					}

					foreach ((INamedTypeSymbol _, INamedTypeSymbol addressableType, AddressableNameFields names, bool _, bool _, bool _) in typeSyntax.fields)
					{
						using (StringBuilderPool.Get(out StringBuilder? nameBuilder))
						{
							nameBuilder.Append("On");
							nameBuilder.Append(TextUtility.FormatVariableLabel(names.UniqueName));
							nameBuilder.Append("Loaded");

							using (MethodScope onLoadedMethod =
							       type.WithMethod(nameBuilder.ToString()).WithAccessor(MethodAccessor.None).Partial())
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
	}
}