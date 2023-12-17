namespace Hertzole.UnityToolbox.Generator.Benchmarks;

public partial class AddressableLoadGeneratorBenchmarks
{
	public const string TEST_CLASS = @"
using System;
using Hertzole.ScriptableValues;
using Hertzole.UnityToolbox;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

namespace My.Long.Namespace.That.Is.Not.A.Problem
{
	public partial class AddressableLoadGeneratorBenchmark : MonoBehaviour
	{
		[SerializeField]
		[GenerateLoad]
		private AssetReferenceGameObject go = default;
		[SerializeField]
		[GenerateLoad]
		private AssetReferenceScriptableBool scriptableBoolReference = default;
		[SerializeField]
		[GenerateLoad]
		private AssetReferenceT<ScriptableInt> intRef = default;
		[SerializeField]
		[GenerateLoad]
		[GenerateSubscribeMethods]
		private AssetReferenceScriptableEvent scriptableEventReference = default;
		[SerializeField]
		[GenerateLoad]
		[GenerateInputCallbacks(nameof(playerInput), GenerateAll = true)]
		private AssetReferenceT<InputActionReference> inputReference = default;

		private PlayerInput playerInput;

		private partial void OnInputInput(InputAction.CallbackContext context)
		{
			Debug.Log(""Input invoked!"");
		}

		private partial void OnScriptableEventInvoked(object sender, EventArgs e)
		{
			Debug.Log(""Event invoked!"");
		}

		partial void OnScriptableBoolLoaded(ScriptableBool value)
		{
			Debug.Log($""Loaded {value}"");
		}
	}
}";
}