using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading;
using Hertzole.UnityToolbox.Shared;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Hertzole.UnityToolbox.Generator;

[Generator(LanguageNames.CSharp)]
internal sealed class InputCallbacksGenerator : IIncrementalGenerator
{
	private static readonly ObjectPool<HashSet<string>> hashSetPool =
		new ObjectPool<HashSet<string>>(() => new HashSet<string>(StringComparer.OrdinalIgnoreCase), null, set => set.Clear());

	private readonly struct InputCallbacksType : IEquatable<InputCallbacksType>
	{
		public readonly INamedTypeSymbol? type;
		public readonly ImmutableArray<InputCallbackField> fields;

		public static IEqualityComparer<InputCallbacksType> TypeComparer { get; } = new TypeEqualityComparer();

		public InputCallbacksType(INamedTypeSymbol type, ImmutableArray<InputCallbackField> fields)
		{
			this.type = type ?? throw new ArgumentNullException(nameof(type));
			this.fields = fields;
		}

		private sealed class TypeEqualityComparer : IEqualityComparer<InputCallbacksType>
		{
			public bool Equals(InputCallbacksType x, InputCallbacksType y)
			{
				return x.Equals(y);
			}

			public int GetHashCode(InputCallbacksType obj)
			{
				return obj.GetHashCode();
			}
		}

		public bool Equals(InputCallbacksType other)
		{
			if (type == null)
			{
				return false;
			}

			if (other.type == null)
			{
				return false;
			}

			return type.StringEquals(other.type) && fields == other.fields;
		}

		public override bool Equals(object? obj)
		{
			return obj is InputCallbacksType other && Equals(other);
		}

		public override int GetHashCode()
		{
			return SymbolEqualityComparer.Default.GetHashCode(type);
		}

		public static bool operator ==(InputCallbacksType left, InputCallbacksType right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(InputCallbacksType left, InputCallbacksType right)
		{
			return !left.Equals(right);
		}
	}

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		Log.LogInfo("==== INITIALIZE INPUT CALLBACKS ====");

		IncrementalValuesProvider<InputCallbacksType> provider = context.SyntaxProvider.CreateSyntaxProvider(
			                                                                IsTypeTargetForGeneration,
			                                                                GetMemberDeclarationsForSourceGen)
		                                                                .Where(t => t.reportAttributeFound)
		                                                                .Select((t, _) => t.Item1)
		                                                                .WithComparer(InputCallbacksType.TypeComparer);

		context.RegisterSourceOutput(context.CompilationProvider.Combine(provider.Collect()), (ctx, t) => GenerateCode(ctx, t.Right));
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

	private static (InputCallbacksType, bool reportAttributeFound) GetMemberDeclarationsForSourceGen(GeneratorSyntaxContext context,
		CancellationToken cancellationToken)
	{
		TypeDeclarationSyntax typeDeclaration = (TypeDeclarationSyntax) context.Node;

		if (context.SemanticModel.GetDeclaredSymbol(typeDeclaration) is not INamedTypeSymbol symbol)
		{
			return (default, false);
		}

		ImmutableArray<InputCallbackField>.Builder fields = ImmutableArray.CreateBuilder<InputCallbackField>();
		using PoolHandle<HashSet<string>> handle = hashSetPool.Get(out HashSet<string> fieldNames);

		foreach (ISymbol memberDeclaration in symbol.GetMembers())
		{
			cancellationToken.ThrowIfCancellationRequested();

			if (memberDeclaration is not IFieldSymbol && memberDeclaration is not IPropertySymbol)
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

				if (!attributeData.AttributeClass.StringEquals(Attributes.GENERATE_INPUT_CALLBACKS))
				{
					continue;
				}

				if (attributeData.ConstructorArguments.Length != 1)
				{
					continue;
				}

				if (attributeData.ConstructorArguments[0].Value is string inputName)
				{
					InputCallbackType callbackType = InputCallbackType.None;

					ImmutableArray<KeyValuePair<string, TypedConstant>> namedArguments = attributeData.NamedArguments;
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

					ITypeSymbol? typeSymbol = null;
					string? fieldName = null;

					switch (memberDeclaration)
					{
						case IFieldSymbol field:
							typeSymbol = field.Type;
							fieldName = field.Name;
							break;
						case IPropertySymbol property:
							typeSymbol = property.Type;
							fieldName = property.Name;
							break;
					}

					if (typeSymbol is null || string.IsNullOrEmpty(fieldName))
					{
						continue;
					}

					string uniqueName = fieldName!;
					// Make sure the field name is unique.
					int counter = 0;
					while (!fieldNames.Add(uniqueName))
					{
						uniqueName = $"{fieldName}_{++counter}";
					}

					bool isAddressable = AddressablesHelper.TryGetAddressableType(typeSymbol, out _);
					string name = fieldName!;

					if (isAddressable)
					{
						name = TextUtility.FormatAddressableName(name);
					}

					fields.Add(new InputCallbackField(name, inputName, uniqueName, callbackType));
				}
			}
		}

		if (fields.Count == 0)
		{
			return (default, false);
		}

		return (new InputCallbacksType(symbol, fields.ToImmutable()), true);
	}

