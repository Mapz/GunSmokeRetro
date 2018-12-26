using System;
using System.Collections;
using System.Collections.Generic;
using BT;
using UnityEngine;
[System.Serializable]
public class SpawnData {

    public Vector3 position;
    public GameObject EnemeyToProduce;
    public int maxCount;
    public float interval;
    private int curCount = 0;
    private float curTime = 0;
    private bool active = false;

    public SpawnData (Vector3 _position, GameObject _EnemeyToProduce, int _maxCount, float _interval) {
        position = _position;
        EnemeyToProduce = _EnemeyToProduce;
        maxCount = _maxCount;
        interval = _interval;
        curCount = 0;
        curTime = interval;
        active = false;
    }

    public SpawnData CloneSetPos (Vector3 _position) {
        return new SpawnData (_position, EnemeyToProduce, maxCount, interval);
    }

    public void SetPosition (Vector3 _position) {
        position = _position;
    }

    public void SetActive (bool _active) {
        active = _active;
    }

    public void UpdateSpawn (float delta, Transform parent) {
        if (!active) return;
        if (curCount >= maxCount) return;
        curTime -= delta;
        if (curTime <= 0) {
            Spawn (parent);
            curCount++;
            curTime = interval;
        }
    }

    public void Spawn (Transform parent) {
        GameObject enemy = UnitMgr.CreateUnit (() => {
            return GameObject.Instantiate (EnemeyToProduce).GetComponent<Unit> ();
        }).gameObject;
        enemy.transform.parent = parent;
        enemy.transform.position = position;
    }
}

public class UnitSpawner : MonoBehaviour {

    public List<SpawnData> spawns;
    private Game game;
    private Transform parent;

    private void Start () {
        game = GameObject.Find ("Game").GetComponent<Game> ();
        parent = game.m_level.transform;
    }
    void Update () {
        foreach (SpawnData data in spawns) {
            if (game.pointInSpawnArea (data.position)) {
                data.SetActive (true);
                data.UpdateSpawn (Time.deltaTime, parent);
            } else {
                data.SetActive (false);
            }

        }
    }

}