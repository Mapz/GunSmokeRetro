using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
public static class GameVars {
    public static int ScreenWidth;
    public static int ScreenHeight;
    private static Camera _mainCamera;

    public static Camera mainCamera {
        get {
            return _mainCamera;
        }
        set { _mainCamera = value; }
    }

    private static Canvas _UICanvas;
    public static Canvas UICanvas {
        get {
            return _UICanvas;
        }
        set { _UICanvas = value; }
    }

    private static PixelPerfectCamera _ppCamera;
    public static PixelPerfectCamera ppCamera {
        get {
            return _ppCamera;
        }
        set { _ppCamera = value; }
    }

    public static Grid tileGrid;

    private static int _money = 0;

    public static int money {
        get { return _money; } set {
            if (value >= 0)
                _money = value;
            if (null != MoneyText) {
                MoneyText.text = _money.ToString ("D10");
            } else {
                _money = 0;
            }
        }
    }

    public static void newLevel () {
        money = 0;
    }

    public static int CurLevel;

    public static Game Game;

    public static InGameUI InGameUI;

    public static BossHPBar BossHPBar;

    public static Text MoneyText;

}