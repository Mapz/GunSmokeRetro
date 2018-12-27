using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
public class SpawnerEditWindow : EditorWindow {

    private UnitSpawner m_unitSpawner;
    private Vector3Int m_position;
    private int m_index;
    public static SpawnerEditWindow Initialize (UnitSpawner us, int index, Vector3Int position) {
        SpawnerEditWindow window = (SpawnerEditWindow) SpawnerEditWindow.GetWindow (typeof (SpawnerEditWindow));
        window.m_unitSpawner = us;
        window.m_position = position;
        window.m_index = index;
        window.Show ();
        return window;
    }

    void OnGUI () {

        EditorGUI.BeginChangeCheck ();
        GUI.enabled = false;
        EditorGUILayout.Vector3Field ("位置：", m_position, null);
        EditorGUILayout.Space ();

        // m_unitSpawner.spawns.TryGetValue (m_position, out sd);
        if (m_index >= m_unitSpawner.spawns.Count) {
            m_unitSpawner.spawns.Add (new SpawnData (m_position, null, 1, 0.1f));
        }
        SpawnData sd = m_unitSpawner.spawns[m_index];
        // Vector3 _position, GameObject _EnemeyToProduce, int _maxCount, float _interval
        var name = EditorGUILayout.TextField ("名称：", sd.EnemeyToProduce != null ? sd.EnemeyToProduce.name : "无");
        GUI.enabled = true;
        m_unitSpawner.spawns[m_index].maxCount = EditorGUILayout.DelayedIntField ("生产最大数：", sd.maxCount);
        m_unitSpawner.spawns[m_index].interval = EditorGUILayout.DelayedFloatField ("敌人产生间隔：", sd.interval);
        m_unitSpawner.spawns[m_index].EnemeyToProduce = (GameObject) EditorGUILayout.ObjectField ("敌人prefab：", sd.EnemeyToProduce, typeof (GameObject), null);
        EditorGUILayout.Space ();

        EditorGUI.EndChangeCheck ();

    }
}

#endif