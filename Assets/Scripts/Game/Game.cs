using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using XAsset;
public enum GameState {
    Init,
    TitleScreen,
    WantedScreen,
    LoadLevel,
    InGame,
    Pause,
    InGameOver,
}

// TODO:生怪器坐标修正 完成
// TODO:PropertyBrush增加屏幕显示 完成
// TODO:生怪器用生怪刷搞定 完成
// TODO:刷怪器概率刷怪 完成
// TODO:Unit 离开销毁 完成
// TODO:Boss1动画锚点优化 完成
// TODO:TitleScreen 完成
// TODO:Boss死后，游戏冻结，主角不能受伤 完成

// TODO:随机障碍物系统
// TODO:UI系统
// TODO:Boss系统,Boss移动方式
// TODO:Unit 离开AI
// TODO:子弹影子用Shader来制作
// TODO:Unit 配置表化
// TODO:加载资源化
// TODO:优化朝向动画设置逻辑
// TODO:关卡头演绎
// TODO:声音

public class Game : MonoBehaviour, PauseAble {
    [System.NonSerialized]
    //屏幕界限
    private static Bounds screenBounds;
    //屏幕外部界限
    private static Bounds outterBounds;
    private static Bounds spawnerActiveBounds; // 要比上面那个小
    // public GameObject LevelPrefab;

    [System.NonSerialized]
    public HeroBehavior m_hero;

    [System.NonSerialized]
    public static RollingLayer m_rolling;
    [System.NonSerialized]
    public GameObject m_level;
    [System.NonSerialized]
    public GameState m_state;
    public GameObject m_titlePrefab;
    private bool m_onChangeState = false;
    private GameState m_lastState;
    public static bool m_isPaused;
    private GameObject m_titleScreen;
    public void SetGameState (GameState state) {
        m_onChangeState = true;
        m_lastState = m_state;
        m_state = state;
    }

    void Awake () {
        _Init ();
        SetGameState (GameState.Init);
    }

    private void _Init () {
        LevelsConfig.Init ();
        GameVars.mainCamera = GameObject.Find ("Main Camera").GetComponent<Camera> ();
        GameVars.ppCamera = GameObject.Find ("Main Camera").GetComponent<PixelPerfectCamera> ();
        GameVars.UICanvas = GameObject.Find ("UICanvas").GetComponent<Canvas> ();
        GameVars.Game = this;
        GameVars.ScreenHeight = GameVars.ppCamera.refResolutionY;
        GameVars.ScreenWidth = GameVars.ppCamera.refResolutionX;
        GameVars.CurLevel = 1;
        Utility.Init ();
    }

    private void Init () {
        screenBounds = new Bounds (new Vector3 (0, 0, 0), new Vector3 (GameVars.ppCamera.refResolutionX, GameVars.ppCamera.refResolutionY, 0));
        outterBounds = new Bounds (new Vector3 (0, 0, 0), new Vector3 (GameVars.ppCamera.refResolutionX * 1.5f, GameVars.ppCamera.refResolutionY * 1.5f, 0));
        spawnerActiveBounds = new Bounds (new Vector3 (0, 0, 0), new Vector3 (GameVars.ppCamera.refResolutionX * 1.3f, GameVars.ppCamera.refResolutionY * 1.3f, 0));
    }

    private void LoadTitle () {
        m_titleScreen = Instantiate (m_titlePrefab) as GameObject;
        m_titleScreen.transform.SetParent (GameVars.UICanvas.transform, false);
        Utility.Fade (1.5f, true);
    }

    private void OnChangeState (GameState newState, GameState oldState) {
        m_onChangeState = false;
        switch (newState) {
            case GameState.Init:
                Init ();
                SetGameState (GameState.TitleScreen);
                break;
            case GameState.TitleScreen:
                LoadTitle ();
                break;
            case GameState.InGameOver:
                ObjectMgr<BulletBehavior>.Instance.PauseAll (true);
                ObjectMgr<Unit>.Instance.PauseAll (true);
                m_rolling.Pause (true);
                GameVars.InGameUI.LevelEnd ();
                Utility.Fade (3f, false, () => {
                    unloadLevel ();
                    SetGameState (GameState.LoadLevel);
                });
                break;
            case GameState.LoadLevel:
                GameVars.newLevel ();
                ObjectMgr<BulletBehavior>.Instance.Clear ();
                ObjectMgr<Unit>.Instance.Clear ();
                loadLevel ();
                SetGameState (GameState.InGame);
                break;
            case GameState.InGame:
                m_rolling.Pause (false);
                GameVars.InGameUI.LevelStart ();
                break;
        }
    }

    public void Pause (bool _pause) {
        ObjectMgr<BulletBehavior>.Instance.PauseAll (_pause);
        ObjectMgr<Unit>.Instance.PauseAll (_pause);
        m_rolling.Pause (_pause);
        m_isPaused = _pause;
    }

    private void Update () {
        if (m_onChangeState)
            OnChangeState (m_state, m_lastState);
        switch (m_state) {
            case GameState.Init:
                break;
            case GameState.LoadLevel:
                break;
            case GameState.TitleScreen:
                if (Input.GetKeyDown ("b")) {
                    //TODO :AfterPressedLockKeyDown
                    Text pushStart = GameObject.Find ("TextPushStart").GetComponent<Text> ();
                    DOTween.ToAlpha (() => pushStart.color, x => pushStart.color = x, 0f, 0.1f).SetLoops (5, LoopType.Yoyo).OnComplete (
                        () => {
                            Utility.Fade (1.5f, false, () => {
                                Destroy (m_titleScreen);
                                SetGameState (GameState.LoadLevel);
                            });
                        }
                    );
                }
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
        m_level = Utility.Instantiate ("Assets/AssetsToBuild/Level/Level");
        GameVars.tileGrid = m_level.GetComponent<Grid> ();
        m_rolling = m_level.GetComponent<RollingLayer> ();
        m_hero = (HeroBehavior) ObjectMgr<Unit>.Instance.Create (() => {
            return Utility.CreateUnit ("Hero");
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
        return outterBounds.Contains (point);
    }

    public static bool pointInSpawnArea (Vector3 point) {
        return (!screenBounds.Contains (point) && spawnerActiveBounds.Contains (point));
    }
}