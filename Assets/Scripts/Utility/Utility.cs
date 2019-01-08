using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public partial class Utility {

    public static string GetPlatformName () {
#if UNITY_EDITOR
        return GetPlatformForAssetBundles (EditorUserBuildSettings.activeBuildTarget);
#else
        return GetPlatformForAssetBundles (Application.platform);
#endif
    }

    public static string GetPlatformForAssetBundles (RuntimePlatform platform) {
        if (platform == RuntimePlatform.Android) {
            return "Android";
        }
        if (platform == RuntimePlatform.IPhonePlayer) {
            return "iOS";
        }
        if (platform == RuntimePlatform.tvOS) {
            return "tvOS";
        }
        if (platform == RuntimePlatform.WebGLPlayer) {
            return "WebGL";
        }
        if (platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.WindowsEditor) {
            return "Windows";
        }
        if (platform == RuntimePlatform.OSXPlayer || platform == RuntimePlatform.OSXEditor) {
            return "OSX";
        }
        return null;
    }

#if UNITY_EDITOR
    static string GetPlatformForAssetBundles (BuildTarget target) {
        if (target == BuildTarget.Android) {
            return "Android";
        }
        if (target == BuildTarget.tvOS) {
            return "tvOS";
        }
        if (target == BuildTarget.iOS) {
            return "iOS";
        }
        if (target == BuildTarget.WebGL) {
            return "WebGL";
        }
        if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64) {
            return "Windows";
        }
        if (target == BuildTarget.StandaloneOSX) {
            return "OSX";
        }
        // Add more build targets for your own.
        // If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
        return null;
    }
#endif  
    public static string GetRegularPath (string path) {
        if (path == null) {
            return null;
        }

        return path.Replace ('\\', '/');
    }
    public static string GetCombinePath (params string[] path) {
        if (path == null || path.Length < 1) {
            return null;
        }

        string combinePath = path[0];
        for (int i = 1; i < path.Length; i++) {
            combinePath = System.IO.Path.Combine (combinePath, path[i]);
        }

        return GetRegularPath (combinePath);
    }
}