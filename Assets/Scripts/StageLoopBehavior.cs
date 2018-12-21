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
    private float ScreenHeight = 240;

    // H 30 * 16 = 480;

    void Start () {
        upperMap = Instantiate (initLoop);
        upperMap.transform.parent = transform;
        upperTilemap = upperMap.GetComponent<LoopData> ().m_ground;
    }

    // Update is called once per frame
    void Update () {
        float curY = camera.WorldToScreenPoint (upperTilemap.transform.position + upperTilemap.localBounds.max).y;
        //屏幕到顶了，创建新循环
        if (curY <= ScreenHeight) {
            int r = Random.Range (0, loopMaps.Count);
            downerMap = Instantiate (loopMaps[r]);
            downerMap.transform.parent = transform;
            var position1 = upperTilemap.transform.position + (upperTilemap.localBounds.max + downerMap.GetComponent<LoopData> ().m_ground.localBounds.center);
            position1 = new Vector3 (0, position1.y, 0);
            downerMap.transform.position = position1;
            Swap<GameObject> (ref upperMap, ref downerMap);
            upperTilemap = upperMap.GetComponent<LoopData> ().m_ground;
        }
        //循环到底了，删除之前的循环
        if (null != downerMap) {
            float downerY = camera.WorldToScreenPoint (downerMap.transform.position + downerMap.GetComponent<LoopData> ().m_ground.localBounds.max).y;
            if (downerY <= 0) {
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