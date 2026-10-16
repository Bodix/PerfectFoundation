// Perfect Foundation for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

using System;

namespace PerfectCore.PerfectFoundation
{
	/// <summary>
	/// A short ID of a class for save files, e.g. <c>[TypeId("kill_ships")]</c>.
	/// A save stores it instead of the class name, so the class can be renamed or moved.
	/// The ID must be unique and must never change once saves exist. Subclasses do not inherit it.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
	public sealed class TypeIdAttribute : Attribute
	{
		public TypeIdAttribute(string id)
		{
			Id = id;
		}

		public string Id { get; }
	}
}
