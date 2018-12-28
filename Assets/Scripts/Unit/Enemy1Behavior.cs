using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Enemy1Behavior : Unit {

    void Awake () {
        m_animator.SetInteger ("UnitState", (int) UnitState.WalkDown);
    }

    new void Update () {
        if (m_isDead || m_isPause) return;
        base.Update ();
    }

    protected override void Die () {
        transform.DOMoveY (3f, 0.2f).SetRelative ();
    }
}