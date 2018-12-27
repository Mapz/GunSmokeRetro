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

// PropertyBrsh:增加屏幕显示
// TODO:生怪器坐标问题,生怪器重写 --TileMap 形式
// TODO:UI系统

public class Game : MonoBehaviour, PauseAble {
    [System.NonSerialized]
    //屏幕界限
    private static Bounds screenBounds;
    //屏幕外部界限
    private static Bounds outerBounds;
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

    void Awake () {
        SetGameState (GameState.Init);
    }

    private void OnChangeState (GameState newState, GameState oldState) {
        m_onChangeState = false;
        switch (newState) {
            case GameState.Init:
                camera = GameObject.Find ("Main Camera").GetComponent<Camera> ();
                PixelPerfectCamera ppc = GameObject.Find ("Main Camera").GetComponent<PixelPerfectCamera> ();
                screenBounds = new Bounds (new Vector3 (0, 0, 0), new Vector3 (ppc.refResolutionX, ppc.refResolutionY, 0));
                outerBounds = new Bounds (new Vector3 (0, 0, 0), new Vector3 (ppc.refResolutionX * 1.5f, ppc.refResolutionY * 1.5f, 0));
                SetGameState (GameState.Loading);
                break;
            case GameState.HeroFail:
                ObjectMgr<BulletBehavior>.Instance.PauseAll (true);
                ObjectMgr<Unit>.Instance.PauseAll (true);
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
                ObjectMgr<BulletBehavior>.Instance.Clear ();
                ObjectMgr<Unit>.Instance.Clear ();
                loadLevel ();
                SetGameState (GameState.InGame);
                break;
            case GameState.InGame:
                m_rolling.Pause (false);
                break;
        }
    }

    public void Pause (bool _pause) {
        ObjectMgr<BulletBehavior>.Instance.PauseAll (_pause);
        ObjectMgr<Unit>.Instance.PauseAll (_pause);
        m_rolling.Pause (_pause);
    }

    private void Update () {
        if (m_onChangeState)
            OnChangeState (m_state, m_lastState);
        switch (m_state) {
            case GameState.Init:
                break;
            case GameState.Loading:
                break;
            case GameState.InGame:
                if (Input.GetKeyDown ("p")) {
                    Pause (true);
                    SetGameState (GameState.Pause);
                }
                break;
            case GameState.Pause:
                if (Input.GetKeyDown ("p")) {
                    Pause (false);
                    SetGameState (GameState.InGame);
                }
                break;
        }
    }

    public void loadLevel () {
        m_level = Instantiate (LevelPrefab);
        m_rolling = m_level.GetComponent<RollingLayer> ();
        m_hero = (HeroBehavior) ObjectMgr<Unit>.Instance.Create (() => {
            return Instantiate (HeroPrefab).GetComponent<HeroBehavior> ();
        });
    }

    public void unloadLevel () {
        Destroy (m_level);
        if (null != m_hero) {
            ObjectMgr<Unit>.Instance.Destroy (m_hero);
        }
    }

    public static bool pointInScreen (Vector3 point) {
        if (screenBounds.Contains (point)) {
            return true;
        } else {
            return false;
        }
    }

    public static bool pointInScreen (Vector3 point, ref Vector3 ClosetPoint) {
        if (screenBounds.Contains (point)) {
            return true;
        } else {
            ClosetPoint = screenBounds.ClosestPoint (point);
            return false;
        }
    }

    public static bool pointInOutterScreen (Vector3 point) {
        return outerBounds.Contains (point);
    }

    public static bool pointInSpawnArea (Vector3 point) {
        return (!screenBounds.Contains (point) && outerBounds.Contains (point));
    }
}