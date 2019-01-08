using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public partial class Utility {
    public const string AssetBundlesOutputPath = "AssetBundles";
    public const string UnitPrefabPath = "Assets/AssetsToBuild/Units/";
    public const string LevelLoopsPrefabPath = "Assets/AssetsToBuild/LevelLoops/";
    public static string AssetBunldesDownloadPath = Application.persistentDataPath + "/Bundles";

#if UNITY_EDITOR 
    public static int activeBundleMode = -1;
    const string kActiveBundleMode = "ActiveBundleMode";

    public static bool ActiveBundleMode {
        get {
            if (activeBundleMode == -1)
                activeBundleMode = EditorPrefs.GetBool (kActiveBundleMode, true) ? 1 : 0;
            return activeBundleMode != 0;
        }
        set {
            int newValue = value ? 1 : 0;
            if (newValue != activeBundleMode) {
                activeBundleMode = newValue;
                EditorPrefs.SetBool (kActiveBundleMode, value);
            }
        }
    }
#endif
}