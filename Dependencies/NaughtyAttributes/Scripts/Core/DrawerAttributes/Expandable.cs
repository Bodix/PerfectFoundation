using System;

namespace PerfectCore.NaughtyAttributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class ExpandableAttribute : DrawerAttribute
	{
	}
}
