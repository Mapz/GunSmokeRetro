using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public partial class Utility {
    public const string AssetBundlesOutputPath = "AssetBundles";
    public const string AssetBundlesOutputPathForDownload = "AssetBundlsForDownload";
    public const string UnitPrefabPath = "Assets/AssetsToBuild/Units/";
    public const string FontPath = "Assets/AssetsToBuild/Font/";
    public const string OtherSpritesPath = "Assets/AssetsToBuild/OtherSprites/";
    public const string UIPath = "Assets/AssetsToBuild/UI/";
    public const string ShadersPath = "Assets/AssetsToBuild/Shaders/";
    public const string WantedPath = "Assets/AssetsToBuild/Wanted/";
    public const string LevelLoopsPrefabPath = "Assets/AssetsToBuild/LevelLoops/";
    public static string AssetBunldesDownloadPath = Application.persistentDataPath + "/Bundles";

    public static int activeBundleMode = -1;
    const string kActiveBundleMode = "ActiveBundleMode";
    public static bool ActiveBundleMode {
        get {
            if (activeBundleMode == -1)
                activeBundleMode = PlayerPrefs.GetInt (kActiveBundleMode, 0);
            return activeBundleMode != 0;
        }
        set {
            int newValue = value ? 1 : 0;
            if (newValue != activeBundleMode) {
                activeBundleMode = newValue;
                PlayerPrefs.SetInt (kActiveBundleMode, newValue);
            }
        }
    }

    public static int activeDownloadMode = -1;
    const string kActiveDownloadMode = "ActiveDownloadMode";
    public static bool ActiveDownloadMode {
        get {
            if (activeDownloadMode == -1)

                activeDownloadMode = PlayerPrefs.GetInt (kActiveDownloadMode, 0);
            return activeDownloadMode != 0;
        }
        set {
            int newValue = value ? 1 : 0;
            if (newValue != activeDownloadMode) {
                activeDownloadMode = newValue;
                PlayerPrefs.SetInt (kActiveDownloadMode, newValue);
            }
        }
    }
}