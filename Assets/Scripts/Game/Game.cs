using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.U2D;
public enum GameState {
    Init,
    Loading,
    InGame,
    Pause,
    HeroFail,
}

// TODO:屏幕分辨率适配
// TODO:生怪器坐标问题
// TODO:UI系统

public class Game : MonoBehaviour {
    [System.NonSerialized]
    //屏幕界限
    private Bounds screenBounds;
    //屏幕外部界限
    private Bounds outerBounds;
    private Camera camera;
    public GameObject LevelPrefab;
    public GameObject HeroPrefab;
    [System.NonSerialized]
    public HeroBehavior m_hero;
    private RollingLayer m_rolling;
    [System.NonSerialized]
    public GameObject m_level;
    private GameState m_state;

    private bool m_onChangeState = false;
    private GameState m_lastState;
    public void SetGameState (GameState state) {
        m_onChangeState = true;
        m_lastState = m_state;
        m_state = state;
    }

    private void OnChangeState (GameState newState, GameState oldState) {
        m_onChangeState = false;
        switch (newState) {
            case GameState.HeroFail:
                UnitMgr.PauseAll ();
                BulletMgr.PauseAll ();
                m_rolling.Pause (true);
                FadeCamera fade = camera.gameObject.AddComponent<FadeCamera> ();
                fade.duration = 3;
                fade.start = 0;
                fade.end = 1;
                fade.DoFade ();
                fade.callBack = () => {
                    unloadLevel ();
                    SetGameState (GameState.Loading);
                    Destroy (fade);
                };
                break;
            case GameState.Loading:
                UnitMgr.Clear ();
                BulletMgr.Clear ();
                loadLevel ();
                SetGameState (GameState.InGame);
                break;
            case GameState.InGame:
                m_rolling.Pause (false);
                break;
        }
    }

    private void Update () {
        if (m_onChangeState)
            OnChangeState (m_state, m_lastState);
        switch (m_state) {
            case GameState.Init:
                UnitMgr.Init ();
                BulletMgr.Init ();
                camera = GameObject.Find ("Main Camera").GetComponent<Camera> ();
                PixelPerfectCamera ppc = GameObject.Find ("Main Camera").GetComponent<PixelPerfectCamera> ();
                screenBounds = new Bounds (new Vector3 (ppc.refResolutionX / 2, ppc.refResolutionY / 2, -camera.transform.position.z), new Vector3 (ppc.refResolutionX, ppc.refResolutionY, 0));
                outerBounds = new Bounds (new Vector3 (ppc.refResolutionX / 2, ppc.refResolutionY / 2, -camera.transform.position.z), new Vector3 (ppc.refResolutionX * 1.5f, ppc.refResolutionY * 1.5f, 0));
                SetGameState (GameState.Loading);
                break;
        }
    }

    void Awake () {

    }

    void Start () {

    }

    public void loadLevel () {
        m_level = Instantiate (LevelPrefab);
        m_rolling = m_level.GetComponent<RollingLayer> ();
        m_hero = (HeroBehavior) UnitMgr.CreateUnit (() => {
            return Instantiate (HeroPrefab).GetComponent<HeroBehavior> ();
        });
    }

    public void unloadLevel () {
        Destroy (m_level);
        if (null != m_hero) {
            UnitMgr.DestroyUnit (m_hero);
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