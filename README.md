# Perfect Foundation

The shared foundation every Perfect Core package is built on.

## Requirements

- Unity 2019.3 or newer.

    >The Asset Store build is submitted from Unity 2022.3 because the store requires it, but the code itself targets 2019.3 and up.

## Installation

1. Import the package from the Asset Store.

    >Perfect Foundation has no hard-coded paths: the folder can sit anywhere in your project, and moving it later breaks nothing.

2. [Optional] If your own code lives in assembly definitions, add PerfectCore.PerfectFoundation to their references.

## Contents

- [Data assets](#data-assets) — `DataAsset`, JSON serialization, `ConfigService`, `Database<T>`
- [Event bus](#event-bus) — `IEventBus`, `EventBus`
- [Type selector](#type-selector) — `[TypeSelector]`, `[TypeSelectorName]`
- [Timer](#timer) — `Timer`
- [Comment](#comment) — `Comment`
- [Structs](#structs) — `TransformData`, `RectTransformData`
- [Abstractions](#abstractions) — the interfaces the other Perfect Core packages are built on

### Data assets

`DataAsset` is a `ScriptableObject` that carries a stable, human-readable `Id` in `folder:name` format. The ID is generated instantly after creation from the asset's name and its parent folder, and can be rebuilt with the "Regenerate ID" button in the inspector.

This ID allows configuration files to be serialized correctly: the configuration is serialized and deserialized only by its ID, rather than by saving every individual configuration value. This offers two advantages:

1. Serializable data types (such as game saves or game settings) take up less space.
2. It’s easier to add changes to the game, since the serialized data contains no configuration values (only the ID). This way, you don't have to overwrite serialized configuration data. You simply update the configuration files, and everything updates automatically for the players.

Use it for any static data, for example for configs:

```csharp
public class ItemConfig : DataAsset
{
    [SerializeField]
    private Sprite _icon;

    public Sprite Icon => _icon;
}
```

#### Serialization

For now, automatic serialization via Newtonsoft.Json is supported out of the box. Support for automatic serialization via ProtoBuf (binary serialization) is planned for the future.

`JsonDataSerializer` does all the serialization for you. Give it a `ConfigService` and every `DataAsset` reference in your save data — any subclass, at any depth — is written as an ID and resolved back on load.

```csharp
var serializer = new JsonDataSerializer(configService);

serializer.Serialize(saveData, filePath);
SaveData loaded = serializer.Deserialize<SaveData>(filePath);
```

>It lives in a separate assembly that is compiled only when `com.unity.nuget.newtonsoft-json` is installed.

#### ConfigService

`ConfigService` is the ID-to-asset lookup all of this goes through. You build the dictionary once at startup — from one database, from several databases, from Addressables, from anywhere else — and hand it over. It is a good fit for a DI container: fill the service before the container is built, register it, and everything that needs a config receives the same instance. With Addressables that is a single startup step, since configs can be loaded by label and keyed by their own ID.

```csharp
// Load every config by label, then register the service (VContainer shown here).
IList<DataAsset> configs = await Addressables.LoadAssetsAsync<DataAsset>("configs", null).Task;

builder.RegisterInstance(new ConfigService(configs.ToDictionary(config => config.Id)));
```

Then you can use it conveniently via DI:

```csharp
[Inject]
private readonly ConfigService configService;

ItemConfig sword = configService.GetConfig<ItemConfig>("weapons:sword");
```

#### Database\<T>

`Database<T>` is the simpler alternative — a `ScriptableObject` holding a static, hand-filled list of data assets with the same `GetById` lookup. Use it when there is no container to register a service into, or when one designer-editable list is all a feature needs.

```csharp
[CreateAssetMenu(menuName = "Game/Item Database")]
public class ItemDatabase : Database<ItemConfig> { }

ItemConfig sword = itemDatabase.GetById("weapons:sword");
```

It builds its dictionary lazily on first access, reports duplicate IDs as errors and warns when an ID is missing. A database also makes a convenient source for the dictionary a `ConfigService` is built from.

### Event bus

`IEventBus` / `EventBus` — an in-memory publish/subscribe bus. Any type can be a message.

```csharp
public readonly struct ItemCollected
{
    public readonly ItemConfig Item;
    public readonly int Amount;

    public ItemCollected(ItemConfig item, int amount)
    {
        Item = item;
        Amount = amount;
    }
}

eventBus.Subscribe<ItemCollected>(OnItemCollected);
eventBus.Publish(new ItemCollected(item, amount));
eventBus.Unsubscribe<ItemCollected>(OnItemCollected);
eventBus.Clear(); // Drops every subscription — useful when tearing down a scene or a test.
```

### Type selector

`[TypeSelector]` turns a `[SerializeReference]` field into a searchable dropdown of every concrete `[Serializable]` type derived from the field's type.

```csharp
[SerializeField, SerializeReference, TypeSelector]
private List<QuestObjective> _objectives = new List<QuestObjective>();
```

A type shows up in the dropdown when it is public (or public nested), non-abstract, non-generic, marked `[Serializable]`, and not derived from `UnityEngine.Object`.

`[TypeSelectorName("...")]` overrides how a type is labelled. Without it, the nicified class name is used.

```csharp
[Serializable, TypeSelectorName("Kill N enemies")]
public class KillEnemiesObjective : QuestObjective { }
```

Renaming or moving a type that is already serialized breaks the reference, as it does with any `[SerializeReference]` field. Use `[MovedFrom]` when you rename one.

### Timer

`Timer` is a `MonoBehaviour` countdown that disables itself while idle, so an unused timer costs nothing per frame. It ticks on `Update` or `FixedUpdate` — set `UpdateMethod` — and reports progress through both callbacks and events.

```csharp
timer.Start(60f, onComplete: () => Debug.Log("Time is up"));

timer.Pause();
timer.Resume();
timer.Stop();

timer.SetRemainingTime(timer.RemainingTime + 10f);
```

| Member | |
|---|---|
| `Start(seconds, onStart, onUpdate, onStop, onComplete)` | All callbacks optional |
| `Pause()` / `Resume()` / `Stop()` | `Stop` raises `onStop`, not `onComplete` |
| `SetRemainingTime(float)` | Only while started |
| `RemainingTime`, `IsStarted`, `IsPaused` | |
| `Started`, `Updated`, `Stopped`, `Completed` | Events, alongside the per-call callbacks |

Callbacks passed to `Start` last for that run only and are cleared when the timer completes or stops; the events persist. `Start` only works in play mode. Perfect UI's `UiTimerText` binds a label straight to one.

### Comment

`Comment` is an editor-only note you attach to a GameObject to explain why it is set up the way it is. Its inspector shows the text as an info or warning box, with an Edit button. Both the field and the text compile to nothing in a player build.

### Structs

`TransformData` and `RectTransformData` are serializable snapshots of a transform's state. Both can be serialized to a save file.

```csharp
// Position, rotation, local scale.
TransformData snapshot = transform.GetData();
transform.SetData(snapshot);

// Anchored position, size delta, anchors, pivot.
RectTransformData layout = rectTransform.GetData();
rectTransform.SetData(layout);
```

### Abstractions

Interfaces, and nothing behind them. Perfect UI, Perfect Inventory and Perfect Quests are written against these, which is what keeps them independent of any particular tweening library, input system or save format. Your own code can implement them the same way.

| Interface | Stands in for |
|---|---|
| `IAnimation`, `IShowHideAnimations` | Show/hide transitions — DOTween, LitMotion, coroutines, whatever you use |
| `IBackNavigationHandler`, `IBackNavigationService` | A back-button stack: a handler consumes the action, the service raises `QuitRequested` when nobody did |
| `IInstantiator` | Instantiation, so a container like VContainer or Zenject can inject into new objects |
| `IDataSerializer` | A save format: an `Extension` plus `Serialize` and `Deserialize` |

The implementations belong in your game, where the input system and the scene structure are known.

## [NaughtyAttributes](https://github.com/dbrizov/NaughtyAttributes)

The package bundles a fork of [NaughtyAttributes](https://github.com/dbrizov/NaughtyAttributes) and uses it for its own inspectors.

>Its assemblies and namespace are renamed to `PerfectCore.PerfectFoundation.NaughtyAttributes`, so a project that already contains the original keeps compiling.

If you already use another inspector extension — Odin Inspector, the original NaughtyAttributes, Tri-Inspector — you can turn Perfect Foundation's project-wide inspector off. Add `PERFECTFOUNDATION_DISABLE_GLOBAL_INSPECTOR` to **Project Settings → Player → Scripting Define Symbols**: the project-wide inspector is left to the other tool, and Perfect Foundation's own components keep their attributes regardless.

>Unity allows a single custom editor per type, and every tool of this kind claims `UnityEngine.Object` to do its work. With two installed, only one wins — and which one is not deterministic. The symptom is that one tool's attributes quietly stop drawing.

## Third-party notice

This asset uses NaughtyAttributes under the MIT license; see `Third Party Notices.txt` in the package for details.

## License

Copyright © 2020-2026 Bogdan Nikolayev. All Rights Reserved. See `LICENSE.md`.
