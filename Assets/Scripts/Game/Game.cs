using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Game : MonoBehaviour {
    [System.NonSerialized]

    //屏幕界限
    private Bounds screenBounds;

    //屏幕外部界限
    private Bounds outerBounds;

    private Camera camera;

    public GameObject LevelPrefab;

    public GameObject HeroPrefab;

    public HeroBehavior m_hero;

    private RollingLayer m_rolling;

    [System.NonSerialized]
    public GameObject m_level;

    void Awake () {
        camera = GameObject.Find ("Main Camera").GetComponent<Camera> ();
        PixelPerfectCamera ppc = GameObject.Find ("Main Camera").GetComponent<PixelPerfectCamera> ();
        screenBounds = new Bounds (new Vector3 (ppc.refResolutionX / 2, ppc.refResolutionY / 2, -camera.transform.position.z), new Vector3 (ppc.refResolutionX, ppc.refResolutionY, 0));
        outerBounds = new Bounds (new Vector3 (ppc.refResolutionX / 2, ppc.refResolutionY / 2, -camera.transform.position.z), new Vector3 (ppc.refResolutionX * 1.5f, ppc.refResolutionY * 1.5f, 0));
    }

    void Start () {
        loadLevel ();
    }

    public void loadLevel () {
        m_level = Instantiate (LevelPrefab);
        m_rolling = m_level.GetComponent<RollingLayer> ();
        m_hero = Instantiate (HeroPrefab).GetComponent<HeroBehavior> ();
    }

    private void Update () {
        if (m_hero.dead) {
            m_rolling.moveSpeed.y = 0;
        }
    }

    public bool pointInScreen (Vector3 point, ref Vector3 ClosetPoint) {
        Vector3 ScreenPoint = camera.WorldToScreenPoint (point);
        if (screenBounds.Contains (ScreenPoint)) {
            return true;
        } else {
            ClosetPoint = screenBounds.ClosestPoint (ScreenPoint);
            ClosetPoint = camera.ScreenToWorldPoint (ClosetPoint);
            return false;
        }
    }

    public bool pointInOutterScreen (Vector3 point) {
        Vector3 ScreenPoint = camera.WorldToScreenPoint (point);
        return outerBounds.Contains (ScreenPoint);
    }

    public bool pointInSpawnArea (Vector3 point) {
        Vector3 ScreenPoint = camera.WorldToScreenPoint (point);
        return (!screenBounds.Contains (point) && outerBounds.Contains (ScreenPoint));
    }
}