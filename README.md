# Unity Toolbox

[![openupm](https://img.shields.io/npm/v/se.hertzole.unitytoolbox?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/se.hertzole.unitytoolbox/)

A library of useful Unity tools, extensions, and source generators.

## Data Types

### RandomFloat/Int

A random float or int value that can be serialized and used in the inspector. Each time you access the value, it will be a new random value.

```cs
RandomFloat randomFloat = new RandomFloat(0, 10);
float value = randomFloat.Value; // Something between 0 and 10
```

### Layer

A layer that can be serialized and used in the inspector. It can be implicitly converted to/from an int.

```cs
Layer layer = 1;
gameObject.layer = layer;
```

### Tag

A tag that can be serialized and used in the inspector. It can be implicitly converted to/from a string.

```cs
Tag tag = "Player";
gameObject.tag = tag;
```

### Animator Parameters

Several structs that can be serialized and used in the inspector to set animator parameters. In the inspector, it will give you a dropdown with all the available parameters on the current object.  
They can be implicitly converted to `int` for use with the animator.

```cs
AnimatorBoolParameter boolParameter = new AnimatorBoolParameter("IsWalking");
animator.SetBool(boolParameter, true);
AnimatorFloatParameter floatParameter = new AnimatorFloatParameter("Speed");
animator.SetFloat(floatParameter, 1.0f);
AnimatorIntParameter intParameter = new AnimatorIntParameter("Health");
animator.SetInteger(intParameter, 100);
AnimatorTriggerParameter triggerParameter = new AnimatorTriggerParameter("Jump");
animator.SetTrigger(triggerParameter);
```

### Identifier

A unique identifier that can be serialized and used in the inspector. It will hash a string to an int for faster checks in lists and dictionaries. The string will be removed in builds to save memory.

```cs
Identifier id = "Player";
Identifier otherId = other.Id;
if (id == otherId) { ... }
```

### Scene Reference

Scene Reference is a `struct` that can be serialized and used in the inspector to reference a scene. It can be implicitly converted to an `int` for the build index. Unlike most other scene references, this one will not break when you move scenes around. It will also not break when you rename scenes. It also provides a quick fix button for when you try to add a scene that is not in the build settings.

```cs
SceneReference scene = new SceneReference(0);
SceneManager.LoadScene(scene);

Debug.Log(scene.SceneName);
Debug.Log(scene.ScenePath);
```

## Extensions

### Addressable Extensions

Only available if the addressables package is installed.

#### LoadAsync

Extends `AssetReference<T>`.

A shorter version of `Addressables.LoadAssetAsync<T>` that can be used with `AssetReference<T>` with a callback. Has an optional bool to only call the callback if the loading was successful.

```cs
AssetReferenceGameObject reference = ...;
AsyncOperationHandle<GameObject> handle = reference.LoadAsync(handle => 
{
    GameObject go = handle.Result;
}, onlyCompleteOnSuccess: true);
```

#### Release

Extends `AsyncOperationHandle<T>?`, `AsyncOperationHandle<T>`, and `AsyncOperationHandle`.

A shorter version of `Addressables.Release` that only calls if the handle is valid.

```cs
AsyncOperationHandle handle = ...;
handle.Release();
```

### Async Extensions

Only available if [UniTask](https://github.com/Cysharp/UniTask) is installed.

#### WaitUntilNotNull

Extends `UnityEngine.Object`.

Waits until the object is not null. This is useful for waiting for an object to be loaded from an addressable.

```cs
GameObject go = ...;
await go.WaitUntilNotNull();
```

There are many more extensions and you can see them all [here](https://github.com/Hertzole/unity-toolbox/tree/master/Packages/se.hertzole.unitytoolbox/Runtime/Extensions).

## Components

Until fully documented, see the components [here](https://github.com/Hertzole/unity-toolbox/tree/master/Packages/se.hertzole.unitytoolbox/Runtime/Components).

## Helpers

Until fully documented, see the helpers [here](https://github.com/Hertzole/unity-toolbox/tree/master/Packages/se.hertzole.unitytoolbox/Runtime/Helpers).

## Pooling

### Circular Object Pool

A pool that will reuse objects. It will never grow in size. It will always return the next object in the pool. When the pool is empty, it will pool the first active object and return that.

```cs
public class MyObject
{
    public int ID { get; set;}

    public MyObject(int id)
    {
        ID = id;
    }
}

private static int nextId = 0;

private readonly CircularObjectPool<MyObject> pool = new CircularObjectPool<MyObject>(10, () => 
{
    int id = nextId;
    nextId++;
    return new MyObject(id)
});

for (int i = 0; i < 10; i++)
{
    MyObject obj = pool.Get();
}

// The pool is now empty.
MyObject obj = pool.Get(); // Will return the first active object.
Console.WriteLine(obj.ID); // 0
```

## Source Generators

These source generators are only available in Unity 2022.2 or newer. They will generate some code for you to make your life easier and they all work together like you would expect. If they don't, please open an issue.

### Addressable Load Generator

**Only available if the addressables package is installed.**

Generates a `LoadAssets` and a `ReleaseAssets` method for all marked `AssetReference<T>` fields. It will start loading all the assets when you call the `LoadAssets` method and release them when you call the `ReleaseAssets` method.

```cs
using Hertzole.UnityToolbox;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Must be 'partial'.
public partial class MyScript : MonoBehaviour
{
    [SerializeField]
    [GenerateLoad] // This will generate the LoadAssets and ReleaseAssets methods.
    private AssetReferenceGameObject prefabReference = default;

    private void Awake()
    {
        LoadAssets();
    }

    private void OnDestroy()
    {
        ReleaseAssets();
    }

    // You can also implement the partial method for when the assets are loaded.
    private partial void OnPrefabLoaded(GameObject value)
    {
        // Do something with the prefab.
    }
}
```

### Input Callbacks Generator

**Only available if the input system package is installed.**

Generates a `OnInputAction` method for all marked `InputActionReference` fields. It will automatically subscribe to the desired event and call the method when the action is performed. It will also generate a (un)subscribe method for all marked `InputActionReference` fields, along with a method to (un)subscribe to all of them at once.

```cs
using Hertzole.UnityToolbox;
using UnityEngine;
using UnityEngine.InputSystem;

// Must be 'partial'.
public partial class MyScript : MonoBehaviour
{
    [SerializeField]
    [GenerateInputCallbacks(nameof(playerInput), // You must provide the name of the PlayerInput variable to subscribe to.
    GenerateStarted = true, // Generate a callback for when the action is started.
    GeneratePerformed = true, // Generate a callback for when the action is performed.
    GenerateCanceled = true, // Generate a callback for when the action is canceled.
    GenerateAll = true)] // Generates a callback for all of the above.
    private InputActionReference action = default;

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        // Subscribe to all callbacks.
        SubscribeToAllInputCallbacks();

        // Subscribe to a specific callback.
        // SubscribeToAction();
    }

    private void OnDisable()
    {
        // Unsubscribe from all callbacks.
        UnsubscribeFromAllInputCallbacks();

        // Unsubscribe from a specific callback.
        // UnsubscribeFromAction();
    }

    // Generated from GenerateStarted.
    private partial void OnInputActionStarted(InputAction.CallbackContext context) { }

    // Generated from GeneratePerformed.
    private partial void OnInputActionPerformed(InputAction.CallbackContext context) { }

    // Generated from GenerateCanceled.
    private partial void OnInputActionCanceled(InputAction.CallbackContext context) { }

    // Generated from GenerateAll.
    private partial void OnInputAction(InputAction.CallbackContext context) { }
}
```

### Subscribe Methods Generator

**Only available if [Scriptable Values](https://github.com/Hertzole/scriptable-values) is installed.**

Automatically generates methods to subscribe to scriptable value's respective events. It works with `ScriptableEvent`, `ScriptableEvent<T>`, and `ScriptableValue<T>`.

```cs
using Hertzole.UnityToolbox;
using Hertzole.ScriptableValues;
using UnityEngine;
using System;

// Must be 'partial'.
public partial class MyScript : MonoBehaviour
{
    [SerializeField]
    [GenerateSubscribeMethods] // Will subscribe to 'OnValueChanged'.
    private ScriptableInt action = default;
    [SerializeField]
    [GenerateSubscribeMethods] // Will subscribe to 'OnInvoked'.
    private ScriptableEvent actionEvent = default;
    [SerializeField]
    [GenerateSubscribeMethods] // Will subscribe to 'OnInvoked'.
    private ScriptableIntEvent actionEventInt = default;

    private void OnEnable()
    {
        // Subscribe to all callbacks.
        SubscribeToAll();

        // Subscribe to a specific callback.
        // SubscribeToAction();
    }

    private void OnDisable()
    {
        // Unsubscribe from all callbacks.
        UnsubscribeFromAll();

        // Unsubscribe from a specific callback.
        // UnsubscribeFromAction();
    }

    private partial void OnActionChanged(int previousValue, int newValue) { }

    private partial void OnActionEventInvoked(object sender, EventArgs e) { }

    private partial void OnActionEventIntInvoked(object sender, int e) { }
}
```