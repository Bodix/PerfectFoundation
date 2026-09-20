// Perfect Core for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PerfectCore.Editor
{
	[CustomPropertyDrawer(typeof(TypeSelectorAttribute))]
	public class TypeSelectorDrawer : AttributePropertyDrawer<TypeSelectorAttribute>
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property, true);
		}

		protected override SerializedPropertyType[] SupportedTypes => new[]
		{
			SerializedPropertyType.ManagedReference
		};

		protected override void OnValidatedGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			GUIContent buttonContent = new GUIContent(GetManagedReferenceValueTypename(property));
			GUIStyle buttonStyle = EditorStyles.popup;
			float buttonWidth = buttonStyle.CalcSize(buttonContent).x + 5f;
			float maxAvailableWidth = position.width - EditorGUIUtility.labelWidth;
			buttonWidth = Mathf.Min(buttonWidth, Mathf.Max(maxAvailableWidth, 50f));
			Rect dropdownButtonRect = new Rect(
				position.xMax - buttonWidth,
				position.y,
				buttonWidth,
				EditorGUIUtility.singleLineHeight
			);

			if (EditorGUI.DropdownButton(dropdownButtonRect, buttonContent, FocusType.Keyboard, buttonStyle))
			{
				Type baseType = property.GetManagedReferenceFieldType();
				TypeSelectorDropdown dropdown = new TypeSelectorDropdown(
					TypeCache.GetTypesDerivedFrom(baseType)
						.Append(baseType)
						.Where(x =>
							(x.IsPublic || x.IsNestedPublic) &&
							!x.IsAbstract &&
							!x.IsGenericType &&
							!typeof(UnityEngine.Object).IsAssignableFrom(x) &&
							System.Attribute.IsDefined(x, typeof(SerializableAttribute))
						));
				dropdown.TypeSelected += type =>
				{
					property.serializedObject.Update();

					object obj = property.CreateManagedReferenceValue(type);
					property.isExpanded = obj != null;

					property.serializedObject.ApplyModifiedProperties();
				};

				dropdown.Show(dropdownButtonRect);
			}

			EditorGUI.PropertyField(position, property, label, true);
		}

		private string GetManagedReferenceValueTypename(SerializedProperty property)
		{
			Type type = property.GetManagedReferenceValueType();

			return type == null ? "<NULL>" : TypeSelectorUtility.GetDisplayName(type);
		}
	}
}