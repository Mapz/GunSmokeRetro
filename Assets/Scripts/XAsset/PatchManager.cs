using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace XAsset {

    public class PatchManager : MonoBehaviour {
        public static PatchManager instance;
        public string patchUrlRoot = "http://192.168.52.174:8082/";
        private string patchUrl;

        // 远端本版本目录root
        static string resUrlRoot;

        private string versionUrl;

        private string localVersionFile;
        public string localBundlePath;

        const int FILE_EXIST = 1;
        const int FILE_NOT_EXIST = -1;
        const int FILE_STATUS_UNKOWN = 0;

        public string curVersionNum;
        public string lastVersionNum;

        private static Dictionary<string, int> filesStatus = new Dictionary<string, int> ();

        private string localManifestPath;
        private string _localXManifestPath;
        public string localXManifestPath {
            get { return _localXManifestPath; }
        }

        private static int downlodingCount = 0;
        private event DownloadManager.DownloadFinishDelegate curDownloadAllOverCallback;
        public event DownloadManager.DownloadFinishDelegate onXAssetManifestDownloadOver;

        public static bool Initialize () {
            if (instance == null) {
                var go = new GameObject ("PatchManager");
                DontDestroyOnLoad (go);
                instance = go.AddComponent<PatchManager> ();

            }
            return true;
        }

        void Awake () {
            versionUrl = patchUrlRoot + "version?platform=" + Utility.GetPlatformName ();
            patchUrl = Path.Combine (patchUrlRoot, "static");
            localVersionFile = Application.persistentDataPath + "/version.txt";
            localBundlePath = Utility.AssetBunldesDownloadPath;
        }

        void Start () { }
        //获取远程版本号
        public void StartUpdateVersionNum () {
#if UNITY_EDITOR
            if (Utility.ActiveBundleMode) {

                GetLocalVersion ();
                StartCoroutine (getRemoteVersion ());
            } else {
                onXAssetManifestDownloadOver ();
            }
#else

            GetLocalVersion ();
            StartCoroutine (getRemoteVersion ());
#endif
        }
        void OnGetRemoteVersion (bool succeed, string versionNum = null) {
            Debug.Log ("读取版本号：" + succeed);
            // 获取了远程版本号
            if (null != versionNum) {
                Debug.Log ("version:" + versionNum);
                if (curVersionNum != versionNum) {
                    lastVersionNum = curVersionNum;
                    curVersionNum = versionNum;
                    FileTool.CreatFile (localVersionFile, Encoding.ASCII.GetBytes (curVersionNum));
                }
                // 下载新的manifest文件
                localManifestPath = Path.Combine (localBundlePath, curVersionNum);
                _localXManifestPath = localBundlePath + "/" + curVersionNum + ".xManifest";
                resUrlRoot = patchUrl + "/" + Utility.GetPlatformName () + "/" + curVersionNum;
                if (!File.Exists (localManifestPath)) {
                    string manifestUrl = patchUrl + "/" + Utility.GetPlatformName () + "/" + curVersionNum + "/" + curVersionNum;
                    Debug.Log (localBundlePath);
                    DownloadManager dm = DownloadManager.instance;
                    dm.OnError += OnDownloadErr;
                    dm.OnFinish += OnAllDownloadOver;
                    curDownloadAllOverCallback = ManifestDownloadFinish;
                    dm.OnFinish += curDownloadAllOverCallback;
                    dm.Push (new DownloadManager.Request (manifestUrl, OnFileDownloadOver, localManifestPath));
                    dm.Excute ();
                } else {
                    //进入下一步
                    ManifestDownloadFinish ();
                }

            } else {
                //未获取远程版本号
                Debug.LogError ("获取远程版本号失败");
            }

        }

        // 获取当前版本号
        void GetLocalVersion () {
            if (File.Exists (localVersionFile)) {
                curVersionNum = FileTool.ReadFile (localVersionFile, 1);
            }
        }

        // 获取服务器版本号
        private IEnumerator getRemoteVersion () {
            using (UnityWebRequest www = UnityWebRequest.Get (versionUrl)) {
                yield return www.SendWebRequest ();
                if (www.error != null) {
                    Debug.Log (www.error);
                    OnGetRemoteVersion (false);
                } else {
                    if (www.responseCode == 200) //200表示接受成功
                    {
                        OnGetRemoteVersion (true, www.downloadHandler.text);
                    } else {
                        OnGetRemoteVersion (false);
                    }
                }
            }
        }

        // Manifest下载完成
        void ManifestDownloadFinish () {
            Debug.Log ("ManifestDownloadFinish！！！");
            ProcessManifest ();
        }

        // 解析 Manifest，检查文件是否存在
        void ProcessManifest () {
            AssetBundle assetBundle = AssetBundle.LoadFromFile (localManifestPath);
            AssetBundleManifest manifest = assetBundle.LoadAsset<AssetBundleManifest> ("assetbundlemanifest");
            string[] bundles = manifest.GetAllAssetBundles ();
            assetBundle.Unload (true);
            for (int i = 0; i < bundles.Length; i++) {
                string _path = bundles[i];
                filesStatus.Add (_path, FILE_STATUS_UNKOWN);
                if (File.Exists (Path.Combine (localBundlePath, _path))) {
                    filesStatus[_path] = FILE_EXIST;
                }
            }
            // 下载XAssetManifest
            if (!File.Exists (_localXManifestPath)) {
                string xManifestUrl = patchUrl + "/" + Utility.GetPlatformName () + "/" + curVersionNum + "/" + curVersionNum + ".xManifest";
                DownloadManager dm = DownloadManager.instance;

                dm.OnError += OnDownloadErr;
                dm.OnFinish += OnAllDownloadOver;

                curDownloadAllOverCallback = onXAssetManifestDownloadOver;
                dm.OnFinish += curDownloadAllOverCallback;

                dm.Push (new DownloadManager.Request (xManifestUrl, OnFileDownloadOver, _localXManifestPath));

                dm.Excute ();

            } else {
                onXAssetManifestDownloadOver ();
            }
        }

        // 检查并下载网络资源 
        public void CheckAndDownloadResource (List<string> assetPath, DownloadManager.DownloadFinishDelegate downloadAllOverCallBack) {
#if UNITY_EDITOR
            if (!Utility.ActiveBundleMode) {
                OnAllDownloadOver ();
                if (downloadAllOverCallBack != null) {
                    downloadAllOverCallBack ();
                }
                return;
            }
#endif
            if (downlodingCount != 0) {
                Debug.LogError ("已经有下载队列");
                return;
            }
            // 检查所有
            HashSet<string> toDownload = new HashSet<string> ();
            foreach (string _path in assetPath) {
                Debug.Log ("_path:" +
                    _path);
                string realPath = Assets.GetBundleName (_path);
                var allDependencies = Bundles.GetAllDependencies (realPath);
                HashSet<string> pathIncludeDependences = new HashSet<string> ();
                pathIncludeDependences.Add (realPath);
                foreach (string dependence in allDependencies) {
                    pathIncludeDependences.Add (dependence);
                }
                foreach (string __path in pathIncludeDependences) {
                    int _fileStatus;
                    if (!filesStatus.TryGetValue (__path, out _fileStatus)) {
                        filesStatus.Add (__path, FILE_STATUS_UNKOWN);
                        toDownload.Add (__path);
                    } else if (_fileStatus == FILE_STATUS_UNKOWN || _fileStatus == FILE_NOT_EXIST) {
                        toDownload.Add (__path);
                    }
                }

            }
            if (toDownload.Count == 0) {
                OnAllDownloadOver ();
                if (downloadAllOverCallBack != null) {
                    downloadAllOverCallBack ();
                }
            } else {

                DownloadManager dm = DownloadManager.instance;
                dm.OnError += OnDownloadErr;
                dm.OnFinish += OnAllDownloadOver;
                if (downloadAllOverCallBack != null) {
                    curDownloadAllOverCallback = downloadAllOverCallBack;
                    dm.OnFinish += curDownloadAllOverCallback;
                }
                foreach (string realPath in toDownload) {
                    string _path = Path.Combine (resUrlRoot, realPath);
                    Debug.Log ("下载文件：" + _path);
                    dm.Push (new DownloadManager.Request (_path, OnFileDownloadOver, realPath));
                }
                dm.Excute ();
            }
        }

        void OnDownloadErr (string err) {
            Debug.LogError ("下载错误");
            DownloadManager.instance.Dispose ();
        }

        void OnAllDownloadOver () {
            Debug.Log ("全部下载完成");
            DownloadManager.instance.Dispose ();
        }

        void OnFileDownloadOver (UnityWebRequest www, object msg) {
            byte[] bytes = www.downloadHandler.data;
            string filePath = Path.Combine (localBundlePath, msg.ToString ());
            //创建文件
            Debug.Log ("单个下载完成：" + filePath);
            FileTool.CreateDirectory (filePath);
            FileTool.CreatFile (filePath, bytes);
            filesStatus[msg.ToString ()] = FILE_STATUS_UNKOWN;
        }

    }
}