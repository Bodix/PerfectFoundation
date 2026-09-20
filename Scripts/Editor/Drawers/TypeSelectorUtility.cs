// Perfect Core for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

using System;
using System.Collections.Generic;
using UnityEditor;

namespace PerfectCore.PerfectFoundation.Editor
{
	public static class TypeSelectorUtility
	{
		private static readonly Dictionary<Type, string> DisplayNames = new Dictionary<Type, string>();

		public static string GetDisplayName(Type type)
		{
			if (DisplayNames.TryGetValue(type, out string displayName))
				return displayName;

			TypeSelectorNameAttribute attribute = (TypeSelectorNameAttribute)Attribute.GetCustomAttribute(
				type, typeof(TypeSelectorNameAttribute), false);
			displayName = attribute != null && !string.IsNullOrEmpty(attribute.Name)
				? attribute.Name
				: ObjectNames.NicifyVariableName(type.Name);

			DisplayNames.Add(type, displayName);

			return displayName;
		}
	}
}