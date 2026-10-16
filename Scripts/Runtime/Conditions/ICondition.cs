// Perfect Foundation for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

namespace PerfectCore.PerfectFoundation
{
	/// <summary>
	/// A data-driven check set up in the inspector: "only at night", "only from elites", "player level 10 or higher".
	/// Store it in a <c>[SerializeReference, TypeSelector]</c> field.
	/// Combine conditions with <see cref="AllOfCondition"/>, <see cref="AnyOfCondition"/> and <see cref="NotCondition"/>.
	/// </summary>
	[RequireTypeId("condition")]
	public interface ICondition
	{
		/// <summary>
		/// Checks the condition against a context: whatever the caller knows, such as the player, the place or the time.
		/// The context can be null or of a type the condition does not know. Usually that means "not met".
		/// </summary>
		bool IsMet(object context);
	}
}
