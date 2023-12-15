// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;

namespace Hertzole.UnityToolbox.Generator.Benchmarks;

internal static class Program
{
	public static void Main(string[] args)
	{
		BenchmarkRunner.Run<AddressableLoadGeneratorBenchmarks>();
	}
}