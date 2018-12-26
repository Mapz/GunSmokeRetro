using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate BulletBehavior GetBullet ();
public class BulletMgr : MonoBehaviour {

    public static BulletMgr Instance;

    private List<BulletBehavior> m_pool = new List<BulletBehavior> ();

    private void Awake () {
        Instance = this;
    }

    public static BulletMgr Init () {
        if (null != Instance) return Instance;
        new GameObject ("BulletMgr").AddComponent<BulletMgr> ();
        return Instance;
    }

    public static BulletBehavior CreateBullet (GetBullet action) {
        BulletBehavior newBullet = action ();
        Instance.m_pool.Add (newBullet);
        return newBullet;

    }

    public static void DestroyBullet (BulletBehavior bullet) {
        Instance.m_pool.Remove (bullet);
        Destroy (bullet.gameObject);
    }

    public static void PauseAll () {
        foreach (BulletBehavior u in Instance.m_pool) {
            if (null == u) continue;
            if (null != u.GetComponent<Rigidbody2D> ()) {
                u.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
            }
        }
    }

    public static void Clear () {
        foreach (BulletBehavior b in Instance.m_pool) {
            if (null != b)
                Destroy (b.gameObject);
        }
        Instance.m_pool.Clear ();
    }
}