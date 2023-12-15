using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Hertzole.UnityToolbox.Generator.Helpers;

public class TypeNameDeclarationComparer : IEqualityComparer<TypeDeclarationSyntax>
{
	public static readonly TypeNameDeclarationComparer Instance = new TypeNameDeclarationComparer();

	public bool Equals(TypeDeclarationSyntax x, TypeDeclarationSyntax y)
	{
		if (ReferenceEquals(x, y))
		{
			return true;
		}

		if (ReferenceEquals(x, null))
		{
			return false;
		}

		if (ReferenceEquals(y, null))
		{
			return false;
		}

		if (x.GetType() != y.GetType())
		{
			return false;
		}

		return x.Identifier.Text == y.Identifier.Text;
	}

	public int GetHashCode(TypeDeclarationSyntax obj)
	{
		return obj.Identifier.Text.GetHashCode();
	}
}