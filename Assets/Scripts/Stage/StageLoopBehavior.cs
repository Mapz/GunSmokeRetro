using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageLoopBehavior : MonoBehaviour {
    public List<GameObject> loopMaps;
    public GameObject initLoop;
    private GameObject upperMap;
    private GameObject downerMap;
    private Tilemap upperTilemap;
    public Camera camera;

    private void Awake () {
        camera = GameObject.Find ("Main Camera").GetComponent<Camera> ();
    }

    void Start () {
        upperMap = Instantiate (initLoop);
        upperMap.transform.parent = transform;
        upperTilemap = upperMap.GetComponent<LoopData> ().m_ground;
    }

    // Update is called once per frame
    void Update () {
        Vector3 YtopPosition = upperTilemap.transform.position + upperTilemap.localBounds.max; // + upperTilemap.localBounds.size.y / 2;
        //屏幕到顶了，创建新循环
        if (Game.pointInOutterScreen (YtopPosition)) {
            int r = Random.Range (0, loopMaps.Count);
            downerMap = Instantiate (loopMaps[r]);
            downerMap.transform.parent = transform;
            var position1 = upperTilemap.transform.position + upperTilemap.localBounds.max + downerMap.GetComponent<LoopData> ().m_ground.localBounds.center;
            position1 = new Vector3 (0, position1.y, 0);
            downerMap.transform.position = position1;
            Swap<GameObject> (ref upperMap, ref downerMap);
            upperTilemap = upperMap.GetComponent<LoopData> ().m_ground;
        }
        //循环到底了，删除之前的循环
        if (null != downerMap) {
            Vector3 downerPosition = downerMap.transform.position + downerMap.GetComponent<LoopData> ().m_ground.localBounds.max;
            if (!Game.pointInOutterScreen (downerPosition)) {
                Destroy (downerMap);
            }
        }
    }

    //交换两个引用
    void Swap<T> (ref T var1, ref T var2) {
        T tmp = var1;
        var1 = var2;
        var2 = tmp;
    }
}