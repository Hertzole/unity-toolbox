using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Hertzole.UnityToolbox.Generator;

public static class Log
{
	[Conditional("DEBUG")]
	public static void LogInfo(string message)
	{
#if DEBUG
		Write($"[INFO] {message}");
#endif
	}

	[Conditional("DEBUG")]
	public static void LogWarning(string message)
	{
#if DEBUG
		Write($"[WARNING] {message}");
#endif
	}

	[Conditional("DEBUG")]
	public static void LogError(string message)
	{
#if DEBUG
		Write($"[ERROR] {message}");
#endif
	}

#if DEBUG
	private static void Write(string value)
	{
		using (FileStream? stream = File.Open($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/UnityToolboxGenerator.log", FileMode.Append,
			       FileAccess.Write, FileShare.Read))
		{
			byte[] bytes = Encoding.UTF8.GetBytes($"[{DateTime.Now:HH:mm:ss.fff}] {value}{Environment.NewLine}");
			stream.Write(bytes, 0, bytes.Length);
		}
	}
#endif
}