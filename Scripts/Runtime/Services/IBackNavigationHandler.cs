// Perfect Core for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

namespace PerfectCore.PerfectFoundation
{
	/// <summary>
	/// Defines an interface for handling back navigation events.
	/// </summary>
	public interface IBackNavigationHandler
	{
		/// <summary>
		/// Invoked when the back action is performed.
		/// Returns true if the action was consumed, stopping further propagation.
		/// </summary>
		bool OnBackPressed();
	}
}