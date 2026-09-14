using UnityEngine;

namespace PerfectCore.NaughtyAttributes.Test
{
	public class ShowNativePropertyTest : MonoBehaviour
	{
		[ShowNativeProperty]
		private Transform Transform
		{
			get
			{
				return transform;
			}
		}

		[ShowNativeProperty]
		private Transform ParentTransform
		{
			get
			{
				return transform.parent;
			}
		}
	}
}
