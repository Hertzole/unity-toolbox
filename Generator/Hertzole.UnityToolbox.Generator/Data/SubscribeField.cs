using Microsoft.CodeAnalysis;

namespace Hertzole.UnityToolbox.Generator.Data;

public record struct SubscribeField(ITypeSymbol FieldType, string FieldName, string UniqueName, bool SubscribeToChanging);