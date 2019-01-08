using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class ShowLife : MonoBehaviour {
    public DG.Tweening.TweenCallback m_OnOver;
    public Text m_lifeText;

    void Awake () {
        this.gameObject.SetActive (false);
    }
    public void Show (DG.Tweening.TweenCallback callBack) {
        this.gameObject.SetActive (true);
        m_OnOver = callBack;
        m_lifeText.text = GameVars.CurLife.ToString ();
        Utility.FadeInOut (3f, 1f, callBack);
    }

}