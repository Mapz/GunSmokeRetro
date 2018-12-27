using UnityEditor;
using UnityEngine;

namespace UnityEditor {
    [CustomEditor (typeof (UnitSpawner))]
    public class SpawnerScene : Editor {

        void OnSceneGUI () {
            // get the chosen game object
            UnitSpawner t = target as UnitSpawner;
            Grid grid = t.GetComponentsInParent<Grid> (true) [0];
            if (t == null || grid == null || t.spawns == null)
                return;
            Handles.color = Color.red;
            int i = 0;
            foreach (var p in t.spawns) {
                Vector3 wp;
                wp = grid.CellToLocal (p.position) + t.transform.position;
                Handles.RectangleHandleCap (i, wp + grid.cellSize / 2, Quaternion.identity, 8f, EventType.Repaint);
                Handles.Label (wp, "生怪器");
                i++;
            }

        }

    }
}