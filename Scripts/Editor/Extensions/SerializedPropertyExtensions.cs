// Perfect Foundation for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

using System;
using UnityEditor;

namespace PerfectCore.PerfectFoundation.Editor
{
	public static class SerializedPropertyExtensions
	{
		public static Type GetManagedReferenceFieldType(this SerializedProperty serializedProperty)
		{
			return GetManagedReferenceType(serializedProperty.managedReferenceFieldTypename);
		}

		public static Type GetManagedReferenceValueType(this SerializedProperty serializedProperty)
		{
			return GetManagedReferenceType(serializedProperty.managedReferenceFullTypename);
		}

		public static object CreateManagedReferenceValue(this SerializedProperty serializedProperty, Type type)
		{
			object obj = type != null ? Activator.CreateInstance(type) : null;
			serializedProperty.managedReferenceValue = obj;

			return obj;
		}

		private static Type GetManagedReferenceType(string managedReferenceTypename)
		{
			if (string.IsNullOrEmpty(managedReferenceTypename))
				return null;

			// Unity's format is "AssemblyName Namespace.Type", with "/" before nested type names.
			// Type.GetType finds the already loaded assembly without Assembly.Load, 
			// which the Unity 6.6 analyzers flag in packages installed from the Asset Store.
			int splitIndex = managedReferenceTypename.IndexOf(' ');
			string assemblyName = managedReferenceTypename.Substring(0, splitIndex);
			string typeName = managedReferenceTypename.Substring(splitIndex + 1).Replace('/', '+');

			return Type.GetType(typeName + ", " + assemblyName);
		}
	}
}