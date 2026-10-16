// Perfect Foundation for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

using System;

namespace PerfectCore.PerfectFoundation
{
	/// <summary>
	/// Marks a base class or interface whose subclasses are saved by type ID.
	/// <see cref="TypeIds.FindProblems" /> reports every subclass without its own <see cref="TypeIdAttribute" />.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
	public sealed class RequireTypeIdAttribute : Attribute
	{
	}
}
