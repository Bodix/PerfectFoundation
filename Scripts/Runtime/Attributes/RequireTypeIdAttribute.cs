// Perfect Foundation for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

using System;

namespace PerfectCore.PerfectFoundation
{
	/// <summary>
	/// Marks a base class or interface whose subclasses are saved by type ID and sets the kind their IDs start with,
	/// e.g. <c>[RequireTypeId("condition")]</c> for <c>condition.all_of</c>. <see cref="TypeIds.FindProblems" />
	/// reports every subclass without its own <see cref="TypeIdAttribute" /> of this kind.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
	public sealed class RequireTypeIdAttribute : Attribute
	{
		public RequireTypeIdAttribute(string kind)
		{
			Kind = kind;
		}

		public string Kind { get; }
	}
}
