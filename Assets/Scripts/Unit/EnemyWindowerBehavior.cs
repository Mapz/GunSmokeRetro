using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum StaticDirection {
    left,
    right,
    up,
    down,
}

public class EnemyWindowerBehavior : Unit {

    public StaticDirection windownerDirection;
    // Start is called before the first frame update
    void Start () {
        if (windownerDirection == StaticDirection.right) {
            m_animator.SetInteger ("UnitState", (int) UnitState.ShootRightDown);
        } else {
            m_animator.SetInteger ("UnitState", (int) UnitState.ShootLeftDown);
        }
    }

    // Update is called once per frame
    new void Update () {
        if (!m_isDead) {
            base.Update ();
        }
    }

    protected override void SetAnimDied () {
        if (windownerDirection == StaticDirection.right) {
            m_animator.SetInteger ("UnitState", (int) UnitState.Die);
        } else {
            m_animator.SetInteger ("UnitState", (int) UnitState.Die2);
        }
    }

    protected override void Die () {
        DOTweenModuleSprite.DOFade (this.GetComponent<SpriteRenderer> (), 0, 0.3f).SetLoops (3);
        // transform.DOColor (3f, 0.2f).SetRelative ();
    }
}