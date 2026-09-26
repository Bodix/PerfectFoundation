using System;

namespace PerfectCore.PerfectFoundation.NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShowNonSerializedFieldAttribute : SpecialCaseDrawerAttribute
    {
    }
}
