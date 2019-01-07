using System;
public static class Utility {
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
}