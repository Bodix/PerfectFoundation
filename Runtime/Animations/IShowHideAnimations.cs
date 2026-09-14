// Perfect Core for Unity
// Copyright © 2020 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

namespace PerfectCore
{
	public interface IShowHideAnimations
	{
		IAnimation ShowAnimation { get; }
		IAnimation HideAnimation { get; }
	}
}