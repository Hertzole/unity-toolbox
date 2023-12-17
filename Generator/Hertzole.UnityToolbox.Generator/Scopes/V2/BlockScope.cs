using System;
using System.Collections.Generic;
using System.Text;
using Hertzole.UnityToolbox.Shared;

namespace Hertzole.UnityToolbox.Generator.NewScopes;

public sealed class BlockScope : IDisposable
{
	private SourceScope source;
	private MethodScope? methodParent;
	private BlockScope? blockParent;
	private StringBuilder sb;
	private List<string> blocks;
	
	private bool hasIndentedBody;
	private bool shouldIndentWrite;
	private bool isLambda;

	private static readonly ObjectPool<BlockScope> pool = new ObjectPool<BlockScope>(() => new BlockScope(), null, null);

	private BlockScope()
	{
		source = null!;
		methodParent = null;
		blockParent = null;

		sb = null!;
		blocks = null!;
	}

	public static BlockScope Create(in MethodScope? parent, in BlockScope? blockParent)
	{
		BlockScope scope = pool.Get();
		scope.methodParent = parent;
		scope.blockParent = blockParent;
		
		scope.source = parent?.type.source ?? blockParent?.source ?? throw new ArgumentNullException(nameof(parent), "Both parent and blockParent are null.");

		scope.sb = StringBuilderPool.Get();
		scope.blocks = ListPool<string>.Get();

		scope.hasIndentedBody = false;
		scope.shouldIndentWrite = true;
		scope.isLambda = false;

		return scope;
	}
	
	public BlockScope AsLambda()
	{
		isLambda = true;
		return this;
	}

	public void Append(string value)
	{
		IndentBodyIfNeeded();
		WriteIndentIfNeeded();
		
		sb.Append(value);
	}

	public void AppendLine(string value)
	{
		IndentBodyIfNeeded();
		WriteIndentIfNeeded();
        
		sb.AppendLine(value);

		shouldIndentWrite = true;
	}

	public void AppendLine()
	{
		sb.AppendLine();
		shouldIndentWrite = true;
	}
	
	public void AppendBlock(string value, bool asLambda)
	{
		IndentBodyIfNeeded();
		WriteIndentIfNeeded();
		
		sb.AppendLine("{");
		sb.AppendLine(value);
		sb.Append(source.GetIndent());
		sb.Append(asLambda ? "};" : "}");
	}

	public BlockScope AddBlock()
	{
		return Create(null, this);
	}
	
	private void IndentBodyIfNeeded()
	{
		if (!hasIndentedBody)
		{
			source.Indent++;
			hasIndentedBody = true;
		}
	}

	private void WriteIndentIfNeeded()
	{
		if(shouldIndentWrite)
		{
			sb.Append(source.GetIndent());
			shouldIndentWrite = false;
		}
	}

	public void Dispose()
	{
		if (hasIndentedBody)
		{
			source.Indent--;
			hasIndentedBody = false;
		}
		
		string block = sb.ToString();

		methodParent?.AppendBlock(block, isLambda);
		blockParent?.AppendBlock(block, isLambda);

		StringBuilderPool.Return(sb);
		ListPool<string>.Return(blocks);
		
		pool.Return(this);
	}
}