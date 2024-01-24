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
public sealed class SubscribeMethodsGenerator : IIncrementalGenerator
{
	private readonly struct SubscribeType : IEquatable<SubscribeType>
	{
		public readonly INamedTypeSymbol type;
		public readonly ImmutableArray<SubscribeField> fields;

		public static IEqualityComparer<SubscribeType> TypeComparer { get; } = new TypeEqualityComparer();

		public SubscribeType(INamedTypeSymbol type, ImmutableArray<SubscribeField> fields)
		{
			this.type = type;
			this.fields = fields;
		}

		private sealed class TypeEqualityComparer : IEqualityComparer<SubscribeType>
		{
			public bool Equals(SubscribeType x, SubscribeType y)
			{
				return SymbolEqualityComparer.Default.Equals(x.type, y.type);
			}

			public int GetHashCode(SubscribeType obj)
			{
				return SymbolEqualityComparer.Default.GetHashCode(obj.type);
			}
		}

		public bool Equals(SubscribeType other)
		{
			return string.Equals(type.ToDisplayString(NullableFlowState.NotNull, SymbolDisplayFormat.FullyQualifiedFormat),
				other.type.ToDisplayString(NullableFlowState.NotNull, SymbolDisplayFormat.FullyQualifiedFormat), StringComparison.Ordinal);
		}

		public override bool Equals(object? obj)
		{
			return obj is SubscribeType other && Equals(other);
		}

		public override int GetHashCode()
		{
			return SymbolEqualityComparer.Default.GetHashCode(type);
		}

		public static bool operator ==(SubscribeType left, SubscribeType right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(SubscribeType left, SubscribeType right)
		{
			return !left.Equals(right);
		}
	}

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		IncrementalValuesProvider<SubscribeType> provider = context.SyntaxProvider.CreateSyntaxProvider(
			                                                           IsClassTargetForGeneration,
			                                                           GetMemberDeclarationsForSourceGen)
		                                                           .Where(t => t.reportAttributeFound)
		                                                           .Select((t, _) => t.Item1);

		IncrementalValueProvider<(Compilation Left, ImmutableArray<SubscribeType> Right)> compilation =
			context.CompilationProvider.Combine(provider.Collect());

		context.RegisterSourceOutput(compilation, (productionContext, tuple) => Execute(productionContext, tuple.Right));
	}

	private static bool IsClassTargetForGeneration(SyntaxNode c, CancellationToken cancellationToken)
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

	private static (SubscribeType, bool reportAttributeFound) GetMemberDeclarationsForSourceGen(GeneratorSyntaxContext context,
		CancellationToken cancellationToken)
	{
		TypeDeclarationSyntax typeDeclaration = (TypeDeclarationSyntax) context.Node;

		if (context.SemanticModel.GetDeclaredSymbol(typeDeclaration) is not INamedTypeSymbol typeSymbol)
		{
			return (default, false);
		}

		ImmutableArray<SubscribeField>.Builder fields = ImmutableArray.CreateBuilder<SubscribeField>();

		foreach (ISymbol member in typeSymbol.GetMembers())
		{
			cancellationToken.ThrowIfCancellationRequested();

			if (member is not IFieldSymbol fieldSymbol)
			{
				continue;
			}

			bool hasAttribute = false;
			bool hasGenerateLoadAttribute = false;

			foreach (AttributeData attribute in fieldSymbol.GetAttributes())
			{
				cancellationToken.ThrowIfCancellationRequested();

				if (attribute.AttributeClass is null)
				{
					continue;
				}

				string attributeName = attribute.AttributeClass.ToDisplayString();

				if (string.Equals(Attributes.GENERATE_SUBSCRIBE_METHODS, attributeName, StringComparison.Ordinal))
				{
					hasAttribute = true;
				}
				else if (string.Equals(Attributes.GENERATE_LOAD, attributeName, StringComparison.Ordinal))
				{
					hasGenerateLoadAttribute = true;
				}

				if (hasAttribute && hasGenerateLoadAttribute)
				{
					break;
				}
			}

			if (!hasAttribute)
			{
				continue;
			}

			string fieldName = fieldSymbol.Name;
			if (hasGenerateLoadAttribute && AddressablesHelper.TryGetAddressableType(fieldSymbol.Type, out _))
			{
				fieldName = TextUtility.FormatAddressableName(fieldSymbol.Name);
			}

			fields.Add(new SubscribeField(fieldSymbol, fieldName));
		}

		if (fields.Count > 0)
		{
			return new ValueTuple<SubscribeType, bool>(new SubscribeType(typeSymbol, fields.ToImmutable()), true);
		}

		return (default, false);
	}

