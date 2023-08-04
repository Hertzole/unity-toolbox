using Microsoft.CodeAnalysis;

namespace Hertzole.UnityToolbox.Generator.Data;

public record struct SubscribeField(IFieldSymbol Field, string FieldName);