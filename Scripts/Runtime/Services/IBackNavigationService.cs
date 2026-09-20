// Perfect Core for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

using System;

namespace PerfectCore
{
	public interface IBackNavigationService
	{
		/// <summary>
		/// Invoked when there are no handlers left to consume the back action.
		/// </summary>
		event Action QuitRequested;

		void Register(IBackNavigationHandler handler);

		void Unregister(IBackNavigationHandler handler);
	}
}