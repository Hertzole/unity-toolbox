using Microsoft.CodeAnalysis;

namespace Hertzole.UnityToolbox.Generator;

public sealed class ReferenceSymbols
{
	private readonly Compilation compilation;

	private INamedTypeSymbol? generateLoadAttribute;
	private INamedTypeSymbol? assetReferenceT;
	private INamedTypeSymbol? generateSubscribeMethodsAttribute;
	private INamedTypeSymbol? scriptableValue;
	private INamedTypeSymbol? scriptableEvent;
	private INamedTypeSymbol? scriptableGenericEvent;
	private INamedTypeSymbol? eventArgs;
	private INamedTypeSymbol? systemObject;
	
	public const string ASSET_REFERENCE_T = "UnityEngine.AddressableAssets.AssetReferenceT`1";

	public INamedTypeSymbol? GenerateLoadAttribute
	{
		get { return generateLoadAttribute ??= compilation.GetTypeByMetadataName("Hertzole.UnityToolbox.GenerateLoadAttribute"); }
	}

	public INamedTypeSymbol? AssetReferenceT
	{
		get { return assetReferenceT ??= compilation.GetTypeByMetadataName("UnityEngine.AddressableAssets.AssetReferenceT`1"); }
	}

	public INamedTypeSymbol? GenerateSubscribeMethodsAttribute
	{
		get { return generateSubscribeMethodsAttribute ??= compilation.GetTypeByMetadataName("Hertzole.UnityToolbox.GenerateSubscribeMethodsAttribute"); }
	}

	public INamedTypeSymbol? ScriptableValue
	{
		get { return scriptableValue ??= compilation.GetTypeByMetadataName("Hertzole.ScriptableValues.ScriptableValue`1"); }
	}

	public INamedTypeSymbol? ScriptableEvent
	{
		get { return scriptableEvent ??= compilation.GetTypeByMetadataName("Hertzole.ScriptableValues.ScriptableEvent"); }
	}

	public INamedTypeSymbol? ScriptableGenericEvent
	{
		get { return scriptableGenericEvent ??= compilation.GetTypeByMetadataName("Hertzole.ScriptableValues.ScriptableEvent`1"); }
	}

	public INamedTypeSymbol? EventArgs
	{
		get { return eventArgs ??= compilation.GetTypeByMetadataName("System.EventArgs"); }
	}

	public INamedTypeSymbol? SystemObject
	{
		get { return systemObject ??= compilation.GetTypeByMetadataName("System.Object"); }
	}

	public ReferenceSymbols(Compilation compilation)
	{
		this.compilation = compilation;
	}
}