	private static void Execute(SourceProductionContext context, ImmutableArray<SubscribeType> typeList)
	{
		if (typeList.IsDefaultOrEmpty)
		{
			return;
		}

		foreach (SubscribeType subscribeType in typeList.Distinct(SubscribeType.TypeComparer))
		{
			bool isStruct = subscribeType.type.IsValueType;

			if (subscribeType.fields.IsDefaultOrEmpty)
			{
				continue;
			}

			using PoolHandle<StringBuilder> handle = StringBuilderPool.Get(out StringBuilder? typeNameBuilder);
			typeNameBuilder.Append(subscribeType.type.Name);
			typeNameBuilder.Append(".ScriptableValueSubscriptions");

			using (NewScopes.SourceScope source = NewScopes.SourceScope.Create(typeNameBuilder.ToString(), context)
			                                               .WithNamespace(subscribeType.type.ContainingNamespace))
			{
				using (NewScopes.TypeScope type = source.WithType(subscribeType.type.GetGenericFriendlyName(), isStruct ? TypeType.Struct : TypeType.Class)
				                                        .WithAccessor(TypeAccessor.None).Partial())
				{
					using (NewScopes.MethodScope subscribeALl = type.AddMethod("SubscribeToAllScriptableValues").WithAccessor(MethodAccessor.Private))
					{
						for (int i = 0; i < subscribeType.fields.Length; i++)
						{
							subscribeALl.Append("SubscribeTo");
							subscribeALl.Append(TextUtility.FormatVariableLabel(subscribeType.fields[i].FieldName));
							if (i < subscribeType.fields.Length - 1)
							{
								subscribeALl.AppendLine("();");
							}
							else
							{
								subscribeALl.Append("();");
							}
						}
					}

					using (NewScopes.MethodScope unsubscribeAll = type.AddMethod("UnsubscribeFromAllScriptableValues").WithAccessor(MethodAccessor.Private))
					{
						for (int i = 0; i < subscribeType.fields.Length; i++)
						{
							unsubscribeAll.Append("UnsubscribeFrom");
							unsubscribeAll.Append(TextUtility.FormatVariableLabel(subscribeType.fields[i].FieldName));
							if (i < subscribeType.fields.Length - 1)
							{
								unsubscribeAll.AppendLine("();");
							}
							else
							{
								unsubscribeAll.Append("();");
							}
						}
					}

					for (int i = 0; i < subscribeType.fields.Length; i++)
					{
						GenerateSubscribeMethods(type, subscribeType.fields[i].Field.Type as INamedTypeSymbol, subscribeType.fields[i].FieldName);
					}
				}
			}
		}
	}

