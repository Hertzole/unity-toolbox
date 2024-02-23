using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Hertzole.UnityToolbox.Generator.Tests;

public static class GeneratorTest
{
	public static void RunTest<T>(string fileName, string text, string expected) where T : class, IIncrementalGenerator, new()
	{
		T generator = new T();

		CSharpGeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

		CSharpCompilation compilation =
			CSharpCompilation.Create(nameof(AddressableLoadGeneratorTests), new[] { CSharpSyntaxTree.ParseText(text) },
				new[]
				{
					MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
					MetadataReference.CreateFromFile("../../../../../Library/ScriptAssemblies/Hertzole.UnityToolbox.dll"),
					MetadataReference.CreateFromFile("../../../../../Library/ScriptAssemblies/Hertzole.ScriptableValues.dll"),
					MetadataReference.CreateFromFile("../../../../../Library/ScriptAssemblies/Unity.Addressables.dll"),
					MetadataReference.CreateFromFile("../../../../../Library/ScriptAssemblies/Unity.ResourceManager.dll")
				});

		GeneratorDriverRunResult runResult = driver.RunGenerators(compilation).GetRunResult();

		SyntaxTree? generatedFileSyntax = runResult.GeneratedTrees.FirstOrDefault(t => t.FilePath.EndsWith(fileName));

		Assert.NotNull(generatedFileSyntax);

		Assert.Equal(expected, generatedFileSyntax.GetText().ToString());
	}
}