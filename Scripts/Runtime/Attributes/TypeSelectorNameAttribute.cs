// Perfect Core for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

using System;

namespace PerfectCore.PerfectFoundation
{
	/// <summary>
	/// Custom display name for this type in a <see cref="TypeSelectorAttribute"/> dropdown.
	/// If omitted, the nicified class name is used.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
	public class TypeSelectorNameAttribute : Attribute
	{
		public TypeSelectorNameAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; }
	}
}