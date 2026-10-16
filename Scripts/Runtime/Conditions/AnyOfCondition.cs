// Perfect Foundation for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace PerfectCore.PerfectFoundation
{
	/// <summary>
	/// Met when at least one inner condition is met. Empty slots are skipped.
	/// Without inner conditions it is always met: an empty condition does not restrict.
	/// </summary>
	[Serializable]
	[TypeSelectorName("Any of")]
	[TypeId("condition.any_of")]
	public class AnyOfCondition : ICondition
	{
		[SerializeReference, TypeSelector]
		public List<ICondition> Conditions = new List<ICondition>();

		public bool IsMet(object context)
		{
			if (Conditions == null)
				return true;

			bool hasConditions = false;

			foreach (ICondition condition in Conditions)
			{
				if (condition == null)
					continue;

				if (condition.IsMet(context))
					return true;

				hasConditions = true;
			}

			return !hasConditions;
		}
	}
}
