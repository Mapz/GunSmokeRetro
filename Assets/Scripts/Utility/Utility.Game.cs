using System;
using UnityEngine;
using XAsset;
public partial class Utility {
    private static FadeCamera fade;

    public static void Init () {
        fade = GameVars.mainCamera.gameObject.AddComponent<FadeCamera> ();
    }
    public static bool Fade (float duration, bool inOut, DG.Tweening.TweenCallback callBack = null) {
        if (!fade._done) {
            return false;
        } else {
            fade.duration = duration;
            fade.start = inOut ? 1 : 0;
            fade.end = inOut ? 0 : 1;
            fade.callBack = callBack;
            fade.DoFade ();
            return true;
        }
    }

    public static bool FadeInOut (float fadeDuration, float showDuration, DG.Tweening.TweenCallback callBack = null) {
        return Fade (fadeDuration / 2, true, () => {
            new EnumTimer (() => Fade (fadeDuration / 2, false, callBack), showDuration).StartTimeout (fade);
        });
    }

    public static Unit CreateUnit (string unitName) {
        Asset asset = Assets.Load<GameObject> (UnitPrefabPath + unitName + ".prefab");
        if (asset != null) {
            var prefab = asset.asset;
            if (prefab != null) {
                var go = Game.Instantiate (prefab) as GameObject;
                ReleaseAssetOnDestroy.Register (go, asset);
                return go.GetComponent<Unit> ();
            } else { throw new System.Exception ("创建Unit失败:" + unitName); }
        } else { throw new System.Exception ("创建Unit失败:" + unitName); }
    }

    public static GameObject Instantiate (string Path) {
        Asset asset = Assets.Load<GameObject> (Path + ".prefab");
        if (asset != null) {
            var prefab = asset.asset;
            if (prefab != null) {
                var go = Game.Instantiate (prefab) as GameObject;
                ReleaseAssetOnDestroy.Register (go, asset);
                return go;
            } else { throw new System.Exception ("创建GO失败:" + Path); }
        } else { throw new System.Exception ("创建GO失败:" + Path); }
    }

    public static GameObject CreateLevelLoop (string name) {
        return Instantiate (LevelLoopsPrefabPath + name);
    }

    public static Font GetFont (string name) {
        Asset asset = Assets.Load<Font> (FontPath + name);
        if (asset != null) {
            Font font = (Font) asset.asset;
            if (font != null) {
                return font;
            } else { throw new System.Exception ("加载字体失败:" + name); }
        } else { throw new System.Exception ("加载字体失败:" + name); }
    }

    public static Sprite GetWanted (string name) {
        Asset asset = Assets.Load<Sprite> (WantedPath + name + ".png");
        if (asset != null) {
            Sprite wanted = (Sprite) asset.asset;
            if (wanted != null) {
                return wanted;
            } else { throw new System.Exception ("加载通缉失败:" + name); }
        } else { throw new System.Exception ("加载通缉失败:" + name); }
    }
}