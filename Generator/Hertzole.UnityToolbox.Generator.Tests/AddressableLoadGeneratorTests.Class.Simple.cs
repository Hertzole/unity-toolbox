﻿using Xunit;

namespace Hertzole.UnityToolbox.Generator.Tests;

partial class AddressableLoadGeneratorTests
{
	public const string ADDRESSABLE_LOAD_CLASS_SIMPLE_T = @"using Hertzole.UnityToolbox;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class TestClassT
{
	[GenerateLoad]
	public AssetReferenceT<Spawnpoint> assetReference;
}";
	
	public const string ADDRESSABLE_LOAD_CLASS_SIMPLE = @"using Hertzole.UnityToolbox;
using UnityEngine;
using UnityEngine.AddressableAssets;
 
public partial class TestClass
{
 	[GenerateLoad]
 	public AssetReferenceGameObject assetReference;
}";

	private const string EXPECTED_ADDRESSABLE_LOAD_CLASS_SIMPLE_T = @"// <auto-generated>
// 		This file was generated by the Unity Toolbox Generator, by Hertzole.
// 		Do not edit this file manually
// </auto-generated>

partial class TestClassT
{
	[global::JetBrains.Annotations.CanBeNull]
	private global::Hertzole.UnityToolbox.Spawnpoint asset = null;
	private global::UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<global::Hertzole.UnityToolbox.Spawnpoint> assetReferenceHandle;

	private void LoadAssets()
	{
		assetReferenceHandle = global::UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<global::Hertzole.UnityToolbox.Spawnpoint>(assetReference);
		assetReferenceHandle.Completed += (op) =>
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
	}

	private void ReleaseAssets()
	{
		if (assetReferenceHandle.IsValid())
		{
			global::UnityEngine.AddressableAssets.Addressables.Release(assetReferenceHandle);
		}
	}

	partial void OnAssetLoaded(global::Hertzole.UnityToolbox.Spawnpoint value);
}
";
	
	private const string EXPECTED_ADDRESSABLE_LOAD_CLASS_SIMPLE = @"// <auto-generated>
// 		This file was generated by the Unity Toolbox Generator, by Hertzole.
// 		Do not edit this file manually
// </auto-generated>

partial class TestClass
{
	[global::JetBrains.Annotations.CanBeNull]
	private global::UnityEngine.GameObject asset = null;
	private global::UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<global::UnityEngine.GameObject> assetReferenceHandle;

	private void LoadAssets()
	{
		assetReferenceHandle = global::UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<global::UnityEngine.GameObject>(assetReference);
		assetReferenceHandle.Completed += (op) =>
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
	}

	private void ReleaseAssets()
	{
		if (assetReferenceHandle.IsValid())
		{
			global::UnityEngine.AddressableAssets.Addressables.Release(assetReferenceHandle);
		}
	}

	partial void OnAssetLoaded(global::UnityEngine.GameObject value);
}
";

	[Fact]
	public void Class_Simple_AssetReferenceT()
	{
		RunTest("TestClassT.Addressables.g.cs", ADDRESSABLE_LOAD_CLASS_SIMPLE_T, EXPECTED_ADDRESSABLE_LOAD_CLASS_SIMPLE_T);
	}
	
	[Fact]
	public void Class_Simple_AssetReference()
	{
		RunTest("TestClass.Addressables.g.cs", ADDRESSABLE_LOAD_CLASS_SIMPLE, EXPECTED_ADDRESSABLE_LOAD_CLASS_SIMPLE);
	}
}