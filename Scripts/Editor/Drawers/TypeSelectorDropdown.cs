// Perfect Foundation for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace PerfectCore.PerfectFoundation.Editor
{
	public class TypeSelectorDropdown : AdvancedDropdown
	{
		private readonly IEnumerable<Type> _types;

		public TypeSelectorDropdown(IEnumerable<Type> types) : base(new AdvancedDropdownState())
		{
			_types = types;

			// Increase the minimum height to fit more items without scrolling.
			minimumSize = new Vector2(minimumSize.x, 300f);
		}

		public event Action<Type> TypeSelected;

		protected override AdvancedDropdownItem BuildRoot()
		{
			AdvancedDropdownItem root = new AdvancedDropdownItem("Select type");

			root.AddChild(new TypeSelectorDropdownItem(null));
			foreach (Type type in _types)
				root.AddChild(new TypeSelectorDropdownItem(type));

			return root;
		}

		protected override void ItemSelected(AdvancedDropdownItem item)
		{
			base.ItemSelected(item);

			if (item is TypeSelectorDropdownItem typeSelectorItem)
				TypeSelected?.Invoke(typeSelectorItem.Type);
		}

		private class TypeSelectorDropdownItem : AdvancedDropdownItem
		{
			public TypeSelectorDropdownItem(Type type) : base(type != null ? TypeSelectorUtility.GetDisplayName(type) : "Null")
			{
				Type = type;
			}

			public Type Type { get; }
		}
	}
}