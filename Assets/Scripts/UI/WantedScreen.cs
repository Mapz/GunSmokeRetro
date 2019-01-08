using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class WantedScreen : MonoBehaviour {
    private Text m_bossName;
    private Text m_bossWeapon;
    private Text m_bossReward;
    private Image m_bossWanted;
    private LevelConfig m_levelConfig;
    private float curCharDuration = 0.3f;
    public Action m_OnOver;

    public static WantedScreen Init (Action _OnOver) {
        GameObject go = new GameObject ("WantedScreen");
        WantedScreen wantedScreen = go.AddComponent<WantedScreen> ();
        go.transform.SetParent (GameVars.InGameUI.transform, false);
        wantedScreen.m_OnOver = _OnOver;
        return wantedScreen;
    }

    void Start () {
        m_levelConfig = LevelsConfig.GetCurLevelConfig ();
        ShowLevelName ();
    }

    void ShowLevelName () {;
        GameObject go = new GameObject ("LevelName");
        go.transform.SetParent (this.transform, false);
        Text levelName = go.AddComponent<Text> ();
        levelName.alignment = TextAnchor.MiddleCenter;
        levelName.font = GameVars.MainFont;
        levelName.fontSize = 8;
        levelName.text = m_levelConfig.levelName;
        levelName.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 0);
        levelName.GetComponent<RectTransform> ().sizeDelta = new Vector2 (GameVars.ScreenWidth, 100);
        Utility.FadeInOut (6f, 1f, () => {
            Destroy (go);
            ShowWantedImage ();
        });
    }

    void ShowWantedImage () {
        GameObject go = new GameObject ("Wanted");
        go.transform.SetParent (this.transform, false);
        m_bossWanted = go.AddComponent<Image> ();
        m_bossWanted.sprite = Utility.GetWanted (m_levelConfig.wantedPostName);
        m_bossWanted.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 0);
        m_bossWanted.SetNativeSize ();
        Utility.Fade (3f, true, () => {
            //FadeOver then move;
            m_bossWanted.GetComponent<RectTransform> ().DOMoveX (75, 1f, true).SetEase (Ease.Linear).OnComplete (() => {
                //Show Name And Reawrd
                ShowBossNameAndReward ();
            });
        });
    }

    void ShowBossNameAndReward () {
        GameObject go = new GameObject ("BossName");
        go.transform.SetParent (this.transform, false);
        m_bossName = go.AddComponent<Text> ();
        m_bossName.alignment = TextAnchor.LowerLeft;
        m_bossName.font = GameVars.MainFont;
        m_bossName.fontSize = 8;
        // m_bossName.text = m_levelConfig.levelName;
        m_bossName.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (140, 30);
        m_bossName.GetComponent<RectTransform> ().sizeDelta = new Vector2 (GameVars.ScreenWidth, 10);
        m_bossName.DOText (m_levelConfig.bossName, curCharDuration * m_levelConfig.bossName.Length).SetEase (Ease.Linear).OnComplete (() => {
            GameObject go1 = new GameObject ("BossWeapon");
            go1.transform.SetParent (this.transform, false);
            m_bossWeapon = go1.AddComponent<Text> ();
            m_bossWeapon.alignment = TextAnchor.LowerLeft;
            m_bossWeapon.font = GameVars.MainFont;
            m_bossWeapon.fontSize = 8;
            // m_bossWeapon.text = m_levelConfig.levelName;
            m_bossWeapon.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (140, 0);
            m_bossWeapon.GetComponent<RectTransform> ().sizeDelta = new Vector2 (GameVars.ScreenWidth, 10);
            m_bossWeapon.DOText (m_levelConfig.bossWeaponName, curCharDuration * m_levelConfig.bossWeaponName.Length).SetEase (Ease.Linear).OnComplete (() => {
                GameObject go2 = new GameObject ("BossReward");
                go2.transform.SetParent (this.transform, false);
                m_bossWeapon = go2.AddComponent<Text> ();
                m_bossWeapon.alignment = TextAnchor.LowerLeft;
                m_bossWeapon.font = GameVars.MainFont;
                m_bossWeapon.fontSize = 8;
                // m_bossWeapon.text = m_levelConfig.levelName;
                m_bossWeapon.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (140, -30);
                m_bossWeapon.GetComponent<RectTransform> ().sizeDelta = new Vector2 (GameVars.ScreenWidth, 10);
                m_bossWeapon.DOText ("$ " + m_levelConfig.bossReward, curCharDuration * m_levelConfig.bossReward.ToString ().Length).SetEase (Ease.Linear).OnComplete (() => {
                    Utility.Fade (3f, false, () => {
                        //FadeOver then move;
                        Destroy (m_bossWanted.gameObject);
                        Destroy (go);
                        Destroy (go1);
                        Destroy (go2);
                        m_OnOver ();
                        Destroy (this.gameObject);
                    });
                });
            });
        });
    }

    // Update is called once per frame
    void Update () {

    }
}