	public static void GenerateSubscribeMethods(NewScopes.TypeScope type, INamedTypeSymbol? typeSymbol, string valueName)
	{
		if (!ScriptableValueHelper.TryGetScriptableType(typeSymbol, out ScriptableType scriptableType, out ITypeSymbol? genericType))
		{
			return;
		}

		string formattedValueName = TextUtility.FormatVariableLabel(valueName);
		type.AddField($"hasSubscribedTo{formattedValueName}", "bool", "false").WithAccessor(FieldAccessor.Private).Dispose();

		using PoolHandle<StringBuilder> handle = StringBuilderPool.Get(out StringBuilder nameBuilder);

		nameBuilder.Append("SubscribeTo");
		nameBuilder.Append(formattedValueName);

		using (NewScopes.MethodScope subscribe = type.AddMethod(nameBuilder.ToString()).WithAccessor(MethodAccessor.Private))
		{
			subscribe.Append("if (");
			subscribe.Append(valueName);
			subscribe.Append(" != null && !hasSubscribedTo");
			subscribe.Append(formattedValueName);
			subscribe.AppendLine(")");

			using (BlockScope ifBlock = subscribe.AddBlock())
			{
				switch (scriptableType)
				{
					case ScriptableType.None:
						break;
					case ScriptableType.Event:
					case ScriptableType.GenericEvent:
						ifBlock.Append(valueName);
						ifBlock.Append(".OnInvoked += On");
						ifBlock.Append(formattedValueName);
						ifBlock.AppendLine("Invoked;");
						break;
					case ScriptableType.Value:
						ifBlock.Append(valueName);
						ifBlock.Append(".OnValueChanged += On");
						ifBlock.Append(formattedValueName);
						ifBlock.AppendLine("Changed;");
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				ifBlock.Append("hasSubscribedTo");
				ifBlock.Append(formattedValueName);
				ifBlock.Append(" = true;");
			}
		}

		nameBuilder.Clear();

		nameBuilder.Append("UnsubscribeFrom");
		nameBuilder.Append(formattedValueName);

		using (NewScopes.MethodScope unsubscribe = type.AddMethod(nameBuilder.ToString()).WithAccessor(MethodAccessor.Private))
		{
			unsubscribe.Append("if (");
			unsubscribe.Append(valueName);
			unsubscribe.Append(" != null && hasSubscribedTo");
			unsubscribe.Append(formattedValueName);
			unsubscribe.AppendLine(")");

			using (BlockScope ifBlock = unsubscribe.AddBlock())
			{
				switch (scriptableType)
				{
					case ScriptableType.None:
						break;
					case ScriptableType.Event:
					case ScriptableType.GenericEvent:
						ifBlock.Append(valueName);
						ifBlock.Append(".OnInvoked -= On");
						ifBlock.Append(formattedValueName);
						ifBlock.AppendLine("Invoked;");
						break;
					case ScriptableType.Value:
						ifBlock.Append(valueName);
						ifBlock.Append(".OnValueChanged -= On");
						ifBlock.Append(formattedValueName);
						ifBlock.AppendLine("Changed;");
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				ifBlock.Append("hasSubscribedTo");
				ifBlock.Append(formattedValueName);
				ifBlock.Append(" = false;");
			}
		}

		nameBuilder.Clear();

		switch (scriptableType)
		{
			case ScriptableType.None:
				break;
			case ScriptableType.Event:
				nameBuilder.Append("On");
				nameBuilder.Append(formattedValueName);
				nameBuilder.Append("Invoked");

				using (NewScopes.MethodScope m = type.AddMethod(nameBuilder.ToString()).Partial().WithAccessor(MethodAccessor.Private))
				{
					m.AddParameter("object", "sender");
					m.AddParameter("global::System.EventArgs", "e");
				}

				break;
			case ScriptableType.GenericEvent:
				nameBuilder.Append("On");
				nameBuilder.Append(formattedValueName);
				nameBuilder.Append("Invoked");

				using (NewScopes.MethodScope m = type.AddMethod(nameBuilder.ToString()).Partial().WithAccessor(MethodAccessor.Private))
				{
					m.AddParameter("object", "sender");
					m.AddParameter(genericType!.ToDisplayString(), "args");
				}

				break;
			case ScriptableType.Value:
				nameBuilder.Append("On");
				nameBuilder.Append(formattedValueName);
				nameBuilder.Append("Changed");

				string genericTypeString = genericType!.ToDisplayString();

				using (NewScopes.MethodScope m = type.AddMethod(nameBuilder.ToString()).Partial().WithAccessor(MethodAccessor.Private))
				{
					m.AddParameter(genericTypeString, "previousValue");
					m.AddParameter(genericTypeString, "newValue");
				}

				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
}