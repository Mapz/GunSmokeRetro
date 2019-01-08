using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InGameUI : MonoBehaviour {
    public Text m_moneyText;
    public BossHPBar m_bossHPBar;

    void Awake () {
        m_moneyText.gameObject.SetActive (false);
        m_bossHPBar.gameObject.SetActive (false);
        GameVars.MoneyText = m_moneyText;
        GameVars.BossHPBar = m_bossHPBar;
        GameVars.InGameUI = this;
    }

    public void OnlyShowMoney () {
        m_moneyText.gameObject.SetActive (true);
        m_bossHPBar.gameObject.SetActive (false);
    }

    public void BossAppear () {
        m_bossHPBar.gameObject.SetActive (true);
        m_moneyText.gameObject.SetActive (false);
    }

    public void HideAll () {
        m_moneyText.gameObject.SetActive (false);
        m_bossHPBar.gameObject.SetActive (false);
    }

}