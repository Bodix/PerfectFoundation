# TODO (internal)

Not shipped: Unity ignores folders ending with `~`, so this file is not imported
into the project and is not part of the Asset Store package. It keeps the design
backlog that used to live as TODO comments in public source files.

## TypeSelector

- Nested dropdown paths via `/` in `TypeSelectorName` (e.g. "Combat/Kill Enemies");
  the button shows the last segment only.
- Localization of type display names.
- Custom ordering of types in the dropdown.
- Hiding specific types from the dropdown.
- Safe type renaming, via `SerializationUtility.HasManagedReferencesWithMissingTypes`
  and `SerializationUtility.GetManagedReferencesWithMissingTypes`.

## IAnimation

- Add `CancellationToken cancellationToken = default` to `Play`, and cancel that
  token in `OnDestroy`.
- Add a `Stop()` method.

## Timer

- Check for an enabled GameObject in `Start`, and review the `Awake` logic afterwards.
- Consider removing the `_onStart` callback (the `Started` event covers it).
- An inspector button `[Button("Start (60 sec)")] TestStart()` was removed before the
  1.0.0 release. Restore it on `Timer` if a one-click play-mode test is wanted.
