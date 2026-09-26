# Changelog

All notable changes to Perfect Foundation are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this package adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.1] - 2026-09-26

### Changed

- The minimum Unity version is now 2022.3.
- The bundled NaughtyAttributes is updated from 2.0.7 to 2.1.6. This fixes compile errors in Unity 6.5 and newer, where `Object.GetInstanceID()` became obsolete.

### Removed

- The documentation files bundled with NaughtyAttributes (`README.html`, `Documentation~`).

## [1.0.0] - 2026-09-20

First public release.

### Added

- **`DataAsset`** - `ScriptableObject` base class carrying a stable, human-readable
  identifier in `folder:name` form. The identifier is generated on first validation
  and can be rebuilt with the "Regenerate ID" button in the inspector.
- **`Database<T>`** - `ScriptableObject` list of data assets with `GetById` lookup
  and duplicate-identifier reporting.
- **`ConfigService`** - runtime registry that resolves a `DataAsset` by identifier
  with type checking and explicit error reporting.
- **`IEventBus` / `EventBus`** - in-memory, thread-safe publish/subscribe bus.
  Subscribing, unsubscribing and nested publishing are safe during dispatch, and an
  exception in one handler does not stop the others.
- **`[TypeSelector]`** - turns a `[SerializeReference]` field into a searchable
  dropdown of every concrete `[Serializable]` type derived from the field's type.
  **`[TypeSelectorName]`** overrides how a type is labelled in that dropdown.
- **`Timer`** - `MonoBehaviour` countdown that disables itself while idle, ticks on
  `Update` or `FixedUpdate`, can be paused and resumed, and reports progress through
  both callbacks and events.
- **`Comment`** - editor-only note attachable to a GameObject. Compiles to nothing in
  a player build.
- **`IAnimation` / `IShowHideAnimations`** - tweening-library-agnostic contracts for
  show/hide transitions.
- **`IBackNavigationHandler` / `IBackNavigationService`** - contracts describing a
  back-button stack.
- **`IInstantiator`** - abstraction over instantiation, so an external container can
  inject dependencies into newly created objects.
- **`IDataSerializer`**, **`TransformData`** and **`RectTransformData`** utilities.
- **Newtonsoft.Json integration** (optional) - `JsonDataSerializer` and
  `DataAssetConverter`, compiled only when `com.unity.nuget.newtonsoft-json` is
  present in the project.
