# TODO (internal)

>**Not shipped:**
<br>Unity ignores folders ending with `~`, so this file is not imported into the project and is not part of the Asset Store package. It keeps the design backlog that used to live as TODO comments in public source files.

## TypeSelector

- Nested dropdown paths via `/` in `TypeSelectorName` (e.g. "Combat/Kill Enemies"); the button shows the last segment only.
- Localization of type display names.
- Custom ordering of types in the dropdown.
- Hiding specific types from the dropdown.
- Safe type renaming, via `SerializationUtility.HasManagedReferencesWithMissingTypes` and `SerializationUtility.GetManagedReferencesWithMissingTypes`.

## IAnimation

- Add `CancellationToken cancellationToken = default` to `Play`, and cancel that token in `OnDestroy`.
- Add a `Stop()` method.

## Timer

- Check for an enabled GameObject in `Start`, and review the `Awake` logic afterwards.
- Consider removing the `_onStart` callback (the `Started` event covers it).

## Package

- Asset Store Publishing Tools 12 export a folder ending with `~` only when its files have `.meta` files, so `Media~` and `Documentation~` are not uploaded. When the Uploader offers to generate meta files for hidden folders, answer No. After publication, install Perfect Foundation from My Assets and check in `Library/PackageCache` that neither folder is there.
