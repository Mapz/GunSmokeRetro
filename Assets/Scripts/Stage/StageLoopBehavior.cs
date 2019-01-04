using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageLoopBehavior : MonoBehaviour {
    public List<GameObject> loopMaps;
    public GameObject initLoop;
    public GameObject m_bossHPBarRes;
    public GameObject bossLoop; //老王出现的Loop
    private GameObject upperMap;
    private GameObject downerMap;
    private Tilemap upperTilemap;
    public GameObject bossPrefab;
    private GameObject m_boss;
    private int m_loopCount; //现在重复的次数
    private int m_loopBeforeBoss; // 还有几次到老王

    private void Awake () {
        m_loopCount = 0;
        m_loopBeforeBoss = 1;
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
        if (m_loopBeforeBoss > 0 && Game.pointInOutterScreen (YtopPosition)) {
            m_loopCount++;
            if (m_loopBeforeBoss >= 0) {
                m_loopBeforeBoss--;
            }
            int r = Random.Range (0, loopMaps.Count);
            if (m_loopBeforeBoss == 0) {
                downerMap = Instantiate (loopMaps[r]);
            } else {
                downerMap = Instantiate (bossLoop);
            }

            downerMap.transform.parent = transform;
            var position1 = upperTilemap.transform.position + upperTilemap.localBounds.max + downerMap.GetComponent<LoopData> ().m_ground.localBounds.center;
            position1 = new Vector3 (0, position1.y, 0);
            downerMap.transform.position = position1;
            Swap<GameObject> (ref upperMap, ref downerMap);
            upperTilemap = upperMap.GetComponent<LoopData> ().m_ground;
        } else if (m_loopBeforeBoss == 0 && Game.pointInScreen (YtopPosition)) {
            // 到Boss了
            if (m_boss != null) return;
            Game.m_rolling.moveSpeed = Vector3.zero;
            ObjectMgr<Unit>.Instance.Create (() => {
                m_boss = Instantiate (bossPrefab);
                m_boss.transform.parent = GameObject.Find ("Game").GetComponent<Game> ().m_level.transform;
                m_boss.transform.position = new Vector3 (0, 130, 0);
                BossHPBar m_bossHPBar = Instantiate (m_bossHPBarRes).GetComponent<BossHPBar> ();
                m_boss.GetComponent<Unit> ().m_HPBar = m_bossHPBar;
                m_bossHPBar.Init ((int) m_boss.GetComponent<Unit> ().m_HP);
                return m_boss.GetComponent<Unit> ();
            });
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