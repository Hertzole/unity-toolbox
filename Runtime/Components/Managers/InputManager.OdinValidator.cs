#if ODIN_VALIDATOR && UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

namespace Hertzole.UnityToolbox
{
	partial class InputManager : ISelfValidator
	{
		public void Validate(SelfValidationResult result)
		{
			bool addressables = false;
#if TOOLBOX_ADDRESSABLES
			addressables = useAddressables;
#endif

			if (setInputActions && !addressables && inputActions == null)
			{
				result.AddError("Input Actions is required").WithFix((InputActionArgs args) => { inputActions = args.newValue; });
			}
#if TOOLBOX_ADDRESSABLES
			else if (addressables && setInputActions && !inputActionsReference.RuntimeKeyIsValid())
			{
				result.AddError("Input Actions Reference is required").WithFix((InputActionsReferenceArgs args) => { inputActionsReference = args.newValue; });
			}
#endif

			if (!addressables && inputsList == null)
			{
				result.AddError("Inputs List is required").WithFix((InputsListArgs args) => { inputsList = args.newValue; });
			}
#if TOOLBOX_ADDRESSABLES
			else if (addressables && !inputsListReference.RuntimeKeyIsValid())
			{
				result.AddError("Inputs List Reference is required").WithFix((InputsListReferenceArgs args) => { inputsListReference = args.newValue; });
			}
#endif
		}

		private class InputActionArgs
		{
			public InputActionAsset newValue;
		}

		private class InputsListArgs
		{
			public ScriptablePlayerInputsList newValue;
		}

		private class InputActionsReferenceArgs
		{
			public AssetReferenceT<InputActionAsset> newValue;
		}

		private class InputsListReferenceArgs
		{
			public AssetReferenceT<ScriptablePlayerInputsList> newValue;
		}
	}
}
#endif