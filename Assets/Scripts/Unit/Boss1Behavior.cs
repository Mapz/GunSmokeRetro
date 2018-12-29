using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Boss1Behavior : Unit {


    new void Update () {
        if (m_isDead || m_isPause) return;
        base.Update ();
    }

    protected override void Die () {
        transform.DOMoveY (3f, 0.2f).SetRelative ();

        //TODO:BossDieFunc
    }
}