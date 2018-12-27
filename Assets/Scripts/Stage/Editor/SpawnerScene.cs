using UnityEditor;
using UnityEngine;

namespace UnityEditor {
    [CustomEditor (typeof (UnitSpawner))]
    public class SpawnerScene : Editor {

        void OnSceneGUI () {
            // get the chosen game object
            UnitSpawner t = target as UnitSpawner;
            if (t == null || t.spawns == null)
                return;
            Handles.color = Color.red;

            for (int i = 0; i < t.spawns.Count; i++) {
                EditorGUI.BeginChangeCheck ();
                Vector3 newPos = Handles.FreeMoveHandle (t.spawns[i].position + t.parent.position, Quaternion.identity, 16f, new Vector3 (1, 1, 0), Handles.CubeHandleCap);
                string name = "null";
                if (t.spawns[i].EnemeyToProduce != null) name = t.spawns[i].EnemeyToProduce.name;
                Handles.Label (t.spawns[i].position + t.parent.position - new Vector3 (8, -4, 0), "生怪器:" + name);
                if (EditorGUI.EndChangeCheck ()) {
                    t.spawns[i] = t.spawns[i].CloneSetPos (newPos - t.parent.position);
                }

            }
        }

    }
}