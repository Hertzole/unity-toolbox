﻿using System;
using System.Collections.Generic;
using System.Text;
using Hertzole.UnityToolbox.Shared;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Hertzole.UnityToolbox.Generator;

public sealed class SourceScope : IDisposable
{
	private static readonly StringBuilder sb = new StringBuilder();

	private readonly string sourceName;
	public readonly SourceProductionContext context;

	private readonly List<string> usings = new List<string>();
	private readonly List<string> writeCommands = new List<string>();

	public int Indent { get; set; }

	public SourceScope(string name, SourceProductionContext context)
	{
		sourceName = name;
		this.context = context;
	}

	public void Write(string? value = null, bool includeIndent = true)
	{
		writeCommands.Add(FormatWriteCommand(value, includeIndent));
	}

	public void WriteLine(string? value = null, bool includeIndent = true)
	{
		writeCommands.Add(FormatWriteCommand(value, includeIndent, true));
	}

	public TypeScope WithClass(string name)
	{
		return new TypeScope(this, name, TypeType.Class);
	}

	public TypeScope WithStruct(string name)
	{
		return new TypeScope(this, name, TypeType.Struct);
	}

	public TypeScope WithInterface(string name)
	{
		return new TypeScope(this, name, TypeType.Interface);
	}

	public void WriteUsing(string value)
	{
		usings.Add(value);
	}

	public string FormatWriteCommand(string? value = null, bool includeIndent = true, bool newLine = false)
	{
		return GetIndent(includeIndent ? Indent : 0) + value + (newLine ? Environment.NewLine : "");
	}

	public void Dispose()
	{
		sb.AppendLine("// <auto-generated>");
		sb.AppendLine("// \t\tThis file was generated by the Unity Toolbox Generator, by Hertzole.");
		sb.AppendLine("// \t\tDo not edit this file manually");
		sb.AppendLine("// </auto-generated>");
		sb.AppendLine();

		if (usings.Count > 0)
		{
			for (int i = 0; i < usings.Count; i++)
			{
				sb.AppendLine($"using {usings[i]};");
			}

			sb.AppendLine();
		}

		foreach (string command in writeCommands)
		{
			sb.Append(command);
		}

		context.AddSource($"{sourceName}.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
		
		Log.LogInfo($"Adding source '{sourceName}.g.cs'");

		sb.Clear();
	}

	public string GetIndent()
	{
		return GetIndent(Indent);
	}

	public string GetIndent(int indentAmount)
	{
		return new string('\t', indentAmount);
	}
}