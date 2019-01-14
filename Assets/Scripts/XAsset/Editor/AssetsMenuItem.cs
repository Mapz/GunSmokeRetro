using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace XAsset.Editor {
	public static class EditorConst {
		public const string LuaScriptAssetLabel = "LuaScript";
		public const string LuaAssetExtInBundle = ".bytes";

		public static string lua_file_map_path = "Assets/AssetsToBuild/LuaScripts/LoadTable.txt";
	}

	public static class AssetsMenuItem {
		[MenuItem ("CodeZF/Copy Asset Path")]
		static void CopyAssetPath () {
			if (EditorApplication.isCompiling) {
				return;
			}
			string path = AssetDatabase.GetAssetPath (Selection.activeInstanceID);
			GUIUtility.systemCopyBuffer = path;
			Debug.Log (string.Format ("systemCopyBuffer: {0}", path));
		}

		const string kRuntimeMode = "CodeZF/XAsset/Bundle Mode";

		[MenuItem (kRuntimeMode)]
		public static void ToggleRuntimeMode () {
			EditorUtility.ActiveBundleMode = !EditorUtility.ActiveBundleMode;
		}

		[MenuItem (kRuntimeMode, true)]
		public static bool ToggleRuntimeModeValidate () {
			Menu.SetChecked (kRuntimeMode, EditorUtility.ActiveBundleMode);
			return true;
		}

		const string kResourceMode = "CodeZF/XAsset/Download Mode";

		[MenuItem (kResourceMode)]
		public static void ToggleResourceMode () {
			EditorUtility.ActiveDownloadMode = !EditorUtility.ActiveDownloadMode;
		}

		[MenuItem (kResourceMode, true)]
		public static bool ToggleResourceModeValidate () {
			if (!EditorUtility.ActiveBundleMode) {
				return false;
			}
			Menu.SetChecked (kResourceMode, EditorUtility.ActiveDownloadMode);
			return true;
		}

		// [MenuItem ("CodeZF/Ensure Asset Labels")]
		// public static void EnsureLuaAssetLabels () {
		// 	var luaPaths = new List<string> (AssetDatabase.GetAllAssetPaths ()).Where (path => path.EndsWith (".lua"));
		// 	foreach (var path in luaPaths) {
		// 		var obj = AssetDatabase.LoadAssetAtPath (path, typeof (UnityEngine.Object));
		// 		var curLabels = AssetDatabase.GetLabels (obj);
		// 		AssetDatabase.SetLabels (obj, new HashSet<string> (curLabels).Union (new string[] { EditorConst.LuaScriptAssetLabel }).ToArray ());
		// 	}
		// }

		[MenuItem ("CodeZF/XAsset/Build AssetBundles For Download")]
		public static void BuildAssetBundles () {
			if (EditorApplication.isCompiling) {
				return;
			}
			string versionNum = DateTime.Now.ToFileTime ().ToString ();
			string maninfestPath = Path.Combine (BuildScript.CreateAssetBundleDirectory (versionNum), versionNum + ".xManifest");
			// var luaPaths = AssetDatabase.FindAssets ("l:" + EditorConst.LuaScriptAssetLabel).ToList ().ConvertAll (guid => AssetDatabase.GUIDToAssetPath (guid));
			// EnsureLuaAssetLabels (); //ForLua
			// GenerateLuaLoadList (); //ForLua
			// var pathMap = ChangeFileNames (luaPaths); //ForLua
			try {
				List<AssetBundleBuild> builds = BuildRule.GetBuilds (maninfestPath);
				AssetBundleManifest assetBundleManifest = BuildScript.BuildAssetBundles (builds, versionNum);
				BuildScript.BuildManifest (maninfestPath, builds, assetBundleManifest, false, versionNum);
			} finally {
				// RevertFileNames (pathMap); //ForLua
			}

		}

		const string assetsManifesttxt = "Assets/Manifest.txt";
		[MenuItem ("CodeZF/XAsset/Build Manifest")]
		public static void BuildAssetManifest () {
			if (EditorApplication.isCompiling) {
				return;
			}
			List<AssetBundleBuild> builds = BuildRule.GetBuilds (assetsManifesttxt);
			BuildScript.BuildManifest (assetsManifesttxt, builds);
		}

		[MenuItem ("CodeZF/XAsset/Build AssetBundles For Stream")]
		public static void BuildAssetBundlesForStream () {
			if (EditorApplication.isCompiling) {
				return;
			}
			if (EditorApplication.isCompiling) {
				return;
			}
			List<AssetBundleBuild> builds = BuildRule.GetBuilds (assetsManifesttxt);
			BuildScript.BuildManifest (assetsManifesttxt, builds);
			BuildScript.BuildAssetBundles (builds);
		}

		private static IDictionary<string, string> ChangeFileNames (IEnumerable<string> luaPaths) {
			var pathMap = new Dictionary<string, string> ();
			foreach (string path in luaPaths) {
				var newPath = path + EditorConst.LuaAssetExtInBundle;
				File.Move (path, newPath);
				pathMap.Add (newPath, path);
				File.Move (path + ".meta", newPath + ".meta");
				pathMap.Add (newPath + ".meta", path + ".meta");
			}

			AssetDatabase.Refresh ();
			return pathMap;
		}
		private static void RevertFileNames (IDictionary<string, string> pathMap) {
			foreach (var kv in pathMap) {
				File.Move (kv.Key, kv.Value);
			}

			AssetDatabase.Refresh ();
		}

		// [MenuItem ("CodeZF/Generate Lua LoadList")]
		// public static void GenerateLuaLoadList () {
		// 	var luaPaths = AssetDatabase.GetAllAssetPaths ().Where (path => path.EndsWith (".lua"));
		// 	string loadLuaTablePath = EditorConst.lua_file_map_path;
		// 	if (File.Exists (loadLuaTablePath)) {
		// 		File.Delete (loadLuaTablePath);
		// 	}
		// 	using (var writer = new StreamWriter (loadLuaTablePath)) {
		// 		foreach (var path in luaPaths) {

		// 			if (path.Contains (LuaConst._luaDir)) {
		// 				writer.WriteLine ("Assets" + LuaConst._luaDir + "\t" + path.Substring (path.IndexOf (LuaConst._luaDir) + LuaConst._luaDir.Length));
		// 			} else if (path.Contains (LuaConst._toluaDir)) {
		// 				writer.WriteLine ("Assets" + LuaConst._toluaDir + "\t" + path.Substring (path.IndexOf (LuaConst._toluaDir) + LuaConst._toluaDir.Length));
		// 			}

		// 		}
		// 		writer.Flush ();
		// 		writer.Close ();
		// 	}

		// 	Debug.Log ("Lua LoadList Generate Over");
		// }

		[MenuItem ("CodeZF/XAsset/Build Player")]
		public static void BuildPlayer () {
			if (EditorApplication.isCompiling) {
				return;
			}
			BuildScript.BuildStandalonePlayer ();
		}
	}
}