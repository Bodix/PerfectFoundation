// Perfect Foundation for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace PerfectCore.PerfectFoundation
{
	/// <summary>
	/// Finds classes by their <see cref="TypeIdAttribute" /> and back.
	/// </summary>
	public static class TypeIds
	{
		private static readonly Lazy<Dictionary<string, Type>> Types =
			new Lazy<Dictionary<string, Type>>(BuildTypes);

		/// <summary>
		/// Gets the ID of a class. False when the class has no <see cref="TypeIdAttribute" />.
		/// </summary>
		public static bool TryGetId(Type type, out string id)
		{
			id = type?.GetCustomAttribute<TypeIdAttribute>(false)?.Id;
			return !string.IsNullOrEmpty(id);
		}

		/// <summary>
		/// Gets the class with this ID. False for an unknown ID and for an ID that several classes share.
		/// </summary>
		public static bool TryGetType(string id, out Type type)
		{
			type = null;
			return !string.IsNullOrEmpty(id) && Types.Value.TryGetValue(id, out type) && type != null;
		}

		/// <summary>
		/// Lists empty IDs and IDs that several classes share. Call it from an edit mode test.
		/// </summary>
		public static List<string> FindProblems()
		{
			List<string> problems = new List<string>();
			foreach (IGrouping<string, Type> group in FindTypesWithId().GroupBy(GetId))
			{
				if (group.Key.Length == 0)
					problems.AddRange(group.Select(type => $"{type.FullName} has an empty type ID."));
				else if (group.Count() > 1)
					problems.Add($"Type ID '{group.Key}' is shared by " +
						$"{string.Join(", ", group.Select(type => type.FullName))}.");
			}

			return problems;
		}

		private static Dictionary<string, Type> BuildTypes()
		{
			Dictionary<string, Type> types = new Dictionary<string, Type>();
			foreach (IGrouping<string, Type> group in FindTypesWithId().GroupBy(GetId))
			{
				if (group.Key.Length == 0)
					continue;

				if (group.Count() > 1)
				{
					// Data of a shared ID could be read into the wrong class, so it is not read at all.
					Debug.LogError($"[{nameof(TypeIds)}] Type ID '{group.Key}' is shared by several classes " +
						$"and cannot be read from a save. See {nameof(TypeIds)}.{nameof(FindProblems)}().");
					types.Add(group.Key, null);
					continue;
				}

				types.Add(group.Key, group.First());
			}

			return types;
		}

		private static string GetId(Type type)
		{
			return type.GetCustomAttribute<TypeIdAttribute>(false).Id ?? string.Empty;
		}

		private static IEnumerable<Type> FindTypesWithId()
		{
			Assembly foundation = typeof(TypeIdAttribute).Assembly;
			string foundationName = foundation.GetName().Name;
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				// Only this assembly and the ones that reference it can use the attribute.
				if (assembly != foundation && (assembly.IsDynamic ||
					assembly.GetReferencedAssemblies().All(reference => reference.Name != foundationName)))
					continue;

				foreach (Type type in GetTypes(assembly))
				{
					if (type != null && type.IsDefined(typeof(TypeIdAttribute), false))
						yield return type;
				}
			}
		}

		private static Type[] GetTypes(Assembly assembly)
		{
			try
			{
				return assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException exception)
			{
				return exception.Types;
			}
		}
	}
}
