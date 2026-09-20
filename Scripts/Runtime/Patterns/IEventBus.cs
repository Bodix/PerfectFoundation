// Perfect Core for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

using System;

namespace PerfectCore
{
	public interface IEventBus
	{
		void Subscribe<T>(Action<T> handler);
		void Unsubscribe<T>(Action<T> handler);
		void Publish<T>(T message);
	}
}