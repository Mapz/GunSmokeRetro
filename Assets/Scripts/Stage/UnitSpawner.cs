using System;
using System.Collections;
using System.Collections.Generic;
using BT;
using UnityEngine;
[System.Serializable]
public class SpawnData {

    public Vector3Int position;
    public GameObject EnemeyToProduce;
    public int maxCount;
    public float interval;
    public float rateOfSpawn;
    private int curCount = 0;
    private float curTime = 0;
    private bool active = false;

    public SpawnData (Vector3Int _position, GameObject _EnemeyToProduce, int _maxCount, float _interval) {
        position = _position;
        EnemeyToProduce = _EnemeyToProduce;
        maxCount = _maxCount;
        interval = _interval;
        curCount = 0;
        curTime = interval;
        active = false;
        rateOfSpawn = 1;
    }

    public SpawnData CloneSetPos (Vector3Int _position) {
        return new SpawnData (_position, EnemeyToProduce, maxCount, interval);
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
            return GameObject.Instantiate (EnemeyToProduce).GetComponent<Unit> ();
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
    public Grid m_grid;

    private Transform m_unitParent;

    private void Start () {
        m_unitParent = GameObject.Find ("Game").GetComponent<Game> ().m_level.transform;
        m_grid = GameObject.Find ("Game").GetComponent<Game> ().m_grid;
    }
    void Update () {
        foreach (var data in spawns) {
            // Debug.Log (m_grid.CellToLocal (data.position));
            // Debug.Log (transform.position + m_grid.CellToLocal (data.position));
            if (Game.pointInSpawnArea (transform.position + m_grid.CellToLocal (data.position))) {
                data.SetActive (true);
                data.UpdateSpawn (Time.deltaTime, m_grid, m_unitParent, transform);
            } else {
                data.SetActive (false);
            }

        }
    }

}