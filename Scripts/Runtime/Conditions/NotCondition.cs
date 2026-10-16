// Perfect Foundation for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

using System;
using UnityEngine;

namespace PerfectCore.PerfectFoundation
{
	/// <summary>
	/// Met when the inner condition is not met.
	/// Without an inner condition it is always met: an empty condition does not restrict.
	/// </summary>
	[Serializable]
	[TypeSelectorName("Not")]
	[TypeId("condition.not")]
	public class NotCondition : ICondition
	{
		[SerializeReference, TypeSelector]
		public ICondition Condition;

		public bool IsMet(object context)
		{
			return Condition == null || !Condition.IsMet(context);
		}
	}
}
