using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class LevelClearUI : MonoBehaviour {

    // 弹孔样式存储 
    //TODO: 修改到配置中
    private static List<List<Vector2>> s_BulletHolesList = new List<List<Vector2>> ();

    static LevelClearUI () {
        List<Vector2> holeList1 = new List<Vector2> ();
        //连续的六个位置  弹孔Pivot 0 1,anchor left top
        int count = 6;
        for (int i = 0; i < count; i++) {
            Vector2 vec = new Vector2 (i * 16, -16 * (count - i - 1));
            holeList1.Add (vec);
        }
        s_BulletHolesList.Add (holeList1);

    }
    public Image m_wanted;
    [SerializeField]
    private Text m_prizeText;
    public Text m_rewardText;
    private LevelConfig m_levelConfig;
    private const string REWARD_TEXT = "PRIZE   $";
    private List<Vector2> m_curBulletHoleList;

    private Sprite m_bulletHoleSprite;

    private float curCharDuration = 0.2f;

    private Action m_callBack;

    void Awake () {
        m_levelConfig = LevelsConfig.GetCurLevelConfig ();
        m_wanted.sprite = Utility.GetWanted (m_levelConfig.wantedPostName);
        m_bulletHoleSprite = Utility.GetOtherSprite ("BulletHole");
        m_prizeText.text = "";
        m_rewardText.text = "";
        m_curBulletHoleList = s_BulletHolesList[UnityEngine.Random.Range (0, s_BulletHolesList.Count)];

    }

    public void Show (Action callBack) {
        m_callBack = callBack;
        StartCoroutine (ShowClearAnim ());
    }

    IEnumerator ShowClearAnim () {
        //FadeIn
        Utility.Fade (true);
        yield return new WaitWhile (() => {
            return !Utility.isFadeOver;
        });
        //WaitFor
        yield return new WaitForSecondsRealtime (2.0f);
        //FireBullet
        foreach (var b_position in m_curBulletHoleList) {
            yield return new WaitForSecondsRealtime (0.3f);
            GameObject bh = new GameObject ("bh");
            bh.transform.SetParent (m_wanted.transform, false);
            Image bhImg = bh.AddComponent<Image> ();
            bhImg.sprite = m_bulletHoleSprite;
            RectTransform bhRt = bhImg.GetComponent<RectTransform> ();
            bhRt.pivot = new Vector2 (0, 1);
            bhRt.anchorMin = new Vector2 (0, 1);
            bhRt.anchorMax = new Vector2 (0, 1);
            bhRt.anchoredPosition = b_position;
            bhImg.SetNativeSize ();
        }
        //WaitFor
        yield return new WaitForSecondsRealtime (2.5f);
        //ShowTextPrinter
        Tweener tn = m_prizeText.DOText (REWARD_TEXT, curCharDuration * REWARD_TEXT.Length).SetEase (Ease.Linear);
        yield return new DOTweenCYInstruction.WaitForCompletion (tn);
        yield return new WaitForSecondsRealtime (0.5f);
        //ShowPrizeMoney
        tn = m_rewardText.DOText (m_levelConfig.bossReward.ToString (), curCharDuration * m_levelConfig.bossReward.ToString ().Length).SetEase (Ease.Linear);
        yield return new DOTweenCYInstruction.WaitForCompletion (tn);
        yield return new WaitForSecondsRealtime (1.5f);
        //闪烁，后处理
        ScreenFlickerEffect sfe = Utility.SetFlicker ();
        yield return new WaitForSecondsRealtime (2.5f);
        Utility.SetFlickerTransParent ();
        yield return new WaitForSecondsRealtime (0.5f);
        Utility.DestroyFlicker ();
        m_callBack ();
        Destroy (gameObject);
    }

}