using UnityEngine;
using UnityEngine.U2D;
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

}