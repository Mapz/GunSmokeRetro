using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace XAsset.Editor {
    public static class BuildScript {
        [InitializeOnLoadMethod]
        public static void Clear () {
            UnityEditor.EditorUtility.ClearProgressBar ();
        }

        static public string CreateAssetBundleDirectory (string versionNum = null) {
            // Choose the output path according to the build target.
            string outputPath = Path.Combine (EditorUtility.AssetBundlesOutputPath, EditorUtility.GetPlatformName ());
            if (versionNum != null) {
                outputPath = Path.Combine (outputPath, versionNum);
            }
            if (!Directory.Exists (outputPath))
                Directory.CreateDirectory (outputPath);

            return outputPath;
        }

        public static AssetBundleManifest BuildAssetBundles (List<AssetBundleBuild> builds, string versionNum = null) {
            // Choose the output path according to the build target.
            string outputPath = CreateAssetBundleDirectory (versionNum);

            var options = BuildAssetBundleOptions.None;

            bool shouldCheckODR = EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS;
#if UNITY_TVOS
            shouldCheckODR |= EditorUserBuildSettings.activeBuildTarget == BuildTarget.tvOS;
#endif
            if (shouldCheckODR) {
#if ENABLE_IOS_ON_DEMAND_RESOURCES
                if (PlayerSettings.iOS.useOnDemandResources)
                    options |= BuildAssetBundleOptions.UncompressedAssetBundle;
#endif
#if ENABLE_IOS_APP_SLICING
                options |= BuildAssetBundleOptions.UncompressedAssetBundle;
#endif
            }

            options |= BuildAssetBundleOptions.AppendHashToAssetBundleName;

            if (builds == null || builds.Count == 0) {
                //@TODO: use append hash... (Make sure pipeline works correctly with it.)
                return BuildPipeline.BuildAssetBundles (outputPath, options, EditorUserBuildSettings.activeBuildTarget);
            } else {
                return BuildPipeline.BuildAssetBundles (outputPath, builds.ToArray (), options, EditorUserBuildSettings.activeBuildTarget);
            }
        }

        public static void BuildPlayerWithoutAssetBundles () {
            var outputPath = UnityEditor.EditorUtility.SaveFolderPanel ("Choose Location of the Built Game", "", "");
            if (outputPath.Length == 0)
                return;

            string[] levels = GetLevelsFromBuildSettings ();
            if (levels.Length == 0) {
                Debug.Log ("Nothing to build.");
                return;
            }

            string targetName = GetBuildTargetName (EditorUserBuildSettings.activeBuildTarget);
            if (targetName == null)
                return;

#if UNITY_5_4 || UNITY_5_3 || UNITY_5_2 || UNITY_5_1 || UNITY_5_0
            BuildOptions option = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
            BuildPipeline.BuildPlayer (levels, outputPath + targetName, EditorUserBuildSettings.activeBuildTarget, option);
#else
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions ();
            buildPlayerOptions.scenes = levels;
            buildPlayerOptions.locationPathName = outputPath + targetName;
            buildPlayerOptions.assetBundleManifestPath = GetAssetBundleManifestFilePath ();
            buildPlayerOptions.target = EditorUserBuildSettings.activeBuildTarget;
            buildPlayerOptions.options = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
            BuildPipeline.BuildPlayer (buildPlayerOptions);
#endif
        }

        public static void BuildStandalonePlayer () {
            var outputPath = UnityEditor.EditorUtility.SaveFolderPanel ("Choose Location of the Built Game", "", "");
            if (outputPath.Length == 0)
                return;

            string[] levels = GetLevelsFromBuildSettings ();
            if (levels.Length == 0) {
                Debug.Log ("Nothing to build.");
                return;
            }

            string targetName = GetBuildTargetName (EditorUserBuildSettings.activeBuildTarget);
            if (targetName == null)
                return;

            CopyAssetBundlesTo (Path.Combine (Application.streamingAssetsPath, EditorUtility.AssetBundlesOutputPath));
            AssetDatabase.Refresh ();

#if UNITY_5_4 || UNITY_5_3 || UNITY_5_2 || UNITY_5_1 || UNITY_5_0
            BuildOptions option = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
            BuildPipeline.BuildPlayer (levels, outputPath + targetName, EditorUserBuildSettings.activeBuildTarget, option);
#else
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions ();
            buildPlayerOptions.scenes = levels;
            buildPlayerOptions.locationPathName = outputPath + targetName;
            buildPlayerOptions.assetBundleManifestPath = GetAssetBundleManifestFilePath ();
            buildPlayerOptions.target = EditorUserBuildSettings.activeBuildTarget;
            buildPlayerOptions.options = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
            BuildPipeline.BuildPlayer (buildPlayerOptions);
#endif
        }

        public static string GetBuildTargetName (BuildTarget target) {
            string name = PlayerSettings.productName + "_" + PlayerSettings.bundleVersion;
            if (target == BuildTarget.Android) {
                return "/" + name + PlayerSettings.Android.bundleVersionCode + ".apk";
            }
            if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64) {
                return "/" + name + PlayerSettings.Android.bundleVersionCode + ".exe";
            }
            if (target == BuildTarget.StandaloneOSX) {
                return "/" + name + ".app";
            }
            if (target == BuildTarget.iOS) {
                return "/iOS";
            }
            Debug.Log ("Target not implemented.");
            return null;
            //if (target == BuildTarget.WebGL)
            //{
            //    return "/web";
            //}

        }

        static public void CopyAssetBundlesTo (string outputPath) {
            // Clear streaming assets folder.
            //            FileUtil.DeleteFileOrDirectory(Application.streamingAssetsPath);
            if (!Directory.Exists (outputPath)) {
                Directory.CreateDirectory (outputPath);
            }
            string outputFolder = EditorUtility.GetPlatformName ();

            // Setup the source folder for assetbundles.
            var source = Path.Combine (Path.Combine (System.Environment.CurrentDirectory, EditorUtility.AssetBundlesOutputPath), outputFolder);
            if (!System.IO.Directory.Exists (source))
                Debug.Log ("No assetBundle output folder, try to build the assetBundles first.");

            // Setup the destination folder for assetbundles.
            var destination = System.IO.Path.Combine (outputPath, outputFolder);
            if (System.IO.Directory.Exists (destination))
                FileUtil.DeleteFileOrDirectory (destination);

            FileUtil.CopyFileOrDirectory (source, destination);
        }

        static string[] GetLevelsFromBuildSettings () {
            List<string> levels = new List<string> ();
            for (int i = 0; i < EditorBuildSettings.scenes.Length; ++i) {
                if (EditorBuildSettings.scenes[i].enabled)
                    levels.Add (EditorBuildSettings.scenes[i].path);
            }

            return levels.ToArray ();
        }

        static string GetAssetBundleManifestFilePath () {
            var relativeAssetBundlesOutputPathForPlatform = Path.Combine (EditorUtility.AssetBundlesOutputPath, EditorUtility.GetPlatformName ());
            return Path.Combine (relativeAssetBundlesOutputPathForPlatform, EditorUtility.GetPlatformName ()) + ".manifest";
        }

        static void SaveVersionFile (string path, string versionNum) {
            path = path + "/version.txt";
            if (File.Exists (path)) {
                File.Delete (path);
            }
            using (var writer = new StreamWriter (path)) {
                //第一行是版本号
                writer.Write (versionNum);
                writer.Flush ();
                writer.Close ();
            }
        }

        static void SaveManifest (string path, List<AssetBundleBuild> builds, string versionNum) {
            if (File.Exists (path)) {
                File.Delete (path);
            }
            using (var writer = new StreamWriter (path)) {
                //第一行是版本号
                writer.WriteLine (versionNum);

                foreach (var item in builds) {
                    writer.WriteLine (item.assetBundleName + ":");
                    foreach (var asset in item.assetNames) {
                        writer.WriteLine (string.Format ("\t{0}", asset));
                    }
                    writer.WriteLine ();
                }
                writer.Flush ();
                writer.Close ();
            }
        }

        public static void BuildManifest (string path, List<AssetBundleBuild> builds, AssetBundleManifest assetBundleManifest, bool forceRebuild, string versionNum) {

            Manifest manifest = new Manifest ();

            string[] RealABPathList = assetBundleManifest.GetAllAssetBundles ();
            Dictionary<string, string> abPathList = new Dictionary<string, string> ();

            for (int i = 0; i < RealABPathList.Length; i++) {

                string[] result = RealABPathList[i].Split ('_');
                Debug.Log ("========");
                string _path = "";
                string _hash = "";
                if (result.Length > 1) {
                    for (int j = 0; j < result.Length; j++) {
                        if (j == 0) {
                            _path = result[j];
                        } else if (j > 0 && j < result.Length - 1) {
                            _path = _path + "_" + result[j];
                        } else if (j == result.Length - 1) {
                            _hash = result[j];
                        }
                    }
                    abPathList.Add (_path, _hash);
                }

            }

            if (File.Exists (path)) {
                using (var reader = new StreamReader (path)) {
                    manifest.Load (reader);
                    reader.Close ();
                }
            }

            Dictionary<string, string> newpaths = new Dictionary<string, string> ();
            List<string> bundles = new List<string> ();
            List<string> assets = new List<string> ();
            bool dirty = false;
            if (builds.Count > 0) {
                for (int i = 0; i < builds.Count; i++) {
                    var item = builds[i];
                    string assetBundleHash;
                    bool gotBundleName = abPathList.TryGetValue (item.assetBundleName, out assetBundleHash);

                    if (gotBundleName) {
                        item.assetBundleName = item.assetBundleName + "_" + assetBundleHash;
                    }
                    builds[i] = item;
                    bundles.Add (item.assetBundleName);
                    foreach (var assetPath in item.assetNames) {
                        newpaths[assetPath] = item.assetBundleName;
                        assets.Add (assetPath + ":" + (bundles.Count - 1));
                    }
                }
            }

            if (manifest.allAssets != null && newpaths.Count == manifest.allAssets.Length) {
                foreach (var item in newpaths) {
                    if (!manifest.ContainsAsset (item.Key) || !manifest.GetBundleName (item.Key).Equals (newpaths[item.Key])) {
                        dirty = true;
                        break;
                    }
                }
            } else {
                dirty = true;
            }

            if (forceRebuild || dirty || !File.Exists (path)) {
                string versionFilePath = Path.Combine (EditorUtility.AssetBundlesOutputPath, EditorUtility.GetPlatformName ());
                SaveVersionFile (versionFilePath, versionNum);
                SaveManifest (path, builds, versionNum);
                AssetDatabase.ImportAsset (path, ImportAssetOptions.ForceUpdate);
                AssetDatabase.Refresh ();
            }

            Debug.Log ("[BuildScript] BuildManifest with " + assets.Count + " assets and " + bundles.Count + " bundels.");
        }
    }
}