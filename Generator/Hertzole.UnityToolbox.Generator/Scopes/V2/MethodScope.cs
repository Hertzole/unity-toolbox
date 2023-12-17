using System;
using System.Collections.Generic;
using System.Text;
using Hertzole.UnityToolbox.Shared;

namespace Hertzole.UnityToolbox.Generator.NewScopes;

public sealed class MethodScope : IDisposable
{
	internal TypeScope type;
	private string name;
	private string returnType;
	private List<Parameter> parameters;
	private StringBuilder sb;

	private MethodAccessor accessor;
	private bool isPartial;
	private bool hasIndentedBody;
	private bool shouldIndentWrite;

	private StringBuilder bodyWriter;

	private static readonly ObjectPool<MethodScope> pool = new ObjectPool<MethodScope>(() => new MethodScope(), null, null);

	private MethodScope()
	{
		type = null!;
		name = null!;

		accessor = MethodAccessor.None;
		isPartial = false;
		returnType = string.Empty;

		parameters = null!;
		sb = null!;
		bodyWriter = null!;
	}

	public static MethodScope Create(in TypeScope type, in string name)
	{
		MethodScope scope = pool.Get();
		scope.type = type;
		scope.name = name;

		scope.accessor = MethodAccessor.None;
		scope.isPartial = false;
		scope.returnType = "void";
		scope.shouldIndentWrite = true;
		scope.hasIndentedBody = false;

		scope.parameters = ListPool<Parameter>.Get();
		scope.sb = StringBuilderPool.Get();
		scope.bodyWriter = StringBuilderPool.Get();

		return scope;
	}

	public MethodScope WithAccessor(MethodAccessor value)
	{
		accessor = value;
		return this;
	}

	public void Append(string value)
	{
		IndentBodyIfNeeded();
		WriteIndentIfNeeded();
		
		bodyWriter.Append(value);
	}

	public void AppendLine(string value)
	{
		IndentBodyIfNeeded();
		WriteIndentIfNeeded();
		
		bodyWriter.AppendLine(value);
		shouldIndentWrite = true;
	}

	public void AppendLine()
	{
		bodyWriter.AppendLine();
		shouldIndentWrite = true;
	}

	public void AppendBlock(string value, bool asLambda)
	{
		IndentBodyIfNeeded();
		WriteIndentIfNeeded();
		
		bodyWriter.AppendLine("{");
		bodyWriter.AppendLine(value);
		bodyWriter.Append(type.source.GetIndent());
		bodyWriter.Append(asLambda ? "};" : "}");
		
		shouldIndentWrite = true;
	}

	public BlockScope AddBlock()
	{
		return BlockScope.Create(this, null);
	}

	public void AddParameter(string parameterType, string parameterName)
	{
		parameters.Add(new Parameter(parameterType, parameterName));
	}

	public MethodScope Partial()
	{
		isPartial = true;
		return this;
	}

	private void IndentBodyIfNeeded()
	{
		if (!hasIndentedBody)
		{
			type.source.Indent++;
			hasIndentedBody = true;
		}
	}

	private void WriteIndentIfNeeded()
	{
		if(shouldIndentWrite)
		{
			bodyWriter.Append(type.source.GetIndent());
			shouldIndentWrite = false;
		}
	}

	public void Dispose()
	{
		if (hasIndentedBody)
		{
			type.source.Indent--;
			hasIndentedBody = false;
		}
        
		sb.Append(type.source.GetIndent());
		
		switch (accessor)
		{
			case MethodAccessor.None:
				break;
			case MethodAccessor.Public:
				sb.Append("public ");
				break;
			case MethodAccessor.Private:
				sb.Append("private ");
				break;
			case MethodAccessor.Protected:
				sb.Append("protected ");
				break;
			case MethodAccessor.Internal:
				sb.Append("internal ");
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
		
		if (isPartial)
		{
			sb.Append("partial ");
		}
		
		sb.Append(returnType);
		sb.Append(' ');
		sb.Append(name);
		
		sb.Append('(');

		if (parameters.Count > 0)
		{
			for (int i = 0; i < parameters.Count; i++)
			{
				sb.Append(parameters[i].type);
				sb.Append(' ');
				sb.Append(parameters[i].name);

				if (i < parameters.Count - 1)
				{
					sb.Append(", ");
				}
			}
		}
		
		sb.Append(')');

		if (bodyWriter.Length > 0)
		{
			sb.AppendLine();
			sb.Append(type.source.GetIndent());
			sb.AppendLine("{");

			sb.AppendLine(bodyWriter.ToString());
		
			sb.Append(type.source.GetIndent());
			sb.Append("}");
		}
		else
		{
			sb.Append(';');
		}
		
		type.AddMethodBody(sb.ToString());
		type.source.Indent--;
		
		ListPool<Parameter>.Return(parameters);
		StringBuilderPool.Return(bodyWriter);
		StringBuilderPool.Return(sb);

		pool.Return(this);
	}

	private readonly struct Parameter
	{
		public readonly string type;
		public readonly string name;

		public Parameter(string type, string name)
		{
			this.type = type;
			this.name = name;
		}
	}
}