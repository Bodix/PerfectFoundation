// Perfect Core for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

using System;

namespace PerfectCore
{
	public interface IAnimation
	{
		/// <summary>
		/// Plays the animation. <paramref name="onStart"/> is raised once playback begins,
		/// <paramref name="onComplete"/> once it finishes.
		/// </summary>
		void Play(Action onStart = null, Action onComplete = null);
	}
}