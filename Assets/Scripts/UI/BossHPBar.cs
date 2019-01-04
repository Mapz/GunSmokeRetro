using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossHPBar : MonoBehaviour {

    private int m_blockWidth = 8;
    private RectTransform imageTrans;

    private void Awake () {
        imageTrans = GetComponent<RectTransform> ();
    }

    public void Init (int _HP) {
        imageTrans.sizeDelta = new Vector2 (m_blockWidth * _HP, imageTrans.sizeDelta.y);
        imageTrans.anchoredPosition = new Vector2 (-imageTrans.sizeDelta.x, 110);
        imageTrans.SetParent (GameVars.UICanvas.transform, false);
    }

    public void SetHP (int _HP) {
        imageTrans.sizeDelta = new Vector2 (m_blockWidth * _HP, imageTrans.sizeDelta.y);
    }

}