// Perfect Core for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

using UnityEngine;

namespace PerfectCore
{
	/// <summary>
	/// Abstracts the instantiation process to allow external frameworks 
	/// to inject dependencies into newly created objects.
	/// </summary>
	public interface IInstantiator
	{
		T Instantiate<T>(T prefab, Transform parent) where T : Component;
	}
}