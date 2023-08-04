using Microsoft.CodeAnalysis;

namespace Hertzole.UnityToolbox.Generator.Data;

public record struct AddressableLoadField(IFieldSymbol Field, INamedTypeSymbol AddressableType, string ValueName, bool GenerateSubscribeMethods);