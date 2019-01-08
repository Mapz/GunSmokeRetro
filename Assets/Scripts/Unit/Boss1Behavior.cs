using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Boss1Behavior : Unit {

    public float m_onHitCrawlTime;
    new void Update () {
        if (m_isDead || m_isPause) return;
        base.Update ();
    }

    protected override void Die () {
        transform.DOMoveY (3f, 0.2f).SetRelative ();
        Game game = GameObject.Find ("Game").GetComponent<Game> ();
        game.SetGameState (GameState.GameOver);
    }

    protected override void OnHit () {
        GetComponent<BoxCollider2D> ().enabled = false;
        m_animator.SetInteger ("UnitState", (int) UnitState.CrawlDown);
        new EnumTimer (() => {
            if (GetComponent<BoxCollider2D> ()) {
                GetComponent<BoxCollider2D> ().enabled = true;
            }
            m_animator.SetInteger ("UnitState", (int) UnitState.WalkDown);
        }, m_onHitCrawlTime).StartTimeout (this);
    }
}