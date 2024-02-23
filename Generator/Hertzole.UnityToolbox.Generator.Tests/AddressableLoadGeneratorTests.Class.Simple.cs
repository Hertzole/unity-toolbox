﻿using Xunit;

namespace Hertzole.UnityToolbox.Generator.Tests;

public sealed partial class AddressableLoadGeneratorTests
{
	public const string ADDRESSABLE_LOAD_CLASS_SIMPLE_T = /*lang=cs*/@"using Hertzole.UnityToolbox;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class TestClassT
{
	[GenerateLoad]
	public AssetReferenceT<Spawnpoint> assetReference;
	[GenerateLoad]
	public AssetReferenceT<Spawnpoint> AssetReference { get; set; }
}";
	
	public const string ADDRESSABLE_LOAD_CLASS_SIMPLE = /*lang=cs*/@"using Hertzole.UnityToolbox;
using UnityEngine;
using UnityEngine.AddressableAssets;
 
public partial class TestClass
{
 	[GenerateLoad]
 	public AssetReferenceGameObject assetReference;
	[GenerateLoad]
	public AssetReferenceGameObject AssetReference { get; set; }
}";

	private const string EXPECTED_ADDRESSABLE_LOAD_CLASS_SIMPLE_T = /*lang=cs*/@"// <auto-generated>
// 		This file was generated by the Unity Toolbox Generator, by Hertzole.
// 		Do not edit this file manually
// </auto-generated>

partial class TestClassT
{
	[global::JetBrains.Annotations.CanBeNull]
	private global::Hertzole.UnityToolbox.Spawnpoint asset = null;
	[global::JetBrains.Annotations.CanBeNull]
	private global::Hertzole.UnityToolbox.Spawnpoint Asset_1 = null;
	private global::UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<global::Hertzole.UnityToolbox.Spawnpoint> assetHandle;
	private global::UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<global::Hertzole.UnityToolbox.Spawnpoint> Asset_1Handle;

	private void LoadAssets()
	{
		assetHandle = global::UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<global::Hertzole.UnityToolbox.Spawnpoint>(assetReference);
		assetHandle.Completed += (op) =>
		{
			if (op.Status == global::UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
			{
				asset = op.Result;
				OnAssetLoaded(op.Result);
			}
			else
			{
				global::UnityEngine.Debug.LogError($""Failed to load reference assetReference with error: {op.OperationException}"");
			}
		};
		Asset_1Handle = global::UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<global::Hertzole.UnityToolbox.Spawnpoint>(AssetReference);
		Asset_1Handle.Completed += (op) =>
		{
			if (op.Status == global::UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
			{
				Asset_1 = op.Result;
				OnAsset_1Loaded(op.Result);
			}
			else
			{
				global::UnityEngine.Debug.LogError($""Failed to load reference AssetReference with error: {op.OperationException}"");
			}
		};
	}

	private void ReleaseAssets()
	{
		if (assetHandle.IsValid())
		{
			global::UnityEngine.AddressableAssets.Addressables.Release(assetHandle);
		}
		if (Asset_1Handle.IsValid())
		{
			global::UnityEngine.AddressableAssets.Addressables.Release(Asset_1Handle);
		}
	}

	partial void OnAssetLoaded(global::Hertzole.UnityToolbox.Spawnpoint value);

	partial void OnAsset_1Loaded(global::Hertzole.UnityToolbox.Spawnpoint value);
}
";
	
	private const string EXPECTED_ADDRESSABLE_LOAD_CLASS_SIMPLE = /*lang=cs*/@"// <auto-generated>
// 		This file was generated by the Unity Toolbox Generator, by Hertzole.
// 		Do not edit this file manually
// </auto-generated>

partial class TestClass
{
	[global::JetBrains.Annotations.CanBeNull]
	private global::UnityEngine.GameObject asset = null;
	[global::JetBrains.Annotations.CanBeNull]
	private global::UnityEngine.GameObject Asset_1 = null;
	private global::UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<global::UnityEngine.GameObject> assetHandle;
	private global::UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<global::UnityEngine.GameObject> Asset_1Handle;

	private void LoadAssets()
	{
		assetHandle = global::UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<global::UnityEngine.GameObject>(assetReference);
		assetHandle.Completed += (op) =>
		{
			if (op.Status == global::UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
			{
				asset = op.Result;
				OnAssetLoaded(op.Result);
			}
			else
			{
				global::UnityEngine.Debug.LogError($""Failed to load reference assetReference with error: {op.OperationException}"");
			}
		};
		Asset_1Handle = global::UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<global::UnityEngine.GameObject>(AssetReference);
		Asset_1Handle.Completed += (op) =>
		{
			if (op.Status == global::UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
			{
				Asset_1 = op.Result;
				OnAsset_1Loaded(op.Result);
			}
			else
			{
				global::UnityEngine.Debug.LogError($""Failed to load reference AssetReference with error: {op.OperationException}"");
			}
		};
	}

	private void ReleaseAssets()
	{
		if (assetHandle.IsValid())
		{
			global::UnityEngine.AddressableAssets.Addressables.Release(assetHandle);
		}
		if (Asset_1Handle.IsValid())
		{
			global::UnityEngine.AddressableAssets.Addressables.Release(Asset_1Handle);
		}
	}

	partial void OnAssetLoaded(global::UnityEngine.GameObject value);

	partial void OnAsset_1Loaded(global::UnityEngine.GameObject value);
}
";

	[Fact]
	public void Class_Simple_AssetReferenceT()
	{
		GeneratorTest.RunTest<AddressableLoadGenerator>("TestClassT.Addressables.g.cs", ADDRESSABLE_LOAD_CLASS_SIMPLE_T, EXPECTED_ADDRESSABLE_LOAD_CLASS_SIMPLE_T);
	}
	
	[Fact]
	public void Class_Simple_AssetReference()
	{
		GeneratorTest.RunTest<AddressableLoadGenerator>("TestClass.Addressables.g.cs", ADDRESSABLE_LOAD_CLASS_SIMPLE, EXPECTED_ADDRESSABLE_LOAD_CLASS_SIMPLE);
	}
}