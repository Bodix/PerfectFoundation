// Perfect Core for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

using System;
using UnityEngine;

namespace PerfectCore.PerfectFoundation
{
	[Serializable]
	public struct TransformData
	{
		public Vector3 Position;
		public Quaternion Rotation;
		public Vector3 LocalScale;

		public TransformData(Vector3 position, Quaternion rotation, Vector3 localScale)
		{
			Position = position;
			Rotation = rotation;
			LocalScale = localScale;
		}

		public static TransformData Default => new TransformData(Vector3.zero, Quaternion.identity, Vector3.one);
	}

	public static class TransformExtensions
	{
		public static TransformData GetData(this Transform transform)
		{
			return new TransformData(
				transform.position,
				transform.rotation,
				transform.localScale);
		}

		public static void SetData(this Transform transform, TransformData data)
		{
			transform.position = data.Position;
			transform.rotation = data.Rotation;
			transform.localScale = data.LocalScale;
		}
	}
}