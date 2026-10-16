// Perfect Foundation for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

#if NEWTONSOFT_JSON
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PerfectCore.PerfectFoundation.Newtonsoft
{
	/// <summary>
	/// Writes the type of polymorphic data as its <see cref="TypeIdAttribute" /> instead of the class name
	/// and reads back only classes that have an ID. Use it with <see cref="TypeNameHandling.Auto" />.
	/// </summary>
	public class TypeIdSerializationBinder : ISerializationBinder
	{
		public void BindToName(Type serializedType, out string assemblyName, out string typeName)
		{
			assemblyName = null;
			if (!TypeIds.TryGetId(serializedType, out typeName))
				throw new JsonSerializationException($"'{serializedType.FullName}' is saved in a field " +
					"of another type, so it needs a [TypeId] attribute.");
		}

		public Type BindToType(string assemblyName, string typeName)
		{
			if (TypeIds.TryGetType(typeName, out Type type))
				return type;

			throw new JsonSerializationException($"Unknown type ID '{typeName}'.");
		}
	}
}
#endif
