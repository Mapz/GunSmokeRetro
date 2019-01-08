using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class WantedScreen : MonoBehaviour {
    private LevelConfig m_levelConfig;
    private float curCharDuration = 0.2f;
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
        Utility.FadeInOut (3f, 1f, () => {
            Destroy (go);
            StartCoroutine (ShowBossInfo ());
        });
    }

    // 改为协程方式执行
    IEnumerator ShowBossInfo () {

        GameObject go3 = new GameObject ("Wanted");
        go3.transform.SetParent (this.transform, false);
        Image bossImg = go3.AddComponent<Image> ();
        bossImg.sprite = Utility.GetWanted (m_levelConfig.wantedPostName);
        bossImg.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 0);
        bossImg.SetNativeSize ();
        Utility.Fade (true);
        yield return new WaitWhile (() => {
            return !Utility.isFadeOver;
        });

        Tweener tn = bossImg.GetComponent<RectTransform> ().DOAnchorPosX (-60, 1f, true).SetEase (Ease.Linear);
        yield return new DOTweenCYInstruction.WaitForCompletion (tn);
        yield return new WaitForSecondsRealtime (0.5f);

        // BOSSName
        GameObject go = new GameObject ("BossName");
        go.transform.SetParent (this.transform, false);
        Text bossName = go.AddComponent<Text> ();
        bossName.alignment = TextAnchor.LowerLeft;
        bossName.font = GameVars.MainFont;
        bossName.fontSize = 8;
        bossName.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (140, 30);
        bossName.GetComponent<RectTransform> ().sizeDelta = new Vector2 (GameVars.ScreenWidth, 10);
        tn = bossName.DOText (m_levelConfig.bossName, curCharDuration * m_levelConfig.bossName.Length).SetEase (Ease.Linear);
        yield return new DOTweenCYInstruction.WaitForCompletion (tn);
        yield return new WaitForSecondsRealtime (0.5f);

        // BOSWeapon
        GameObject go1 = new GameObject ("BossWeapon");
        go1.transform.SetParent (this.transform, false);
        Text bossWeapon = go1.AddComponent<Text> ();
        bossWeapon.alignment = TextAnchor.LowerLeft;
        bossWeapon.font = GameVars.MainFont;
        bossWeapon.fontSize = 8;
        bossWeapon.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (140, 0);
        bossWeapon.GetComponent<RectTransform> ().sizeDelta = new Vector2 (GameVars.ScreenWidth, 10);
        tn = bossWeapon.DOText (m_levelConfig.bossWeaponName, curCharDuration * m_levelConfig.bossWeaponName.Length).SetEase (Ease.Linear);
        yield return new DOTweenCYInstruction.WaitForCompletion (tn);
        yield return new WaitForSecondsRealtime (0.5f);

        // BOSSReward
        GameObject go2 = new GameObject ("BossReward");
        go2.transform.SetParent (this.transform, false);
        Text bossReward = go2.AddComponent<Text> ();
        bossReward.alignment = TextAnchor.LowerLeft;
        bossReward.font = GameVars.MainFont;
        bossReward.fontSize = 8;
        bossReward.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (140, -30);
        bossReward.GetComponent<RectTransform> ().sizeDelta = new Vector2 (GameVars.ScreenWidth, 10);
        tn = bossReward.DOText ("$ " + m_levelConfig.bossReward, curCharDuration * m_levelConfig.bossReward.ToString ().Length).SetEase (Ease.Linear);
        yield return new DOTweenCYInstruction.WaitForCompletion (tn);
        yield return new WaitForSecondsRealtime (1.5f);

        Utility.Fade (false);
        yield return new WaitWhile (() => {
            return !Utility.isFadeOver;
        });

        m_OnOver ();
        Destroy (this.gameObject);
    }

}