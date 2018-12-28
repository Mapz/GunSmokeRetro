using System.Collections;
using System.Collections.Generic;
using BT;
using UnityEngine;

public class DoAttackEnemyWindowner : DoAttack {

    private StaticDirection m_direction;
    public DoAttackEnemyWindowner (GameObject target, StaticDirection direct, BTPrecondition precondition = null) : base (target, precondition) {
        m_direction = direct;
    }

    protected override bool CanAttack () {
        Vector3 offset = _destination - _trans.position;
        if (offset.x > 0) {
            return m_direction == StaticDirection.right;
        } else if (offset.x < 0) {
            return m_direction == StaticDirection.left;
        }
        return false;
    }

    protected override void UpdateFaceDirection () {
        Vector3 offset = _destination - _trans.position;
        if (offset.x > 0 && m_direction == StaticDirection.right) {
            if (offset.y > 0) {
                if (offset.x / offset.y > 0.5) {
                    database.GetComponent<Animator> ().SetInteger ("UnitState", (int) UnitState.ShootRight);
                } else if (offset.x / offset.y <= 0.5) {
                    database.GetComponent<Animator> ().SetInteger ("UnitState", (int) UnitState.ShootRightUp);
                }
            } else if (offset.y < 0) {
                if (offset.x > Mathf.Abs (offset.y)) {
                    database.GetComponent<Animator> ().SetInteger ("UnitState", (int) UnitState.ShootRight);
                } else if (offset.x < Mathf.Abs (offset.y)) {
                    database.GetComponent<Animator> ().SetInteger ("UnitState", (int) UnitState.ShootRightDown);
                }
            }
        } else if (offset.x < 0 && m_direction == StaticDirection.left) {
            if (offset.y > 0) {
                if (Mathf.Abs (offset.x) / offset.y > 0.5) {
                    database.GetComponent<Animator> ().SetInteger ("UnitState", (int) UnitState.ShootLeft);
                } else if (Mathf.Abs (offset.x) < offset.y) {
                    database.GetComponent<Animator> ().SetInteger ("UnitState", (int) UnitState.ShootLeftUp);
                }
            } else if (offset.y < 0) {
                if (Mathf.Abs (offset.x) / Mathf.Abs (offset.y) <= 0.5) {
                    database.GetComponent<Animator> ().SetInteger ("UnitState", (int) UnitState.ShootLeft);
                } else if (Mathf.Abs (offset.x) < Mathf.Abs (offset.y)) {
                    database.GetComponent<Animator> ().SetInteger ("UnitState", (int) UnitState.ShootLeftDown);
                }
            }
        }
    }

}