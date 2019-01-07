using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/**
	DownloadManager
	@author koki ibukuro
*/
public class DownloadManager : MonoBehaviour, System.IDisposable {
    // using classes
    public class Request {
        public string url;
        public DownloadDelegate del;

        public WWWForm form;

        public byte[] bytes;
        public Hashtable header;

        public object message;

        //-------------------------------------------------------------
        // Constractors
        //-------------------------------------------------------------
        public Request (string url, DownloadDelegate del) {
            this.url = url;
            this.del = del;
        }
        public Request (string url, DownloadDelegate del, object msg) : this (url, del) {
            this.message = msg;
        }

        public Request (string url, DownloadDelegate del, object msg, WWWForm form) : this (url, del, msg) {
            this.form = form;
        }

        public Request (string url, DownloadDelegate del, object msg, byte[] bytes) : this (url, del, msg) {
            this.bytes = bytes;
        }

        public Request (string url, DownloadDelegate del, object msg, byte[] bytes, Hashtable header) : this (url, del, msg, bytes) {
            this.header = header;
        }

        //
        public UnityWebRequest MakeWWW () {
            if (header != null) {
                UnityWebRequest wr = UnityWebRequest.Get (url);
                foreach (var i in header.Keys) {
                    wr.SetRequestHeader (i.ToString (), header[i].ToString ());
                }
                return wr;
            }
            if (bytes != null) {
                return UnityWebRequest.Post (url, null, bytes);
            }
            if (form != null) {
                return UnityWebRequest.Post (url, form);
            }
            return UnityWebRequest.Get (url);
        }
    }

    class TimeOut {
        float beforProgress;
        float beforTime;

        public bool CheckTimeout (float progress) {
            float now = Time.time;
            if ((now - beforTime) > TIME_OUT) {
                // timeout
                return true;
            }
            // update progress
            if (beforProgress != progress) {
                beforProgress = progress;
                beforTime = now;
            }
            return false;
        }
    }

    // delegates
    public delegate void DownloadDelegate (UnityWebRequest www, object msg = null);
    public delegate void DownloadErrorDelegate (string error);
    public delegate void DownloadFinishDelegate ();

    // const
    public const float TIME_OUT = 20f;

    // public 
    public event DownloadErrorDelegate OnError;
    public event DownloadFinishDelegate OnFinish;

    // member
    Queue<Request> requests;
    UnityWebRequest _www;
    bool isDownloading = false;
    TimeOut timeout;

    // static
    private static DownloadManager _instance;
    static public DownloadManager instance {
        get {
            if (_instance == null) {
                GameObject go = new GameObject ();
                go.name = "DownloadManager";
                _instance = go.AddComponent<DownloadManager> ();
                DontDestroyOnLoad (go);
            }
            return _instance;
        }
    }

    //-------------------------------------------
    // life cycle
    //-------------------------------------------
    void Awake () {
        requests = new Queue<Request> ();
        timeout = new TimeOut ();
    }

    void Destroy () {
        this.Dispose ();
        _instance = null;
    }

    public void Dispose () {
        isDownloading = false;
        StopAllCoroutines ();
        if (_www != null) {
            _www.Dispose ();
        }
        requests.Clear ();
        OnError = null;
        OnFinish = null;
    }

    void FixedUpdate () {
        if (!isDownloading) {
            this.enabled = false;
        }

        if (timeout.CheckTimeout (CurrentProgress)) {
            // timeout
            if (OnError != null) OnError ("timeout");
            this.Dispose ();
        }
    }

    //-------------------------------------------
    // public
    //-------------------------------------------
    public void Push (Request req) {
        requests.Enqueue (req);
    }

    public void Excute () {
        if (isDownloading) {
            Debug.Log ("aleady downloading...");
            return;
        }
        StartCoroutine (Download ());
    }

    public float CurrentProgress {
        get {
            if (_www == null) { return 0f; }
            return _www.downloadProgress;
        }
    }

    //-------------------------------------------
    // private
    //-------------------------------------------
    IEnumerator Download () {
        if (requests.Count == 0) {
            Debug.LogWarning ("no requests");
            yield return true;
        }
        this.isDownloading = true;
        this.enabled = true;

        while (requests.Count > 0) {
            Request req = requests.Dequeue ();
            _www = req.MakeWWW ();
            _www.timeout = 30; //设置超时，若m_webRequest.SendWebRequest()连接超时会返回，且isNetworkError为true
            yield return _www.SendWebRequest ();
            // error check
            if (_www.isNetworkError) {
                if (OnError != null) OnError (_www.error);
            } else if (_www.responseCode != 200) {
                if (OnError != null) OnError (_www.responseCode.ToString ());
            } else {
                req.del (_www, req.message);
            }
        }

        if (OnFinish != null) OnFinish ();
        
        this.isDownloading = false;
        this.enabled = false;

    }
}