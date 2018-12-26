using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate Unit GetUnit ();
public class UnitMgr : MonoBehaviour {

    public static UnitMgr Instance;

    private List<Unit> m_pool = new List<Unit> ();

    private void Awake () {
        Instance = this;
    }

    public static UnitMgr Init () {
        if (null != Instance) return Instance;
        new GameObject ("UnitMgr").AddComponent<UnitMgr> ();
        return Instance;
    }

    public static Unit CreateUnit (GetUnit action) {
        Unit newUnit = action ();
        Instance.m_pool.Add (newUnit);
        return newUnit;

    }

    public static void DestroyUnit (Unit unit) {
        Instance.m_pool.Remove (unit);
        Destroy (unit.gameObject);
    }

    public static void PauseAll () {
        foreach (Unit u in Instance.m_pool) {
            if (null == u) continue;
            foreach (WeaponsBehavior weapon in u.weapons) {
                weapon.active = false;
            }
            if (null != u.GetComponent<Rigidbody2D> ()) {
                u.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
            }
        }
    }

    public static void Clear () {
        foreach (Unit u in Instance.m_pool) {
            if (null != u)
                Destroy (u.gameObject);
        }
        Instance.m_pool.Clear ();
    }
}