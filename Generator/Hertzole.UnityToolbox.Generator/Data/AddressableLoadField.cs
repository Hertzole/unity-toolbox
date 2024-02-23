using Microsoft.CodeAnalysis;

namespace Hertzole.UnityToolbox.Generator.Data;

public record struct AddressableLoadField(INamedTypeSymbol FieldType, INamedTypeSymbol AddressableType, AddressableNameFields Names, bool GenerateSubscribeMethods, bool FieldExists, bool GenerateInputCallbacks);

public record struct AddressableNameFields(string FieldName, string AddressableName, string UniqueName);