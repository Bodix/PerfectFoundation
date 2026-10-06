// Perfect Foundation for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PerfectCore.PerfectFoundation.Editor
{
	/// <summary>
	/// Finds data assets without an ID and data assets that share an ID.
	/// Call it from an edit mode test or use the "Tools/Perfect Foundation/Validate Data Assets" menu.
	/// </summary>
	public static class DataAssetValidator
	{
		[MenuItem("Tools/Perfect Foundation/Validate Data Assets")]
		private static void ValidateProject()
		{
			List<string> problems = FindProblems("Assets");

			if (problems.Count == 0)
				Debug.Log("[Perfect Foundation] All data assets have unique IDs.");
			else
				Debug.LogError("[Perfect Foundation] Data asset problems:\n" + string.Join("\n", problems));
		}

		/// <summary>
		/// Returns readable problems: a missing ID or an ID shared by several assets. Empty when all IDs are valid.
		/// </summary>
		/// <param name="folders">Folders to search, for example "Assets/Game". Empty — the whole project.</param>
		public static List<string> FindProblems(params string[] folders)
		{
			List<DataAsset> assets = FindDataAssets(folders);
			List<string> problems = new List<string>();

			foreach (DataAsset asset in assets)
			{
				if (string.IsNullOrWhiteSpace(asset.Id))
					problems.Add($"No ID: {AssetDatabase.GetAssetPath(asset)}");
			}

			IEnumerable<IGrouping<string, DataAsset>> duplicates = assets
				.Where(asset => !string.IsNullOrWhiteSpace(asset.Id))
				.GroupBy(asset => asset.Id)
				.Where(group => group.Count() > 1);

			foreach (IGrouping<string, DataAsset> group in duplicates)
				problems.Add($"Duplicate ID '{group.Key}': {string.Join(", ", group.Select(AssetDatabase.GetAssetPath))}");

			return problems;
		}

		/// <summary>
		/// Loads every data asset in the folders, sub-assets included.
		/// </summary>
		/// <param name="folders">Folders to search. Empty — the whole project.</param>
		public static List<DataAsset> FindDataAssets(params string[] folders)
		{
			string[] guids = folders == null || folders.Length == 0
				? AssetDatabase.FindAssets("t:ScriptableObject")
				: AssetDatabase.FindAssets("t:ScriptableObject", folders);

			return guids
				.Select(AssetDatabase.GUIDToAssetPath)
				.Distinct()
				.SelectMany(AssetDatabase.LoadAllAssetsAtPath)
				.OfType<DataAsset>()
				.ToList();
		}
	}
}
