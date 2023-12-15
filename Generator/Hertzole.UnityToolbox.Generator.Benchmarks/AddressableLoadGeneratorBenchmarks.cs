using BenchmarkDotNet.Attributes;
using Hertzole.UnityToolbox.Generator.Tests;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Hertzole.UnityToolbox.Generator.Benchmarks;

[MemoryDiagnoser]
public class AddressableLoadGeneratorBenchmarks
{
	private CSharpGeneratorDriver driver = null!;
	private CSharpCompilation compilation = null!;

	[GlobalSetup]
	public void GlobalSetup()
	{
		AddressableLoadGenerator generator = new AddressableLoadGenerator();

		driver = CSharpGeneratorDriver.Create(generator);
		
		const string base_path = "../../../../../../../../../Library/ScriptAssemblies/";

		compilation =
			CSharpCompilation.Create(nameof(AddressableLoadGeneratorBenchmarks), new[]
				{
					CSharpSyntaxTree.ParseText(AddressableLoadGeneratorTests.ADDRESSABLE_LOAD_CLASS_SIMPLE),
					CSharpSyntaxTree.ParseText(AddressableLoadGeneratorTests.ADDRESSABLE_LOAD_CLASS_SIMPLE_T)
				},
				new[]
				{
					MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
					MetadataReference.CreateFromFile(base_path + "Hertzole.UnityToolbox.dll"),
					MetadataReference.CreateFromFile(base_path + "Hertzole.ScriptableValues.dll"),
					MetadataReference.CreateFromFile(base_path + "Unity.Addressables.dll"),
					MetadataReference.CreateFromFile(base_path + "Unity.ResourceManager.dll")
				});
	}

	[Benchmark]
	public void Run()
	{
		driver.RunGenerators(compilation);
	}
}