using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMgr<T> where T : MonoBehaviour, PauseAble {
    public delegate T GetObj ();
    public static ObjectMgr<T> _Instance;

    private List<T> m_pool = new List<T> ();
    public static ObjectMgr<T> Instance {
        get {
            if (null != _Instance) return _Instance;
            _Instance = new ObjectMgr<T> ();
            return _Instance;
        }
    }

    public T Create (GetObj action) {
        T newObj = action ();
        _Instance.m_pool.Add (newObj);
        return newObj;

    }

    public void Destroy (T bullet) {
        _Instance.m_pool.Remove (bullet);
        GameObject.Destroy (bullet.gameObject);
    }

    public void PauseAll (bool _pause) {
        foreach (T obj in _Instance.m_pool) {
            if (null == obj) continue;
            obj.Pause (_pause);
        }
    }

    public void Clear () {
        foreach (T obj in _Instance.m_pool) {
            if (null != obj)
                GameObject.Destroy (obj.gameObject);
        }
        _Instance.m_pool.Clear ();
    }
}