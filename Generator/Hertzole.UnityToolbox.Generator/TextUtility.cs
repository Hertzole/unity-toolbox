using System;
using System.Text;
using Hertzole.UnityToolbox.Generator.Pooling;

namespace Hertzole.UnityToolbox.Generator;

public static class TextUtility
{
	private const int REFERENCE_LENGTH = 9;

	private static void RemovePrefix(StringBuilder sb)
	{
		if (sb.Length > 2 && sb[1] == '_')
		{
			sb.Remove(0, 2);
		}
		else if (sb.Length > 1 && sb[0] == '_')
		{
			sb.Remove(0, 1);
		}
	}

	private static void UppercaseStart(StringBuilder builder)
	{
		builder[0] = char.ToUpper(builder[0]);
	}

	public static string FormatVariableLabel(string label)
	{
		if (string.IsNullOrEmpty(label))
		{
			return string.Empty;
		}

		using (ObjectPool<StringBuilder>.Get(out StringBuilder sb))
		{
			sb.Clear();
			sb.Append(label);
			RemovePrefix(sb);
			UppercaseStart(sb);
			return sb.ToString();
		}
	}

	public static string FormatAddressableName(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return string.Empty;
		}

		using (ObjectPool<StringBuilder>.Get(out StringBuilder sb))
		{
			sb.Clear();
			sb.Append(name);
			if (name.EndsWith("reference", StringComparison.OrdinalIgnoreCase))
			{
				sb.Remove(name.Length - REFERENCE_LENGTH, REFERENCE_LENGTH);
			}
			else
			{
				sb.Append("Value");
			}

			return sb.ToString();
		}
	}
}