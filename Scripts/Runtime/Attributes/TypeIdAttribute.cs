// Perfect Foundation for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

using System;

namespace PerfectCore.PerfectFoundation
{
	/// <summary>
	/// A short stable name of a class for save files, e.g. <c>[TypeId("kill_ships")]</c>.
	/// <para>
	/// A save has to store the type of polymorphic data: a field of a base class that holds a subclass.
	/// With an ID instead of the class name, the class can be renamed or moved to another namespace or assembly
	/// without breaking old saves. Only classes with an ID are read back, so a tampered save cannot create
	/// any other class.
	/// </para>
	/// <para>
	/// Keep IDs unique and never change them once saves exist. A subclass does not inherit the ID.
	/// </para>
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
