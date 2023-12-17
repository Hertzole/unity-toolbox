using System;
using System.Collections.Generic;
using System.Text;
using Hertzole.UnityToolbox.Shared;

namespace Hertzole.UnityToolbox.Generator.NewScopes;

public sealed class TypeScope : IDisposable
{
	internal SourceScope source;
	private string name;
	private TypeType type;

	private TypeAccessor accessor;
	private bool isPartial;
	
	private StringBuilder sb;
	private List<string> fields;
	private List<string> methods;

	private static readonly ObjectPool<TypeScope> pool = new ObjectPool<TypeScope>(() => new TypeScope(), null, null);

	private TypeScope()
	{
		source = null!;
		name = string.Empty;
		type = TypeType.Class;

		accessor = TypeAccessor.None;
		isPartial = false;

		sb = null!;
		fields = null!;
		methods = null!;
	}

	public static TypeScope Create(in SourceScope source, in string name, in TypeType type)
	{
		TypeScope scope = pool.Get();
		scope.source = source;
		scope.name = name;
		scope.type = type;

		scope.accessor = TypeAccessor.None;
		scope.isPartial = false;

		scope.sb = StringBuilderPool.Get();
		scope.fields = ListPool<string>.Get();
		scope.methods = ListPool<string>.Get();

		return scope;
	}

	public TypeScope WithAccessor(TypeAccessor value)
	{
		accessor = value;
		return this;
	}

	public MethodScope AddMethod(string methodName)
	{
		source.Indent++;
		return MethodScope.Create(this, methodName);
	}

	internal void AddMethodBody(string method)
	{
		methods.Add(method);
	}

	public TypeScope Partial()
	{
		isPartial = true;
		return this;
	}

	public FieldScope AddField(string fieldName, string fieldType, string? defaultValue = null)
	{
		source.Indent++;
		return FieldScope.Create(this, fieldName, fieldType, defaultValue);
	}

	public void AddField(string field)
	{
		fields.Add(field);
	}

	public void Dispose()
	{
		sb.Append(source.GetIndent());

		switch (accessor)
		{
			case TypeAccessor.None:
				break;
			case TypeAccessor.Public:
				sb.Append("public ");
				break;
			case TypeAccessor.Private:
				sb.Append("private ");
				break;
			case TypeAccessor.Internal:
				sb.Append("internal ");
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		if (isPartial)
		{
			sb.Append("partial ");
		}

		switch (type)
		{
			case TypeType.Class:
				sb.Append("class ");
				break;
			case TypeType.Struct:
				sb.Append("struct ");
				break;
			case TypeType.Interface:
				sb.Append("interface ");
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		sb.AppendLine(name);
		sb.Append(source.GetIndent());
		sb.AppendLine("{");

		source.Indent++;

		if (fields.Count > 0)
		{
			for (int i = 0; i < fields.Count; i++)
			{
				sb.AppendLine(fields[i]);
			}

			sb.AppendLine();
		}

		if (methods.Count > 0)
		{
			for (int i = 0; i < methods.Count; i++)
			{
				sb.AppendLine(methods[i]);

				if (i < methods.Count - 1)
				{
					sb.AppendLine();
				}
			}
		}

		source.Indent--;

		sb.Append(source.GetIndent());
		sb.Append("}");

		source.AddType(sb.ToString());

		StringBuilderPool.Return(sb);
		ListPool<string>.Return(fields);
		ListPool<string>.Return(methods);

		pool.Return(this);
	}
}