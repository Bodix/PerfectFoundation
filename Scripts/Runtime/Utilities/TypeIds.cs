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
	/// Finds classes by their <see cref="TypeIdAttribute" /> and back, and checks the IDs of the project.
	/// </summary>
	public static class TypeIds
	{
		private static readonly Lazy<Dictionary<string, Type>> TypesById =
			new Lazy<Dictionary<string, Type>>(BuildTypesById);

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
			return !string.IsNullOrEmpty(id) && TypesById.Value.TryGetValue(id, out type) && type != null;
		}

		/// <summary>
		/// Lists classes without the ID that <see cref="RequireTypeIdAttribute" /> asks for,
		/// empty IDs and IDs shared by several classes. Call it from an edit mode test.
		/// </summary>
		public static List<string> FindProblems()
		{
			List<Type> types = FindTypes().ToList();
			List<string> problems = new List<string>();

			foreach (Type type in types)
			{
				Type requiringType = FindRequiringType(type);
				if (requiringType != null && !type.IsDefined(typeof(TypeIdAttribute), false))
					problems.Add($"{type.FullName} needs a type ID: it derives from {requiringType.FullName}.");
			}

			foreach (IGrouping<string, Type> group in GroupById(types))
			{
				if (group.Key.Length == 0)
					problems.AddRange(group.Select(type => $"{type.FullName} has an empty type ID."));
				else if (group.Count() > 1)
					problems.Add($"Type ID '{group.Key}' is shared by " +
						$"{string.Join(", ", group.Select(type => type.FullName))}.");
			}

			return problems;
		}

		private static Dictionary<string, Type> BuildTypesById()
		{
			Dictionary<string, Type> typesById = new Dictionary<string, Type>();
			foreach (IGrouping<string, Type> group in GroupById(FindTypes()))
			{
				if (group.Key.Length == 0)
					continue;

				if (group.Count() > 1)
				{
					// Data of a shared ID could be read into the wrong class, so it is not read at all.
					Debug.LogError($"[{nameof(TypeIds)}] Type ID '{group.Key}' is shared by several classes " +
						$"and cannot be read from a save. See {nameof(TypeIds)}.{nameof(FindProblems)}().");
					typesById.Add(group.Key, null);
					continue;
				}

				typesById.Add(group.Key, group.First());
			}

			return typesById;
		}

		private static IEnumerable<IGrouping<string, Type>> GroupById(IEnumerable<Type> types)
		{
			return types
				.Where(type => type.IsDefined(typeof(TypeIdAttribute), false))
				.GroupBy(type => type.GetCustomAttribute<TypeIdAttribute>(false).Id ?? string.Empty);
		}

		/// <summary>
		/// Finds the base class or interface that asks this class for an ID, or null.
		/// </summary>
		private static Type FindRequiringType(Type type)
		{
			// Only a class that can be created gets into a save.
			if (type.IsAbstract || type.IsInterface || type.ContainsGenericParameters)
				return null;

			for (Type baseType = type.BaseType; baseType != null; baseType = baseType.BaseType)
			{
				if (baseType.IsDefined(typeof(RequireTypeIdAttribute), false))
					return baseType;
			}

			return type.GetInterfaces().FirstOrDefault(i => i.IsDefined(typeof(RequireTypeIdAttribute), false));
		}

		/// <summary>
		/// All types of this assembly and of the assemblies that reference it: only they can use type IDs.
		/// </summary>
		private static IEnumerable<Type> FindTypes()
		{
			Assembly foundation = typeof(TypeIdAttribute).Assembly;
			string foundationName = foundation.GetName().Name;
			return AppDomain.CurrentDomain.GetAssemblies()
				.Where(assembly => assembly == foundation || (!assembly.IsDynamic &&
					assembly.GetReferencedAssemblies().Any(reference => reference.Name == foundationName)))
				.SelectMany(GetTypes)
				.Where(type => type != null);
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
