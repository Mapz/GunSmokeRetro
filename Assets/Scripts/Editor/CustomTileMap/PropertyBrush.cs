using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor {
    [CustomGridBrush (false, false, false, "Property Brush")]
    public class PropertyBrush : GridBrushBase {
        private PropertyEditWindow m_window;
        private Dictionary<GridInformation.GridInformationKey, GridInformation.GridInformationValue> m_pickedProperties;
        public override void Select (GridLayout grid, GameObject brushTarget, BoundsInt position) {

            if (brushTarget.layer == 31)
                return;
            Tilemap layerTilemap = brushTarget.GetComponent<Tilemap> ();
            GridInformation gi = brushTarget.GetComponent<GridInformation> ();
            Vector3Int pos = position.position;
            if (gi == null) {
                Debug.LogError ("TileMap对象必须包含一个 GridInformation 组件");
            } else if (null != layerTilemap && null == layerTilemap.GetTile (pos)) {
                Debug.LogWarning ("必须有一个Tile，才能加上数据");
                if (m_window) {
                    m_window.Close ();
                }
            } else {
                m_window = PropertyEditWindow.Initialize (pos, ref gi);
            }

        }
        public override void Paint (GridLayout grid, GameObject brushTarget, Vector3Int position) {

            if (brushTarget.layer == 31)
                return;
            Tilemap layerTilemap = brushTarget.GetComponent<Tilemap> ();
            GridInformation gi = brushTarget.GetComponent<GridInformation> ();

            if (gi == null) {
                Debug.LogError ("TileMap对象必须包含一个 GridInformation 组件");
            } else if (null != layerTilemap && null == layerTilemap.GetTile (position)) {
                Debug.LogWarning ("必须有一个Tile，才能加上数据");
                if (m_window) {
                    m_window.Close ();
                }
            } else {
                if (null != m_pickedProperties) {
                    foreach (var item in m_pickedProperties) {
                        gi.SetPositionProperty (position, item.Key.name, item.Value.type, item.Value.data);
                        Debug.LogWarning ("复制到了:" + item.Key.name + " @ " + position);
                    }
                    m_window = PropertyEditWindow.Initialize (position, ref gi);
                } else {
                    Debug.LogWarning ("剪贴板没有值");
                }
            }
        }

        public override void Pick (GridLayout gridLayout, GameObject brushTarget, BoundsInt position, Vector3Int pivot) {
            if (brushTarget.layer == 31)
                return;
            Tilemap layerTilemap = brushTarget.GetComponent<Tilemap> ();
            GridInformation gi = brushTarget.GetComponent<GridInformation> ();
            Vector3Int pos = position.position;
            if (gi == null) {
                Debug.LogError ("TileMap对象必须包含一个 GridInformation 组件");
            } else if (null != layerTilemap && null == layerTilemap.GetTile (pos)) {
                Debug.LogWarning ("必须有一个Tile，才能获取数据");
                if (m_window) {
                    m_window.Close ();
                }
            } else {
                m_pickedProperties = gi.getPropertiesInAGrid (pos);
                foreach (var item in m_pickedProperties) {
                    Debug.LogWarning ("复制了:" + item.Key.name + " @ " + position);
                }
            }
        }

        public override void Erase (GridLayout grid, GameObject brushTarget, Vector3Int position) {
            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;

            Tilemap layerTilemap = brushTarget.GetComponent<Tilemap> ();
            GridInformation gi = brushTarget.GetComponent<GridInformation> ();

            if (gi == null) {
                Debug.LogError ("TileMap对象必须包含一个 GridInformation 组件");
            } else if (null != layerTilemap && null == layerTilemap.GetTile (position)) {
                Debug.LogWarning ("必须有一个Tile，才能擦除数据");
                if (m_window) {
                    m_window.Close ();
                }
            } else {
                Dictionary<GridInformation.GridInformationKey, GridInformation.GridInformationValue> data = gi.getPropertiesInAGrid (position);
                foreach (var item in data) {
                    gi.ErasePositionProperty (position, item.Key.name);
                    Debug.LogWarning ("删除了:" + item.Key.name + " @ " + position);
                }
            }
        }
    }

    [CustomEditor (typeof (PropertyBrush))]
    public class PropertyBrushEditor : GridBrushEditorBase {
        public override GameObject[] validTargets {
            get {
                return GameObject.FindObjectsOfType<Tilemap> ().Select (x => x.gameObject).ToArray ();
            }
        }
        public override void OnPaintSceneGUI (GridLayout grid, GameObject brushTarget, BoundsInt position, GridBrushBase.Tool tool, bool executing) {
            base.OnPaintSceneGUI (grid, brushTarget, position, tool, executing);
            Tilemap layerTilemap = brushTarget.GetComponent<Tilemap> ();
            GridInformation gi = brushTarget.GetComponent<GridInformation> ();
            if (null == gi) return;
            int i = 0;
            Handles.color = Color.black;
            foreach (var p in gi.GetAllPositions ()) {
                Vector3 wp = grid.CellToWorld (p);
                Handles.RectangleHandleCap (i, wp + grid.cellSize / 2, Quaternion.identity, 8f, EventType.Repaint);
                Handles.Label (wp, "属性格子");
                i++;
            }

        }
    }
}