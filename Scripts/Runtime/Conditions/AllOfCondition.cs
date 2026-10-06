// Perfect Foundation for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace PerfectCore.PerfectFoundation
{
	/// <summary>
	/// Met when every inner condition is met. Empty slots are skipped.
	/// Without inner conditions it is always met: an empty condition does not restrict.
	/// </summary>
	[Serializable]
	[TypeSelectorName("All of")]
	public class AllOfCondition : ICondition
	{
		[SerializeReference, TypeSelector]
		public List<ICondition> Conditions = new List<ICondition>();

		public bool IsMet(object context)
		{
			if (Conditions == null)
				return true;

			foreach (ICondition condition in Conditions)
			{
				if (condition != null && !condition.IsMet(context))
					return false;
			}

			return true;
		}
	}
}
