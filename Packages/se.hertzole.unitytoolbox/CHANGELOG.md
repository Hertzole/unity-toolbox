# [1.15.0](https://github.com/Hertzole/unity-toolbox/compare/v1.14.0...v1.15.0) (2025-12-04)


### Bug Fixes

* compile issues without addressables ([ba14367](https://github.com/Hertzole/unity-toolbox/commit/ba14367b77557e7e6c737b57fac46e4975a16e33))
* debug log ([40cb3ec](https://github.com/Hertzole/unity-toolbox/commit/40cb3ece93cd0dc3bad4f1ada02431b486010a58))
* InputManager trying to release invalid addressable handles ([cbc8054](https://github.com/Hertzole/unity-toolbox/commit/cbc8054615b8006d1d05a2ad06c0ed18ffb9fa3c))
* scriptable value match nullability warnings ([a09fdca](https://github.com/Hertzole/unity-toolbox/commit/a09fdca64703fa019267949d07d9bd9c07d00639))


### Features

* (Un)RegisterValueChangeCallback extensions for PropertyField ([8c5bb69](https://github.com/Hertzole/unity-toolbox/commit/8c5bb6937ed823a0cad0e3fac7580507e38ddb96))
* EqualsApproximately to common vector types ([851ceb0](https://github.com/Hertzole/unity-toolbox/commit/851ceb0636fb2b02e09d2ee4eedc7e5699846d19))
* GameObject.HasLayer extension methods ([5234749](https://github.com/Hertzole/unity-toolbox/commit/52347491bb0a4f72a9e54df72c20150231b4b6fc))
* GizmosExtra.DrawArrow(ToPoint) ([042d4d2](https://github.com/Hertzole/unity-toolbox/commit/042d4d200f7be001718a9af0284d2ac73757848e))
* GizmosScope, replacing GizmoColorScope ([b97cc6c](https://github.com/Hertzole/unity-toolbox/commit/b97cc6c97e92b0dbc62cb0529c8cc8a50056b1d7))
* invert function to ScriptableValueMatch ([28ae782](https://github.com/Hertzole/unity-toolbox/commit/28ae7825bffd7f8079ce93c90336e1147ee53558))
* MatchGroup type ([31fe5ec](https://github.com/Hertzole/unity-toolbox/commit/31fe5ecc7dbcfbf81dbde044e99684c46202eee8))
* MinMax types ([8ae1db7](https://github.com/Hertzole/unity-toolbox/commit/8ae1db77c2a845597863d594bd782eb2773a4dd2))
* MinMaxInt that replaces RandomInt ([f552daa](https://github.com/Hertzole/unity-toolbox/commit/f552daab9e0089c235feb4a1c915b82e0c446c10))
* missing UnregisterValueChangedCallback extension method ([a2135a5](https://github.com/Hertzole/unity-toolbox/commit/a2135a58360bd5079e56c4a6ca006b0f596e57e8))
* new chat manager ([392579e](https://github.com/Hertzole/unity-toolbox/commit/392579e23d9174782164d6d005c213d64ad37b90))
* new WeightedRandom methods using Func to get weight ([9164667](https://github.com/Hertzole/unity-toolbox/commit/9164667a89e3484385219085dc30f164c02f983a))
* UI toolkit extensions: TryGetLabelElement, IsFocused, and ClampValue ([64925dc](https://github.com/Hertzole/unity-toolbox/commit/64925dc2be71a65d011e8593a3c3e2cd4b1c4624))
* WeightedRandom.GetRandomIndex ([58e4516](https://github.com/Hertzole/unity-toolbox/commit/58e451619b507849b5b396f55ef458d4731e297c))
* WithX extensions to common Unity types ([f326bec](https://github.com/Hertzole/unity-toolbox/commit/f326becdab71fc14b98bf87e73d714894bf19d8c))


### Performance Improvements

* less allocations in ColorExtensions.ToHex ([5547b04](https://github.com/Hertzole/unity-toolbox/commit/5547b04631c0cf57bd5b716c4306c0830dad3072))

# [1.14.0](https://github.com/Hertzole/unity-toolbox/compare/v1.13.1...v1.14.0) (2025-07-13)


### Features

* access to Identifier.StringValue, but only in the editor! ([c903041](https://github.com/Hertzole/unity-toolbox/commit/c903041dbe0761343b5e3c1f08e6f176ba4d4589))
* RegisterValueChangeCallback with user args extension method for visual elements ([a310fce](https://github.com/Hertzole/unity-toolbox/commit/a310fce869f08e10f80b7c8d7fb8248cc49421c9))

## [1.13.1](https://github.com/Hertzole/unity-toolbox/compare/v1.13.0...v1.13.1) (2025-06-11)


### Bug Fixes

* correct action subscription operator in InputCallbacksGenerator ([e516c4a](https://github.com/Hertzole/unity-toolbox/commit/e516c4a648739920b4dac4ab550d851a7085a713))

# [1.13.0](https://github.com/Hertzole/unity-toolbox/compare/v1.12.7...v1.13.0) (2025-06-11)


### Features

* Identifier.ToString() ([1985650](https://github.com/Hertzole/unity-toolbox/commit/19856500df38739f579c26c1d7ee32fcf6c3766e))
* you no longer need to provide a PlayerInput for GenerateInputCallbacksAttribute ([3f693fa](https://github.com/Hertzole/unity-toolbox/commit/3f693fa860d359ac463403ff209362883bebad5b))

## [1.12.7](https://github.com/Hertzole/unity-toolbox/compare/v1.12.6...v1.12.7) (2025-05-28)


### Bug Fixes

* GenerateSubscribeMethodsAttribute being included in builds even if obsolete ([67ef64b](https://github.com/Hertzole/unity-toolbox/commit/67ef64b1b2f60100fef67d598defbc8a2690ebec))
* support for scriptable values 2.0 ([6458b4e](https://github.com/Hertzole/unity-toolbox/commit/6458b4ed69f4ee059f3e7053f703a61d3be476ee))


### Performance Improvements

* don't include tooltips in builds ([69a4367](https://github.com/Hertzole/unity-toolbox/commit/69a4367caec965a672d9297f2c1dd385a4ca0189))

## [1.12.6](https://github.com/Hertzole/unity-toolbox/compare/v1.12.5...v1.12.6) (2024-11-13)


### Bug Fixes

* compiler errors when ScriptableValues Package is missing ([4cf50b5](https://github.com/Hertzole/unity-toolbox/commit/4cf50b5d5111df82f7aea7dd34fb59ab4ccff158)), closes [#2](https://github.com/Hertzole/unity-toolbox/issues/2)
* generated subscribe code referencing the wrong field ([91a5b8d](https://github.com/Hertzole/unity-toolbox/commit/91a5b8d1f307bb18408450643477209ca7a6d4fe))
* throw an error if value callbacks haven't been initialized ([6b1337e](https://github.com/Hertzole/unity-toolbox/commit/6b1337e5245df6955c240e0d2da0f2ecac6c0f8c))

## [1.12.5](https://github.com/Hertzole/unity-toolbox/compare/v1.12.4...v1.12.5) (2024-11-12)


### Bug Fixes

* subscribing to changing method throws error even if changing method is implemented ([fd15c06](https://github.com/Hertzole/unity-toolbox/commit/fd15c0672b17305d7a599f32f110ab8757e0ec9b))

## [1.12.4](https://github.com/Hertzole/unity-toolbox/compare/v1.12.3...v1.12.4) (2024-11-12)


### Bug Fixes

* unity package not being updated along the release ([c324c49](https://github.com/Hertzole/unity-toolbox/commit/c324c4974b6cc65c3889e1c7c9f131ca3bb475bd))

## [1.12.3](https://github.com/Hertzole/unity-toolbox/compare/v1.12.2...v1.12.3) (2024-11-12)


### Bug Fixes

* release not working ([b7e0392](https://github.com/Hertzole/unity-toolbox/commit/b7e0392bcfe184adbbd64751e704bbbf30f5c86f))

## [1.12.2](https://github.com/Hertzole/unity-toolbox/compare/v1.12.1...v1.12.2) (2024-11-12)


### Bug Fixes

* everything that shouldn't be included being included ([17e12e2](https://github.com/Hertzole/unity-toolbox/commit/17e12e2f7b8f889e55043948047137dce5600ad7))


### Reverts

* Revert "chore(release): 1.12.1 [skip ci]" ([e489d68](https://github.com/Hertzole/unity-toolbox/commit/e489d6813c16e29b55f87530a889790ce3d81004))

# [1.12.0](https://github.com/Hertzole/unity-toolbox/compare/v1.11.0...v1.12.0) (2024-11-12)


### Bug Fixes

* AddressableLoadGenerator duplicate names and property support ([f92b40a](https://github.com/Hertzole/unity-toolbox/commit/f92b40a0966157ec47fb131d6b52241c7b15a446))
* AddressableLoadGenerator not naming files properly ([fb74656](https://github.com/Hertzole/unity-toolbox/commit/fb7465648f1e857b9c151440b0e91864df7291a6))
* CircularObjectPool not disposing its internal pool ([e27e92f](https://github.com/Hertzole/unity-toolbox/commit/e27e92fdc472af04e4fae4f97d13c0643276a704))
* compilation error without physics module ([43c9091](https://github.com/Hertzole/unity-toolbox/commit/43c90913ac71430123849c3e0bdee684ad17937c)), closes [#1](https://github.com/Hertzole/unity-toolbox/issues/1)
* GenerateSubscribeMethods generator not generating properly ([18d574c](https://github.com/Hertzole/unity-toolbox/commit/18d574c6f451417dbe24cc10e6bc7b492fa5155c))
* GenerateSubscribeMethods not being available on properties ([2ebc4fd](https://github.com/Hertzole/unity-toolbox/commit/2ebc4fd529602ea7f399ae8ba7ef1db26acad370))
* generators and analyzers breaking after editing anything ([6c94af4](https://github.com/Hertzole/unity-toolbox/commit/6c94af4d6267d4a79ab45534fd4a2397966b63e0))
* InputCallbackGenerator not generating (un)subscribe all methods ([c494d85](https://github.com/Hertzole/unity-toolbox/commit/c494d85f927b982ec5d36dfdb3508333d9462bb1))
* InputCallbacksGenerator duplicate names ([918bfc3](https://github.com/Hertzole/unity-toolbox/commit/918bfc3ad7a26ae8489c5ed6802eac07e38ba03d))
* InputCallbacksGenerator not working properly ([fe08084](https://github.com/Hertzole/unity-toolbox/commit/fe080847804583a2d73347781e421dc6bba3859c))
* InputManager compiler error without addressables ([05eb7ce](https://github.com/Hertzole/unity-toolbox/commit/05eb7ce8bd0ed10d4722ddc275f3c3179c836255))
* not being able to have multiple partial types when using source generator ([893f6e9](https://github.com/Hertzole/unity-toolbox/commit/893f6e953c696329ec0f44f5fa3d3e1a5421003e))
* SubscribeField field equals ([f03d6c1](https://github.com/Hertzole/unity-toolbox/commit/f03d6c1f292b6efde1f222c2f5b7e967564024e5))
* SubscribeMethodsAnalyzer giving wrong error ([290c95f](https://github.com/Hertzole/unity-toolbox/commit/290c95f1adb5056d8716bb8e78fcff416aeb28b3))
* SubscribeMethodsGenerator exiting too early ([1ad91f2](https://github.com/Hertzole/unity-toolbox/commit/1ad91f2e9c87ff5c66faace4ce591420ab5cd4e3))
* SubscribeMethodsGenerator now avoids duplicate field names ([9f9eeac](https://github.com/Hertzole/unity-toolbox/commit/9f9eeace7a9d1d011ed630dc64e78a428fe7a3d2))
* wrong indent when no namespace in AddressableLoadGenerator ([3295b26](https://github.com/Hertzole/unity-toolbox/commit/3295b26a4cd69e067d8876517685ab7ed5c63d6e))


### Features

* InputCallbacksGenerator now supports properties ([b0ca105](https://github.com/Hertzole/unity-toolbox/commit/b0ca1058c7590dc65b8490d2d1b7c2fcf5eed923))
* odin validator support for input manager ([53d4c23](https://github.com/Hertzole/unity-toolbox/commit/53d4c236d5011aef643be421d43a6307a8dcb8fa))
* support for InputManager to set input actions ([36a9687](https://github.com/Hertzole/unity-toolbox/commit/36a968778700825073774c637cd47585eb2c2f44))
* support GenerateLoadAttribute in structs ([3123416](https://github.com/Hertzole/unity-toolbox/commit/312341696d787c7f1d44a230fb9e4cbb555309da))
* VisualElement.SetUniformBorder extension ([2f62e3b](https://github.com/Hertzole/unity-toolbox/commit/2f62e3b020c6bbe1906ed4f836db96d07b95bfe1))


### Performance Improvements

* AddressableLoadGenerator now uses new pooled scopes ([d3b8b7b](https://github.com/Hertzole/unity-toolbox/commit/d3b8b7b44bd3b8ddb1fe0a8b42c8e7acf2da4b6c))
* SubscribeMethodsGenerator now caches scriptable value callbacks ([01d1f85](https://github.com/Hertzole/unity-toolbox/commit/01d1f85926d8cb71742880b547a0e40994eff527))
* SubscribeMethodsGenerator now uses pooled scopes ([955cb65](https://github.com/Hertzole/unity-toolbox/commit/955cb65938996b857b54fef33f61dfd7ab9083d0))

# [1.11.0](https://github.com/Hertzole/unity-toolbox/compare/v1.10.1...v1.11.0) (2023-12-07)


### Features

* IsSingletonInstance bool to MonoSingleton.cs ([f4c1bec](https://github.com/Hertzole/unity-toolbox/commit/f4c1bece86931c8ad84501ccb04628dcb65e29ff))

## [1.10.1](https://github.com/Hertzole/unity-toolbox/compare/v1.10.0...v1.10.1) (2023-12-03)


### Bug Fixes

* cursor manager unlocking cursor when destroying non-main instance ([38ad014](https://github.com/Hertzole/unity-toolbox/commit/38ad014cfff44e4cb0ab44c0d71127cc02dee601))

# [1.10.0](https://github.com/Hertzole/unity-toolbox/compare/v1.9.1...v1.10.0) (2023-11-28)


### Features

* scene reference ([1ba824e](https://github.com/Hertzole/unity-toolbox/commit/1ba824e35e0dfe092870c9d7ec500095460f17df))

## [1.9.1](https://github.com/Hertzole/unity-toolbox/compare/v1.9.0...v1.9.1) (2023-11-24)


### Bug Fixes

* circular pool ReleaseAll not releasing properly ([feab174](https://github.com/Hertzole/unity-toolbox/commit/feab174bfffa12c31f7567a7f94e7bd68d70e3c9))

# [1.9.0](https://github.com/Hertzole/unity-toolbox/compare/v1.8.0...v1.9.0) (2023-11-23)


### Features

* circular object pool class ([b55ee64](https://github.com/Hertzole/unity-toolbox/commit/b55ee64f0e99d837e634bbde58fe38970a5e1077))
* custom property drawers for RandomInt and RandomFloat ([87c3185](https://github.com/Hertzole/unity-toolbox/commit/87c318598845f74b23de226a5813c1b84f420ff9))
* open paths utility ([513fb81](https://github.com/Hertzole/unity-toolbox/commit/513fb813c8f1df0ca8c58e87ecd7a318a5bbcb1a))

# [1.8.0](https://github.com/Hertzole/unity-toolbox/compare/v1.7.1...v1.8.0) (2023-11-12)


### Bug Fixes

* no changelog url in package manifest ([5171502](https://github.com/Hertzole/unity-toolbox/commit/5171502a98e7a3275717138b4f3a06c6f1614e53))
* no docs url in package manifest ([d7c1cdd](https://github.com/Hertzole/unity-toolbox/commit/d7c1cdd6ec68e8aabbb581834f4ed505d6819e8c))
* no license url in package manifest ([9c73433](https://github.com/Hertzole/unity-toolbox/commit/9c73433794e9aef1e29868f228b0d446e12e0e3d))


### Features

* show hidden game objects tool ([db42b79](https://github.com/Hertzole/unity-toolbox/commit/db42b79691756837059afad6f89ac56a3551a6f2))

## [1.7.1](https://github.com/Hertzole/unity-toolbox/compare/v1.7.0...v1.7.1) (2023-11-01)


### Bug Fixes

* compile error with Unity.Collections > 2.0.0 ([4c3bcd9](https://github.com/Hertzole/unity-toolbox/commit/4c3bcd95cfced8bceb89a255df9e582112ce3148))
* compiling issue without Unity.Collections ([cd2d575](https://github.com/Hertzole/unity-toolbox/commit/cd2d57525a3f040b3b4427ad75fb26dcaa6c4865))


### Performance Improvements

* use in and ref for native extensions ([af08ba4](https://github.com/Hertzole/unity-toolbox/commit/af08ba47f57eac3c37f6ca0e9e6c2f6b7f1cb27f))

# [1.7.0](https://github.com/Hertzole/unity-toolbox/compare/v1.6.3...v1.7.0) (2023-11-01)


### Features

* add collection extensions to NativeArray ([04283c7](https://github.com/Hertzole/unity-toolbox/commit/04283c736ae83cc4e24ab88cba0bdd9ff3b1b366))
* add collection extensions to NativeList ([40ecf09](https://github.com/Hertzole/unity-toolbox/commit/40ecf09bce47d7274a31bb4505380f0d9cee0811))
* get elements by ref in native collections ([4ae44c9](https://github.com/Hertzole/unity-toolbox/commit/4ae44c9a613f5d1788f72853e93c565814b9fcb8))

## [1.6.3](https://github.com/Hertzole/unity-toolbox/compare/v1.6.2...v1.6.3) (2023-10-27)


### Bug Fixes

* animator paramater types having empty types ([83b5991](https://github.com/Hertzole/unity-toolbox/commit/83b599110f9ce026e3b41a7b30d5c4c6ae5b9e90))

## [1.6.2](https://github.com/Hertzole/unity-toolbox/compare/v1.6.1...v1.6.2) (2023-10-07)


### Bug Fixes

* compile error when not using Addressables ([5bd9ad2](https://github.com/Hertzole/unity-toolbox/commit/5bd9ad295d34272068f9f875e0e89f3e5082545b))
* ScriptableValueMatchDrawer being sealed ([aaeb36b](https://github.com/Hertzole/unity-toolbox/commit/aaeb36b05f5269bf3ed31437be7ddf5746d6fa17))

## [1.6.1](https://github.com/Hertzole/unity-toolbox/compare/v1.6.0...v1.6.1) (2023-09-09)


### Bug Fixes

* GenerateInputCallback not working on asset reference classes ([3e12619](https://github.com/Hertzole/unity-toolbox/commit/3e12619811291c180a400aeb4c5952a8c15d789a))

# [1.6.0](https://github.com/Hertzole/unity-toolbox/compare/v1.5.0...v1.6.0) (2023-09-08)


### Features

* GenerateLoad analyzer ([e48c7da](https://github.com/Hertzole/unity-toolbox/commit/e48c7da4f8d73496ae7f45c831f191655751700e))

# [1.5.0](https://github.com/Hertzole/unity-toolbox/compare/v1.4.0...v1.5.0) (2023-09-07)


### Features

* add input callbacks code fixer ([57d8f24](https://github.com/Hertzole/unity-toolbox/commit/57d8f242039df7dd2a8c50ee3ec9cb15e1e8cd6c))
* GenerateInputCallbacks attribute with generator ([91c07ff](https://github.com/Hertzole/unity-toolbox/commit/91c07ffc48d6f14f98512cde85d8e9899e4d2700))
* input callback analyzer ([306b625](https://github.com/Hertzole/unity-toolbox/commit/306b6255e7d8b530bba1d7a4f8dffd134f4e611c))

# [1.4.0](https://github.com/Hertzole/unity-toolbox/compare/v1.3.0...v1.4.0) (2023-08-31)


### Bug Fixes

* AddressableLoadGenerator not supporting generic names ([e5a5c3a](https://github.com/Hertzole/unity-toolbox/commit/e5a5c3aa00657a8e3f73ab466fff4d47e691db45))
* SubscribeMethodsGenerator not supporting generic names ([24b1744](https://github.com/Hertzole/unity-toolbox/commit/24b1744f7f58cdda3d49084b68febcf713c83b59))


### Features

* addressables support for cursor manager ([efc6b8a](https://github.com/Hertzole/unity-toolbox/commit/efc6b8ab0ce34396395876c371d004c1a074d12e))
* custom editor for scriptable value matches ([0e6dcde](https://github.com/Hertzole/unity-toolbox/commit/0e6dcde0f7ccafd2c286f7048b9eb4dc472ef409))

# [1.3.0](https://github.com/Hertzole/unity-toolbox/compare/v1.2.0...v1.3.0) (2023-08-30)


### Features

* Add/RemoveAllListeners extension method ([45a4d6a](https://github.com/Hertzole/unity-toolbox/commit/45a4d6a139f31c3f66b7461666cf0ecfecdccdd1))

# [1.2.0](https://github.com/Hertzole/unity-toolbox/compare/v1.1.0...v1.2.0) (2023-08-13)


### Bug Fixes

* compiler error on SpawnpointEditor.cs without addressables ([a50a20e](https://github.com/Hertzole/unity-toolbox/commit/a50a20ef0354fb2873589f42c95194c00fa2a607))
* tag, layer, and animation parameter fields may not using the correct label ([a9cd185](https://github.com/Hertzole/unity-toolbox/commit/a9cd1850b908ca2c7b7579705c9860067fb71b2c))


### Features

* CursorManager can now handle cursor locking automatically ([c43ea6b](https://github.com/Hertzole/unity-toolbox/commit/c43ea6bfac6812dc02f9ca4b7c9269781d810df2))
* identifier type ([9649c58](https://github.com/Hertzole/unity-toolbox/commit/9649c58e939a53b4bd8d521ed2c42885eaebe8e6))

# [1.1.0](https://github.com/Hertzole/unity-toolbox/compare/v1.0.0...v1.1.0) (2023-08-10)


### Bug Fixes

* generator not being able to use already existing fields ([1ca7615](https://github.com/Hertzole/unity-toolbox/commit/1ca7615947c660a5fa28779f8e06e1e9502f3691))


### Features

* add spawnpoint editor ([3b8fbfc](https://github.com/Hertzole/unity-toolbox/commit/3b8fbfc4346b72799e23e5618c30c36f51ce9a48))
* spawnpoints now returns transform rotation if list is empty ([0539bd8](https://github.com/Hertzole/unity-toolbox/commit/0539bd8089afc4f182a84b6b30c4b6bff910b671))

# 1.0.0 (2023-08-05)


### Bug Fixes

* cursor manager ([2ebaeaa](https://github.com/Hertzole/unity-toolbox/commit/2ebaeaa06eb5c6f33bc56056e844f07fbce994fa))
* example snippets ([8c378bf](https://github.com/Hertzole/unity-toolbox/commit/8c378bf6a3e5c942d9faca5c5177415d0a38c701))
* generator new lines ([5b2691c](https://github.com/Hertzole/unity-toolbox/commit/5b2691c74b3e272662131027e1922d5eb1a34cdd))
* namespace on IHasPlayerInput ([f12c1de](https://github.com/Hertzole/unity-toolbox/commit/f12c1dec07a6962c5952a89f4f09d628680a559b))
* on validate in network input in FishNet ([003f170](https://github.com/Hertzole/unity-toolbox/commit/003f17038a5718d44558800659b6e9d96e9e0a40))
* resetting wrong value in input manager editor ([415af5b](https://github.com/Hertzole/unity-toolbox/commit/415af5be1243c7c8ccf8b7ea419c733cfa7697bb))
* singleton warning on 2023.1 ([bd70b94](https://github.com/Hertzole/unity-toolbox/commit/bd70b9429b7093567fec2dfa0349fa455e361797))
* warnings and errors about extra meta files ([3c46659](https://github.com/Hertzole/unity-toolbox/commit/3c46659be0970b8283fc062c5050ae1d24436a0f))

# 1.0.0  (2023-08-05)

Initial release
