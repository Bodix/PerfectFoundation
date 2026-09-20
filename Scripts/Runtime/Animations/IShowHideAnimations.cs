// Perfect Core for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

namespace PerfectCore.PerfectFoundation
{
	public interface IShowHideAnimations
	{
		IAnimation ShowAnimation { get; }
		IAnimation HideAnimation { get; }
	}
}