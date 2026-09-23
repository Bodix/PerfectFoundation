// Perfect Foundation for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

using UnityEngine;

namespace PerfectCore.PerfectFoundation
{
	[AddComponentMenu("Perfect Core/Comment")]
	public class Comment : MonoBehaviour
	{
#if UNITY_EDITOR
		public string Message;
		public CommentType Type = CommentType.Info;

		public enum CommentType
		{
			Info,
			Warning
		}
#endif
	}
}