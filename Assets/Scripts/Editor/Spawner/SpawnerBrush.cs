using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor {
    [CustomGridBrush (false, false, false, "Spawner Brush")]
    public class SpawnerBrush : GridBrushBase {
        private SpawnerEditWindow m_window;
        private SpawnData m_clipBoardBuff;

        public override void Select (GridLayout grid, GameObject brushTarget, BoundsInt position) {

            if (brushTarget.layer == 31)
                return;
            Tilemap layerTilemap = brushTarget.GetComponent<Tilemap> ();
            UnitSpawner spawner = brushTarget.GetComponent<UnitSpawner> ();
            if (spawner == null) {
                Debug.LogError ("TileMap对象必须包含一个 生怪器 组件");
                return;
            }
            if (m_window) {
                m_window.Close ();
            }
            Vector3Int pos = position.position;

            for (int i = 0; i < spawner.spawns.Count; i++) {
                if (pos == spawner.spawns[i].position) {
                    m_window = SpawnerEditWindow.Initialize (spawner, i, pos);
                    return;
                }
            }
            m_window = SpawnerEditWindow.Initialize (spawner, spawner.spawns.Count, pos);

        }
        public override void Paint (GridLayout grid, GameObject brushTarget, Vector3Int pos) {

            if (brushTarget.layer == 31)
                return;
            Tilemap layerTilemap = brushTarget.GetComponent<Tilemap> ();
            UnitSpawner spawner = brushTarget.GetComponent<UnitSpawner> ();

            if (spawner == null) {
                Debug.LogError ("TileMap对象必须包含一个 生怪器 组件");
                return;
            }
            if (m_window) {
                m_window.Close ();
            }

            if (null != m_clipBoardBuff) {
                for (int i = 0; i < spawner.spawns.Count; i++) {
                    if (pos == spawner.spawns[i].position) {
                        spawner.spawns[i] = m_clipBoardBuff.CloneSetPos (pos);
                        m_window = SpawnerEditWindow.Initialize (spawner, i, pos);
                        return;
                    }
                }
                spawner.spawns.Add (m_clipBoardBuff.CloneSetPos (pos));
                m_window = SpawnerEditWindow.Initialize (spawner, spawner.spawns.Count - 1, pos);
                return;
            } else {
                Debug.LogWarning ("剪贴板没有值");
            }

        }

        public override void Pick (GridLayout gridLayout, GameObject brushTarget, BoundsInt position, Vector3Int pivot) {
            if (brushTarget.layer == 31)
                return;
            Tilemap layerTilemap = brushTarget.GetComponent<Tilemap> ();
            UnitSpawner spawner = brushTarget.GetComponent<UnitSpawner> ();
            Vector3Int pos = position.position;
            if (spawner == null) {
                Debug.LogError ("TileMap对象必须包含一个 生怪器 组件");
                return;
            }
            if (m_window) {
                m_window.Close ();
            }

            for (int i = 0; i < spawner.spawns.Count; i++) {
                if (pos == spawner.spawns[i].position) {
                    m_clipBoardBuff = spawner.spawns[i];
                    Debug.LogWarning ("复制了刷怪器: @ " + position);
                    return;
                }
            }

        }

        public override void Erase (GridLayout grid, GameObject brushTarget, Vector3Int pos) {
            if (brushTarget.layer == 31)
                return;

            Tilemap layerTilemap = brushTarget.GetComponent<Tilemap> ();
            UnitSpawner spawner = brushTarget.GetComponent<UnitSpawner> ();
            if (spawner == null) {
                Debug.LogError ("TileMap对象必须包含一个 生怪器 组件");
                return;
            }
            if (m_window) {
                m_window.Close ();
            }

            for (int i = 0; i < spawner.spawns.Count; i++) {
                if (pos == spawner.spawns[i].position) {
                    spawner.spawns.Remove (spawner.spawns[i]);
                    return;
                }
            }
        }
    }

    [CustomEditor (typeof (SpawnerBrush))]
    public class SpawnerBrushEditor : GridBrushEditorBase {
        public override GameObject[] validTargets {
            get {
                return GameObject.FindObjectsOfType<Tilemap> ().Select (x => x.gameObject).ToArray ();
            }
        }
        public override void OnPaintSceneGUI (GridLayout grid, GameObject brushTarget, BoundsInt position, GridBrushBase.Tool tool, bool executing) {
            base.OnPaintSceneGUI (grid, brushTarget, position, tool, true);
            Tilemap layerTilemap = brushTarget.GetComponent<Tilemap> ();
            UnitSpawner spawner = brushTarget.GetComponent<UnitSpawner> ();

            int i = 0;
            Handles.color = Color.red;
            foreach (var p in spawner.spawns) {
                Vector3 wp = grid.CellToWorld (p.position);
                Handles.RectangleHandleCap (i, wp + grid.cellSize / 2, Quaternion.identity, 8f, EventType.Repaint);
                Handles.Label (wp, "生怪器");
                i++;
            }

        }

    }
}