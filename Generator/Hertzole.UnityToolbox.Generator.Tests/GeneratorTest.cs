using System.Collections.Generic;
using System.IO;
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

		expected = expected.Replace("    ", "\t").ReplaceLineEndings();

		CSharpGeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

		List<PortableExecutableReference> refs = new List<PortableExecutableReference>();
		refs.Add(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
		var unityAssemblies = Directory.GetFiles("../../../../../Library/ScriptAssemblies", "*.dll");
		foreach (var assembly in unityAssemblies)
		{
			refs.Add(MetadataReference.CreateFromFile(assembly));
		}
		
		CSharpCompilation compilation =
			CSharpCompilation.Create(nameof(AddressableLoadGeneratorTests), new[] { CSharpSyntaxTree.ParseText(text) },
				refs, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, true));

		GeneratorDriverRunResult runResult = driver.RunGenerators(compilation).GetRunResult();

		SyntaxTree? generatedFileSyntax = runResult.GeneratedTrees.FirstOrDefault(t => t.FilePath.EndsWith(fileName));

		Assert.NotNull(generatedFileSyntax);

		Assert.Equal(expected, generatedFileSyntax.GetText().ToString());
	}
}