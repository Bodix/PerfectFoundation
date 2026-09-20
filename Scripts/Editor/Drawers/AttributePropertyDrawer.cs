// Perfect Core for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PerfectCore.PerfectFoundation.Editor
{
	/// <summary>
	/// Base class for property drawers bound to a single <see cref="PropertyAttribute"/>.
	/// A derived drawer declares its own [CustomPropertyDrawer(typeof(TAttribute))] attribute
	/// and lists the property types it supports in <see cref="SupportedTypes"/>.
	/// </summary>
	public abstract class AttributePropertyDrawer<TAttribute> : PropertyDrawer where TAttribute : PropertyAttribute
	{
		protected abstract SerializedPropertyType[] SupportedTypes { get; }

		protected TAttribute Attribute => (TAttribute)attribute;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (IsSupported(property))
			{
				OnValidatedGUI(position, property, label);
			}
			else
			{
				string typeErrorText = "Type " + property.propertyType
					+ " is not supported with this property attribute.\n\n"
					+ "Supported property types:\n"
					+ string.Join("\n", SupportedTypes.Select(x => "- " + x));

				EditorGUI.HelpBox(GUILayoutUtility.GetRect(new GUIContent(typeErrorText), EditorStyles.helpBox),
					typeErrorText, MessageType.Error);
			}
		}

		protected virtual bool IsSupported(SerializedProperty property)
		{
			return SupportedTypes.Any(x => x == property.propertyType);
		}

		protected virtual void OnValidatedGUI(Rect position, SerializedProperty property, GUIContent label) { }
	}
}