using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyBehavior : Unit {

    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    new void Update () {
        if (!m_isDead) {
            base.Update ();
        }
    }

    protected override void Die () {
        transform.DOMoveY (3f, 0.2f).SetRelative();
    }
}