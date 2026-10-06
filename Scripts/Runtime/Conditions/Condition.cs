// Perfect Foundation for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

using System;

namespace PerfectCore.PerfectFoundation
{
	/// <summary>
	/// Base for a condition that needs a context of a known type.
	/// Through <see cref="ICondition"/> it is not met when the context is null or of another type.
	/// </summary>
	[Serializable]
	public abstract class Condition<TContext> : ICondition
	{
		bool ICondition.IsMet(object context)
		{
			return context is TContext typedContext && IsMet(typedContext);
		}

		public abstract bool IsMet(TContext context);
	}
}
