using System;
using System.Collections;
using System.Collections.Generic;
using BT;
using UnityEngine;
[System.Serializable]
public class SpawnData {

    public Vector3Int position;
    // public GameObject EnemeyToProduce;
    public string enemyName;
    public int maxCount;
    public float interval;
    public float rateOfSpawn;
    private int curCount = 0;
    private float curTime = 0;
    private bool active = false;

    public SpawnData (Vector3Int _position, string _enemyName, int _maxCount, float _interval) {
        position = _position;
        enemyName = _enemyName;
        maxCount = _maxCount;
        interval = _interval;
        curCount = 0;
        curTime = interval;
        active = false;
        rateOfSpawn = 1;
    }

    public SpawnData CloneSetPos (Vector3Int _position) {
        return new SpawnData (_position, enemyName, maxCount, interval);
    }

    public void SetPosition (Vector3Int _position) {
        position = _position;
    }

    public void SetActive (bool _active) {
        active = _active;
    }

    public void UpdateSpawn (float delta, Grid grid, Transform parent, Transform spawnerTransform) {
        if (!active) return;
        if (curCount >= maxCount) return;
        curTime -= delta;
        if (curTime <= 0) {
            Spawn (grid, parent, spawnerTransform);
            curCount++;
            curTime = interval;
        }
    }

    public void Spawn (Grid grid, Transform parent, Transform spawnerTransform) {
        if (UnityEngine.Random.Range (0f, 1f) >= rateOfSpawn) return;
        GameObject enemy = ObjectMgr<Unit>.Instance.Create (() => {
            return Utility.CreateUnit (enemyName);
        }).gameObject;
        enemy.transform.parent = parent;
        enemy.transform.position = grid.CellToLocal (position) + spawnerTransform.position + grid.cellSize / 2;
        enemy.transform.position += enemy.GetComponent<Unit> ().m_positionFffsetOnCreate;
    }
}

public class UnitSpawner : MonoBehaviour {

    [HideInInspector]
    public List<SpawnData> spawns;
    // private Game game;

    private Transform m_unitParent;

    private void Start () {
        m_unitParent = GameObject.Find ("Game").GetComponent<Game> ().m_level.transform;
    }
    void Update () {
        if (GameVars.Game.m_state != GameState.InGame) return;
        foreach (var data in spawns) {
            if (Game.pointInSpawnArea (transform.position + GameVars.tileGrid.CellToLocal (data.position))) {
                data.SetActive (true);
                data.UpdateSpawn (Time.deltaTime, GameVars.tileGrid, m_unitParent, transform);
            } else {
                data.SetActive (false);
            }

        }
    }

}