using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Hertzole.UnityToolbox.Lab
{
	public partial class GenerateLoad
	{
		[GenerateLoad]
		public AssetReferenceT<GameObject> assetReference;
		[GenerateLoad]
		public AssetReferenceT<GameObject> AssetReference { get; set; }
	}
}