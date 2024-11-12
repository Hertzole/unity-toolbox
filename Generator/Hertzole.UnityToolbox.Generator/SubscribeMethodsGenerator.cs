using System;
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
public sealed class SubscribeMethodsGenerator : IIncrementalGenerator
{
	private static readonly ObjectPool<HashSet<string>> hashSetPool =
		new ObjectPool<HashSet<string>>(() => new HashSet<string>(StringComparer.OrdinalIgnoreCase), null, set => set.Clear());

	/// <summary>
	///     Represents a type that can be subscribed to.
	/// </summary>
	/// <remarks>
	///     This struct is used to hold information about a type that can be subscribed to,
	///     including the type itself and the fields within the type that are relevant for subscription.
	///     It implements the IEquatable interface to allow for easy comparison of SubscribeType instances.
	/// </remarks>
	private readonly struct SubscribeType : IEquatable<SubscribeType>
	{
		public readonly INamedTypeSymbol? type;
		public readonly ImmutableArray<SubscribeField> fields;

		public static IEqualityComparer<SubscribeType> TypeComparer { get; } = new TypeEqualityComparer();

		public SubscribeType(INamedTypeSymbol type, ImmutableArray<SubscribeField> fields)
		{
			this.type = type ?? throw new ArgumentNullException(nameof(type));
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
		                                                           .Select((t, _) => t.Item1)
		                                                           .WithComparer(SubscribeType.TypeComparer);

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

	/// <summary>
	///     Gets the member declarations for source generation.
	/// </summary>
	/// <param name="context">The generator syntax context.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A tuple containing the SubscribeType and a boolean indicating if the attribute was found.</returns>
	/// <remarks>
	///     This method is used to get the member declarations for source generation. It checks if the type declaration
	///     is a named type symbol and if it has any attributes. If it does, it creates a SubscribeField for each member
	///     with the attribute and adds it to the fields list. If the fields list has any items, it returns a tuple with
	///     the SubscribeType and a boolean indicating that the attribute was found.
	/// </remarks>
	private static (SubscribeType, bool reportAttributeFound) GetMemberDeclarationsForSourceGen(GeneratorSyntaxContext context,
		CancellationToken cancellationToken)
	{
		TypeDeclarationSyntax typeDeclaration = (TypeDeclarationSyntax) context.Node;

		if (context.SemanticModel.GetDeclaredSymbol(typeDeclaration) is not INamedTypeSymbol typeSymbol)
		{
			return (default, false);
		}

		ImmutableArray<SubscribeField>.Builder fields = ImmutableArray.CreateBuilder<SubscribeField>();

		using PoolHandle<HashSet<string>> handle = hashSetPool.Get(out HashSet<string> fieldNames);

		foreach (ISymbol member in typeSymbol.GetMembers())
		{
			cancellationToken.ThrowIfCancellationRequested();

			if (member is not IFieldSymbol && member is not IPropertySymbol)
			{
				continue;
			}

			bool hasAttribute = false;
			bool hasSubscribeToChanging = false;
			bool hasGenerateLoadAttribute = false;

			foreach (AttributeData attribute in member.GetAttributes())
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

					if (attribute.ConstructorArguments.Length > 0 && attribute.ConstructorArguments[0].Value is bool subscribeToChanging)
					{
						hasSubscribeToChanging = subscribeToChanging;
					}
					
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

			ITypeSymbol fieldSymbol = member switch
			{
				IFieldSymbol field => field.Type,
				IPropertySymbol property => property.Type,
				_ => throw new ArgumentOutOfRangeException()
			};

			string fieldName = member.Name;
			string uniqueName = member.Name;
			if (hasGenerateLoadAttribute && AddressablesHelper.TryGetAddressableType(fieldSymbol, out _))
			{
				fieldName = TextUtility.FormatAddressableName(fieldSymbol.Name);
			}

			// Make sure the field name is unique.
			int counter = 0;
			while (!fieldNames.Add(uniqueName))
			{
				uniqueName = $"{fieldName}_{++counter}";
			}

			fields.Add(new SubscribeField(fieldSymbol, fieldName, uniqueName, hasSubscribeToChanging));
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

		using PoolHandle<StringBuilder> stringBuilderScope = StringBuilderPool.Get(out StringBuilder sb);

		foreach (SubscribeType subscribeType in typeList)
		{
			bool isStruct = subscribeType.type!.IsValueType;

			if (subscribeType.fields.IsDefaultOrEmpty)
			{
				continue;
			}

			using PoolHandle<StringBuilder> handle = StringBuilderPool.Get(out StringBuilder? typeNameBuilder);
			typeNameBuilder.Append(subscribeType.type.Name);
			typeNameBuilder.Append(".ScriptableValueSubscriptions");

			using (SourceScope source = SourceScope.Create(typeNameBuilder.ToString(), context)
			                                       .WithNamespace(subscribeType.type.ContainingNamespace))
			{
				using (TypeScope type = source.WithType(subscribeType.type.GetGenericFriendlyName(), isStruct ? TypeType.Struct : TypeType.Class)
				                              .WithAccessor(TypeAccessor.None).Partial())
				{
					foreach (SubscribeField field in subscribeType.fields)
					{
						if (field.FieldType is not INamedTypeSymbol fieldTypeSymbol ||
						    !ScriptableValueHelper.TryGetScriptableType(fieldTypeSymbol, out ScriptableType scriptableType, out ITypeSymbol? genericType))
						{
							continue;
						}

						sb.Clear();
						sb.Append(field.UniqueName);

						switch (scriptableType)
						{
							case ScriptableType.None:
								break;
							case ScriptableType.Event:
							case ScriptableType.GenericEvent:
								sb.Append("_OnInvoked");
								break;
							case ScriptableType.Value:
								sb.Append(field.SubscribeToChanging ? "_OnChanging" : "_OnChanged");
								break;
							default:
								throw new ArgumentOutOfRangeException();
						}

						string fieldName = sb.ToString();

						sb.Clear();

						string? fullTypeString = genericType?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

						switch (scriptableType)
						{
							case ScriptableType.None:
								break;
							case ScriptableType.Event:
								sb.Append("global::System.EventHandler");
								break;
							case ScriptableType.GenericEvent:
								sb.Append("global::System.EventHandler<");
								sb.Append(fullTypeString);
								sb.Append(">");
								break;
							case ScriptableType.Value:
								sb.Append("global::Hertzole.ScriptableValues.ScriptableValue<");
								sb.Append(fullTypeString);
								sb.Append(">.OldNewValue<");
								sb.Append(fullTypeString);
								sb.Append(">");
								break;
							default:
								throw new ArgumentOutOfRangeException();
						}

						string fieldType = sb.ToString();

						type.WithField(fieldName, fieldType, "").WithAccessor(FieldAccessor.Private).Dispose();
					}

					using (MethodScope createCallbacks = type.WithMethod("CreateScriptableValueCallbacks").WithAccessor(MethodAccessor.Private))
					{
						int writeCount = 0;

						foreach (SubscribeField field in subscribeType.fields)
						{
							if (field.FieldType is not INamedTypeSymbol fieldTypeSymbol ||
							    !ScriptableValueHelper.TryGetScriptableType(fieldTypeSymbol, out ScriptableType scriptableType, out ITypeSymbol? genericType))
							{
								continue;
							}

							if (writeCount > 0)
							{
								createCallbacks.AppendLine();
							}

							createCallbacks.Append(field.UniqueName);

							switch (scriptableType)
							{
								case ScriptableType.None:
									break;
								case ScriptableType.Event:
								case ScriptableType.GenericEvent:
									createCallbacks.Append("_OnInvoked");
									break;
								case ScriptableType.Value:
									createCallbacks.Append(field.SubscribeToChanging ? "_OnChanging" : "_OnChanged");
									break;
								default:
									throw new ArgumentOutOfRangeException();
							}

							string formattedValueName = TextUtility.FormatVariableLabel(field.UniqueName);

							createCallbacks.Append(" = On");
							createCallbacks.Append(formattedValueName);

							switch (scriptableType)
							{
								case ScriptableType.Event:
								case ScriptableType.GenericEvent:
									createCallbacks.Append("Invoked;");
									break;
								case ScriptableType.Value:
									createCallbacks.Append(field.SubscribeToChanging ? "Changing;" : "Changed;");
									break;
							}

							writeCount++;
						}
					}

					using (MethodScope subscribeALl = type.WithMethod("SubscribeToAllScriptableValues").WithAccessor(MethodAccessor.Private))
					{
						for (int i = 0; i < subscribeType.fields.Length; i++)
						{
							subscribeALl.Append("SubscribeTo");
							subscribeALl.Append(TextUtility.FormatVariableLabel(subscribeType.fields[i].UniqueName));
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

					using (MethodScope unsubscribeAll = type.WithMethod("UnsubscribeFromAllScriptableValues").WithAccessor(MethodAccessor.Private))
					{
						for (int i = 0; i < subscribeType.fields.Length; i++)
						{
							unsubscribeAll.Append("UnsubscribeFrom");
							unsubscribeAll.Append(TextUtility.FormatVariableLabel(subscribeType.fields[i].UniqueName));
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
						GenerateSubscribeMethods(type, subscribeType.fields[i]);
					}
				}
			}
		}
	}

	private static void GenerateSubscribeMethods(TypeScope type, in SubscribeField field)
	{
		INamedTypeSymbol typeSymbol = (INamedTypeSymbol) field.FieldType;

		if (!ScriptableValueHelper.TryGetScriptableType(typeSymbol, out ScriptableType scriptableType, out ITypeSymbol? genericType))
		{
			return;
		}

		string formattedValueName = TextUtility.FormatVariableLabel(field.UniqueName);
		using (StringBuilderPool.Get(out StringBuilder fieldNameBuilder))
		{
			fieldNameBuilder.Append("hasSubscribedTo");
			fieldNameBuilder.Append(formattedValueName);
			type.WithField(fieldNameBuilder.ToString(), "bool", "false").WithAccessor(FieldAccessor.Private).Dispose();
		}

		using PoolHandle<StringBuilder> handle = StringBuilderPool.Get(out StringBuilder nameBuilder);

		nameBuilder.Append("SubscribeTo");
		nameBuilder.Append(formattedValueName);

		using (MethodScope subscribe = type.WithMethod(nameBuilder.ToString()).WithAccessor(MethodAccessor.Private))
		{
			subscribe.Append("if (");
			subscribe.Append(field.FieldName);
			subscribe.Append(" != null && !hasSubscribedTo");
			subscribe.Append(formattedValueName);
			subscribe.AppendLine(")");

			using (BlockScope ifBlock = subscribe.WithBlock())
			{
				ifBlock.AppendLine("#if DEBUG || UNITY_EDITOR");
				ifBlock.Append("if (");
				ifBlock.Append(field.UniqueName);
				
				switch (scriptableType)
				{
					case ScriptableType.Event:
					case ScriptableType.GenericEvent:
						ifBlock.Append("_OnInvoked");
						break;
					case ScriptableType.Value:
						ifBlock.Append(field.SubscribeToChanging ? "_OnChanging" : "_OnChanged");
						break;
				}
				
				ifBlock.AppendLine(" == null)");
				ifBlock.AppendLine("{");
				type.source.Indent++;
				
				ifBlock.Append("throw new global::System.NullReferenceException(\"");
				ifBlock.Append("Missing callback for ");
				ifBlock.Append(field.UniqueName);
				ifBlock.AppendLine(". Did you call 'CreateScriptableValueCallbacks()' before subscribing to values?\");");
				
				type.source.Indent--;
				ifBlock.AppendLine("}");
				
				ifBlock.AppendLine("#endif");
				
				ifBlock.Append(field.FieldName);
				
				switch (scriptableType)
				{
					case ScriptableType.None:
						break;
					case ScriptableType.Event:
					case ScriptableType.GenericEvent:
						ifBlock.Append(".OnInvoked += ");
						ifBlock.Append(field.UniqueName);
						ifBlock.AppendLine("_OnInvoked;");
						break;
					case ScriptableType.Value:
						ifBlock.Append(".OnValue");
						ifBlock.Append(field.SubscribeToChanging ? "Changing" : "Changed");
						ifBlock.Append(" += ");
						ifBlock.Append(field.UniqueName);
						ifBlock.AppendLine(field.SubscribeToChanging ? "_OnChanging;" : "_OnChanged;");
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

		using (MethodScope unsubscribe = type.WithMethod(nameBuilder.ToString()).WithAccessor(MethodAccessor.Private))
		{
			unsubscribe.Append("if (");
			unsubscribe.Append(field.FieldName);
			unsubscribe.Append(" != null && hasSubscribedTo");
			unsubscribe.Append(formattedValueName);
			unsubscribe.AppendLine(")");

			using (BlockScope ifBlock = unsubscribe.WithBlock())
			{
				ifBlock.AppendLine("#if DEBUG || UNITY_EDITOR");
				ifBlock.Append("if (");
				ifBlock.Append(field.UniqueName);
				
				switch (scriptableType)
				{
					case ScriptableType.Event:
					case ScriptableType.GenericEvent:
						ifBlock.Append("_OnInvoked");
						break;
					case ScriptableType.Value:
						ifBlock.Append(field.SubscribeToChanging ? "_OnChanging" : "_OnChanged");
						break;
				}
				
				ifBlock.AppendLine(" == null)");
				ifBlock.AppendLine("{");
				type.source.Indent++;
				
				ifBlock.Append("throw new global::System.NullReferenceException(\"");
				ifBlock.Append("Missing callback for ");
				ifBlock.Append(field.UniqueName);
				ifBlock.AppendLine(". Did you call 'CreateScriptableValueCallbacks()' before subscribing to values?\");");
				
				type.source.Indent--;
				ifBlock.AppendLine("}");
				
				ifBlock.AppendLine("#endif");
				
				switch (scriptableType)
				{
					case ScriptableType.None:
						break;
					case ScriptableType.Event:
					case ScriptableType.GenericEvent:
						ifBlock.Append(field.FieldName);
						ifBlock.Append(".OnInvoked -= ");
						ifBlock.Append(field.UniqueName);
						ifBlock.AppendLine("_OnInvoked;");
						break;
					case ScriptableType.Value:
						ifBlock.Append(field.FieldName);
						ifBlock.Append(".OnValue");
						ifBlock.Append(field.SubscribeToChanging ? "Changing" : "Changed");
						ifBlock.Append(" -= ");
						ifBlock.Append(field.UniqueName);
						ifBlock.AppendLine(field.SubscribeToChanging ? "_OnChanging;" : "_OnChanged;");
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

				using (MethodScope m = type.WithMethod(nameBuilder.ToString()).Partial().WithAccessor(MethodAccessor.Private))
				{
					m.AddParameter("object", "sender");
					m.AddParameter("global::System.EventArgs", "e");
				}

				break;
			case ScriptableType.GenericEvent:
				nameBuilder.Append("On");
				nameBuilder.Append(formattedValueName);
				nameBuilder.Append("Invoked");

				using (MethodScope m = type.WithMethod(nameBuilder.ToString()).Partial().WithAccessor(MethodAccessor.Private))
				{
					m.AddParameter("object", "sender");
					m.AddParameter(genericType!.ToDisplayString(), "args");
				}

				break;
			case ScriptableType.Value:
				nameBuilder.Append("On");
				nameBuilder.Append(formattedValueName);
				nameBuilder.Append(field.SubscribeToChanging ? "Changing" : "Changed");

				string genericTypeString = genericType!.ToDisplayString();

				using (MethodScope m = type.WithMethod(nameBuilder.ToString()).Partial().WithAccessor(MethodAccessor.Private))
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