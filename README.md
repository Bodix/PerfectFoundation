# Perfect Foundation

The shared foundation every Perfect Core package is built on.

## Requirements

Unity 2019.3 or newer.

>The Asset Store build is submitted from Unity 2022.3 because the
store requires it, but the code itself targets 2019.3 and up.

## Installation

Import the package from the Asset Store. Perfect Foundation has no hard-coded paths: the folder can
sit anywhere in your project, and moving it later breaks nothing.

### Assembly definitions

Perfect Foundation ships as three assemblies:

| Assembly | Platforms | Contents |
|---|---|---|
| `PerfectCore.PerfectFoundation` | All | Everything under `Scripts/Runtime` |
| `PerfectCore.PerfectFoundation.Editor` | Editor | Drawers and inspectors |
| `PerfectCore.PerfectFoundation.Newtonsoft` | All | JSON integration, compiled only when Newtonsoft.Json is installed |

All three are auto-referenced, so `using PerfectCore.PerfectFoundation;` works straight away in Unity's default
`Assembly-CSharp` with nothing to set up. If your own code lives in assembly definitions, add
`PerfectCore.PerfectFoundation` to their references as usual — auto-referencing only ever applies to Unity's
predefined assemblies, so it never interferes with an asmdef-based project.

## Contents

- [Data assets](#data-assets) — `DataAsset`, `Database<T>`, `ConfigService`, JSON serialization
- [Event bus](#event-bus) — `IEventBus`, `EventBus`
- [Type selector](#type-selector) — `[TypeSelector]`, `[TypeSelectorName]`
- [Timer](#timer) — `Timer`
- [Comment](#comment) — `Comment`
- [Abstractions](#abstractions) — the interfaces the other Perfect Core packages are built on

### Data assets

`DataAsset` is a `ScriptableObject` that carries a stable, human-readable `Id` in
`folder:name` form. The ID is generated on first validation from the asset's name and its
parent folder, and can be rebuilt from the inspector's context menu ("Regenerate ID").

Use it for any static data (for example for configs):

```csharp
public class ItemConfig : DataAsset
{
    [SerializeField] private Sprite _icon;

    public Sprite Icon => _icon;
}
```

`Database<T>` is a `ScriptableObject` list of data assets with `GetById` lookup. It builds
its dictionary lazily on first access, reports duplicate IDs as errors, and warns when an
ID is missing.

```csharp
[CreateAssetMenu(menuName = "Game/Item Database")]
public class ItemDatabase : Database<ItemConfig> { }

ItemConfig sword = itemDatabase.GetById("weapons:sword");
```

`ConfigService` resolves a `DataAsset` by ID from a dictionary you build at startup — from
one database, from several, or from anywhere else. It reports an empty ID, a missing ID and
a type mismatch separately, so a bad save file tells you which of the three went wrong.

```csharp
var configService = new ConfigService(allConfigsById);
ItemConfig sword = configService.GetConfig<ItemConfig>("weapons:sword");
```

#### Data assets serialization

The point of a stable ID is what happens at save time, so the JSON side lives here too.
`DataAssetConverter<T>` writes any `DataAsset` reference as its ID string and reads it back
through a `ConfigService` — a save file then references configs by name instead of storing
copies of them. `JsonDataSerializer` is an `IDataSerializer` that wires that converter into
indented JSON.

```csharp
var serializer = new JsonDataSerializer(configService);

serializer.Serialize(saveData, filePath);
SaveData loaded = serializer.Deserialize<SaveData>(filePath);
```

Both live in a separate assembly that is compiled only when
`com.unity.nuget.newtonsoft-json` is installed. Without it the assembly is empty and nothing
else about the package changes.

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
```

Subscribing, unsubscribing and nested publishing are all safe during dispatch: `Publish`
takes a snapshot of the handler list before invoking it. Handlers are isolated — an
exception in one is logged and the rest still run. The bus locks around its handler table,
so subscriptions from other threads are safe; handlers themselves run on whichever thread
called `Publish`, so marshal to the main thread yourself if a handler touches the Unity API.

`EventBus.Clear()` drops every subscription — useful when tearing down a scene or a test.

### Type selector

`[TypeSelector]` turns a `[SerializeReference]` field into a searchable dropdown of every
concrete `[Serializable]` type derived from the field's type.

```csharp
[SerializeField, SerializeReference, TypeSelector]
private List<QuestObjective> _objectives = new List<QuestObjective>();
```

A type shows up in the dropdown when it is public (or public nested), non-abstract,
non-generic, marked `[Serializable]`, and not derived from `UnityEngine.Object`.

`[TypeSelectorName("...")]` overrides how a type is labelled. Without it, the nicified
class name is used.

```csharp
[Serializable, TypeSelectorName("Kill N enemies")]
public class KillEnemiesObjective : QuestObjective { }
```

Renaming or moving a type that is already serialized breaks the reference, as it does with
any `[SerializeReference]` field. Use `[MovedFrom]` when you rename one.

### Timer

`Timer` is a `MonoBehaviour` countdown that disables itself while idle, so an unused timer
costs nothing per frame. It ticks on `Update` or `FixedUpdate` — set `UpdateMethod` — and
reports progress through both callbacks and events.

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

Callbacks passed to `Start` last for that run only and are cleared when the timer completes
or stops; the events persist. `Start` only works in play mode. Perfect UI's `UiTimerText`
binds a label straight to one.

### Comment

`Comment` is an editor-only note you attach to a GameObject to explain why it is set up the
way it is. Its inspector shows the text as an info or warning box, with an Edit button.
Both the field and the text compile to nothing in a player build.

### Abstractions

Interfaces, and nothing behind them. Perfect UI, Perfect Inventory and Perfect Quests are
written against these, which is what keeps them independent of any particular tweening library,
input system or save format. Your own code can implement them the same way.

| Interface | Stands in for |
|---|---|
| `IAnimation`, `IShowHideAnimations` | Show/hide transitions — DOTween, LitMotion, coroutines, whatever you use |
| `IBackNavigationHandler`, `IBackNavigationService` | A back-button stack: a handler consumes the action, the service raises `QuitRequested` when nobody did |
| `IInstantiator` | Instantiation, so a container like VContainer or Zenject can inject into new objects |
| `IDataSerializer` | A save format: an `Extension` plus `Serialize` and `Deserialize` |

The implementations belong in your game, where the input system and the scene structure are
known.

### Utilities

`RectTransformData` is a serializable snapshot of a `RectTransform`'s anchors, pivot, size and
position, with `GetData()` / `SetData()` extension methods for capturing a layout and putting it
back.

## Inspector attributes

The package bundles a fork of [NaughtyAttributes](https://github.com/dbrizov/NaughtyAttributes)
and uses it for its own inspectors. Its assemblies and namespace are renamed to
`PerfectCore.PerfectFoundation.NaughtyAttributes`, so a project that already contains the original keeps
compiling.

The attributes work across your whole project out of the box — put `[Button]`, `[ShowIf]` or
`[Foldout]` on any MonoBehaviour and it draws. A component that carries no such attribute is
drawn exactly as Unity would draw it.

One thing to know if your project already uses another inspector extension. Unity allows a
single custom editor per type, and every tool of this kind — Odin Inspector, the original
NaughtyAttributes, Tri-Inspector — claims `UnityEngine.Object` to do its work. When two are
installed, only one wins, and which one is not deterministic; the symptom is that one tool's
attributes quietly stop drawing.

If that happens, add `PERFECTFOUNDATION_DISABLE_GLOBAL_INSPECTOR` to **Project Settings → Player →
Scripting Define Symbols**. Perfect Foundation then leaves the project-wide inspector to the other
tool, and its own components keep their attributes regardless.

## Third-party notice

This asset uses NaughtyAttributes under the MIT license; see `Third Party Notices.txt` in
the package for details.

## License

Copyright © 2020-2026 Bogdan Nikolayev. All Rights Reserved. See `LICENSE.md`.