	private static void GenerateCode(SourceProductionContext context, ImmutableArray<InputCallbacksType> typesList)
	{
		if (typesList.IsDefaultOrEmpty)
		{
			return;
		}

		foreach (InputCallbacksType callbackType in typesList)
		{
			context.CancellationToken.ThrowIfCancellationRequested();

			bool isStruct = callbackType.type!.IsValueType;

			if (callbackType.fields.IsDefaultOrEmpty)
			{
				continue;
			}

			using PoolHandle<StringBuilder> nameBuilderHandle = StringBuilderPool.Get(out StringBuilder nameBuilder);

			nameBuilder.Append(callbackType.type.Name);
			nameBuilder.Append(".InputCallbacks");

			using (SourceScope source = SourceScope.Create(nameBuilder.ToString(), context)
			                                       .WithNamespace(callbackType.type.ContainingNamespace))
			{
				source.AddUsing("Hertzole.UnityToolbox");

				using (TypeScope type = source.WithType(callbackType.type.GetGenericFriendlyName(), isStruct ? TypeType.Struct : TypeType.Class)
				                              .WithAccessor(TypeAccessor.None).Partial())
				{
					foreach (InputCallbackField field in callbackType.fields)
					{
						type.WithField(field.hasSubscribedField, "bool", "false").WithAccessor(FieldAccessor.Private).Dispose();

						using (MethodScope subscribe = type.WithMethod(field.subscribeToField).WithAccessor(MethodAccessor.Private))
						{
							subscribe.Append("if (!");
							subscribe.Append(field.hasSubscribedField);
							subscribe.Append(" && ");
							subscribe.Append(field.name);
							subscribe.Append(" != null && ");
							subscribe.Append(field.inputName);
							subscribe.AppendLine(" != null)");

							using (BlockScope ifBlock = subscribe.WithBlock())
							{
								if ((field.callbackType & InputCallbackType.Started) != 0)
								{
									ifBlock.Append(field.inputName);
									ifBlock.Append(".AddStartedListener(");
									ifBlock.Append(field.name);
									ifBlock.Append(", ");
									ifBlock.Append(field.startedMethod!);
									ifBlock.AppendLine(");");
								}

								if ((field.callbackType & InputCallbackType.Performed) != 0)
								{
									ifBlock.Append(field.inputName);
									ifBlock.Append(".AddPerformedListener(");
									ifBlock.Append(field.name);
									ifBlock.Append(", ");
									ifBlock.Append(field.performedMethod!);
									ifBlock.AppendLine(");");
								}

								if ((field.callbackType & InputCallbackType.Canceled) != 0)
								{
									ifBlock.Append(field.inputName);
									ifBlock.Append(".AddCanceledListener(");
									ifBlock.Append(field.name);
									ifBlock.Append(", ");
									ifBlock.Append(field.canceledMethod!);
									ifBlock.AppendLine(");");
								}

								if ((field.callbackType & InputCallbackType.All) != 0)
								{
									ifBlock.Append(field.inputName);
									ifBlock.Append(".AddAllListeners(");
									ifBlock.Append(field.name);
									ifBlock.Append(", ");
									ifBlock.Append(field.allMethod!);
									ifBlock.AppendLine(");");
								}

								ifBlock.Append(field.hasSubscribedField);
								ifBlock.Append(" = true;");
							}
						}

						using (MethodScope unsubscribe = type.WithMethod(field.unsubscribeFromField).WithAccessor(MethodAccessor.Private))
						{
							unsubscribe.Append("if (");
							unsubscribe.Append(field.hasSubscribedField);
							unsubscribe.Append(" && ");
							unsubscribe.Append(field.name);
							unsubscribe.Append(" != null && ");
							unsubscribe.Append(field.inputName);
							unsubscribe.AppendLine(" != null)");

							using (BlockScope ifBlock = unsubscribe.WithBlock())
							{
								if ((field.callbackType & InputCallbackType.Started) != 0)
								{
									ifBlock.Append(field.inputName);
									ifBlock.Append(".RemoveStartedListener(");
									ifBlock.Append(field.name);
									ifBlock.Append(", ");
									ifBlock.Append(field.startedMethod!);
									ifBlock.AppendLine(");");
								}

								if ((field.callbackType & InputCallbackType.Performed) != 0)
								{
									ifBlock.Append(field.inputName);
									ifBlock.Append(".RemovePerformedListener(");
									ifBlock.Append(field.name);
									ifBlock.Append(", ");
									ifBlock.Append(field.performedMethod!);
									ifBlock.AppendLine(");");
								}

								if ((field.callbackType & InputCallbackType.Canceled) != 0)
								{
									ifBlock.Append(field.inputName);
									ifBlock.Append(".RemoveCanceledListener(");
									ifBlock.Append(field.name);
									ifBlock.Append(", ");
									ifBlock.Append(field.canceledMethod!);
									ifBlock.AppendLine(");");
								}

								if ((field.callbackType & InputCallbackType.All) != 0)
								{
									ifBlock.Append(field.inputName);
									ifBlock.Append(".RemoveAllListeners(");
									ifBlock.Append(field.name);
									ifBlock.Append(", ");
									ifBlock.Append(field.allMethod!);
									ifBlock.AppendLine(");");
								}

								ifBlock.Append(field.hasSubscribedField);
								ifBlock.Append(" = false;");
							}
						}

						if ((field.callbackType & InputCallbackType.Started) != 0)
						{
							using (MethodScope startedMethod = type.WithMethod(field.startedMethod!).WithAccessor(MethodAccessor.Private).Partial())
							{
								startedMethod.AddParameter("global::UnityEngine.InputSystem.InputAction.CallbackContext", "context");
							}
						}

						if ((field.callbackType & InputCallbackType.Performed) != 0)
						{
							using (MethodScope performedMethod =
							       type.WithMethod(field.performedMethod!).WithAccessor(MethodAccessor.Private).Partial())
							{
								performedMethod.AddParameter("global::UnityEngine.InputSystem.InputAction.CallbackContext", "context");
							}
						}

						if ((field.callbackType & InputCallbackType.Canceled) != 0)
						{
							using (MethodScope canceledMethod = type.WithMethod(field.canceledMethod!).WithAccessor(MethodAccessor.Private).Partial())
							{
								canceledMethod.AddParameter("global::UnityEngine.InputSystem.InputAction.CallbackContext", "context");
							}
						}

						if ((field.callbackType & InputCallbackType.All) != 0)
						{
							using (MethodScope allMethod = type.WithMethod(field.allMethod!).WithAccessor(MethodAccessor.Private).Partial())
							{
								allMethod.AddParameter("global::UnityEngine.InputSystem.InputAction.CallbackContext", "context");
							}
						}
					}
				}
			}
		}
	}
}