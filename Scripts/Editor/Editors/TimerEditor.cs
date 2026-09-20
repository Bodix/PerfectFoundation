// Perfect Core for Unity
// Copyright (c) 2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

using PerfectCore.PerfectFoundation.NaughtyAttributes.Editor;
using UnityEditor;

namespace PerfectCore.PerfectFoundation.Editor
{
	/// <summary>
	/// Draws <see cref="Timer"/> with NaughtyAttributes support.
	/// <para>
	/// The bundled inspector is registered project-wide, so this explicit registration is
	/// normally redundant. It exists so that Perfect Core's own components keep their
	/// attributes when the project-wide slot goes to another tool - either because
	/// PERFECTFOUNDATION_DISABLE_GLOBAL_INSPECTOR was defined, or because Odin Inspector or a similar
	/// extension won it.
	/// </para>
	/// </summary>
	[CanEditMultipleObjects]
	[CustomEditor(typeof(Timer))]
	public class TimerEditor : NaughtyInspector
	{
	}
}
