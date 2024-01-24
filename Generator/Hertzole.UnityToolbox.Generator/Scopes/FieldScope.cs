using System;
using System.Collections.Generic;
using System.Text;
using Hertzole.UnityToolbox.Shared;

namespace Hertzole.UnityToolbox.Generator;

public enum FieldAccessor
{
	None,
	Public,
	Private,
	Protected,
	Internal,
	ProtectedInternal
}

public sealed class FieldScope : IDisposable
{
	private TypeScope type;
	private string fieldName;
	private string fieldType;
	private string? defaultValue;
	private List<string> attributes;
	private FieldAccessor accessor;

	private static readonly ObjectPool<FieldScope> pool = new ObjectPool<FieldScope>(() => new FieldScope(), null, null);

	private FieldScope()
	{
		type = null!;
		fieldName = null!;
		fieldType = null!;
		defaultValue = null;
		accessor = FieldAccessor.None;

		attributes = null!;
	}

	public static FieldScope Create(in TypeScope type, in string fieldName, in string fieldType, in string? defaultValue)
	{
		FieldScope scope = pool.Get();
		scope.type = type;
		scope.fieldName = fieldName;
		scope.fieldType = fieldType;
		scope.defaultValue = defaultValue;
		scope.accessor = FieldAccessor.Private;

		scope.attributes = ListPool<string>.Get();

		return scope;
	}

	public FieldScope WithAccessor(FieldAccessor value)
	{
		accessor = value;
		return this;
	}

	public void AddAttribute(string attribute)
	{
		attributes.Add(attribute);
	}

	public void Dispose()
	{
		using (StringBuilderPool.Get(out StringBuilder? fieldBuilder))
		{
			if (attributes.Count > 0)
			{
				for (int i = 0; i < attributes.Count; i++)
				{
					fieldBuilder.Append(type.source.GetIndent());
					fieldBuilder.Append("[global::");
					fieldBuilder.Append(attributes[i]);
					fieldBuilder.Append(']');
					fieldBuilder.AppendLine();
				}
			}

			fieldBuilder.Append(type.source.GetIndent());

			switch (accessor)
			{
				case FieldAccessor.None:
					break;
				case FieldAccessor.Public:
					fieldBuilder.Append("public ");
					break;
				case FieldAccessor.Private:
					fieldBuilder.Append("private ");
					break;
				case FieldAccessor.Protected:
					fieldBuilder.Append("protected ");
					break;
				case FieldAccessor.Internal:
					fieldBuilder.Append("internal ");
					break;
				case FieldAccessor.ProtectedInternal:
					fieldBuilder.Append("protected internal ");
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			fieldBuilder.Append(fieldType);
			fieldBuilder.Append(' ');
			fieldBuilder.Append(fieldName);

			if (!string.IsNullOrEmpty(defaultValue))
			{
				fieldBuilder.Append(" = ");
				fieldBuilder.Append(defaultValue);
			}

			fieldBuilder.Append(';');

			type.WithField(fieldBuilder.ToString());
		}

		type.source.Indent--;

		ListPool<string>.Return(attributes);

		pool.Return(this);
	}
}