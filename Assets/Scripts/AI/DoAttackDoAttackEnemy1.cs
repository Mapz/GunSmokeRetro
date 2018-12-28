using System.Collections;
using System.Collections.Generic;
using BT;
using UnityEngine;

public class DoAttackDoAttackEnemy1 : DoAttack {

    public DoAttackDoAttackEnemy1 (GameObject target, BTPrecondition precondition = null) : base (target, precondition) {

    }

    protected override void UpdateFaceDirection () {
        Vector3 offset = _destination - _trans.position;
        if (offset.x > 0) {
            if (offset.y > 0) {
                if (offset.x > offset.y) {
                    database.GetComponent<Animator> ().SetInteger ("UnitState", (int) UnitState.WalkRight);
                } else if (offset.x < offset.y) {
                    database.GetComponent<Animator> ().SetInteger ("UnitState", (int) UnitState.WalkUp);
                }
            } else if (offset.y < 0) {
                if (offset.x > Mathf.Abs (offset.y)) {
                    database.GetComponent<Animator> ().SetInteger ("UnitState", (int) UnitState.WalkRight);
                } else if (offset.x < Mathf.Abs (offset.y)) {
                    database.GetComponent<Animator> ().SetInteger ("UnitState", (int) UnitState.WalkDown);
                }
            }
        } else if (offset.x < 0) {
            if (offset.y > 0) {
                if (Mathf.Abs (offset.x) > offset.y) {
                    database.GetComponent<Animator> ().SetInteger ("UnitState", (int) UnitState.WalkLeft);
                } else if (Mathf.Abs (offset.x) < offset.y) {
                    database.GetComponent<Animator> ().SetInteger ("UnitState", (int) UnitState.WalkUp);
                }
            } else if (offset.y < 0) {
                if (Mathf.Abs (offset.x) > Mathf.Abs (offset.y)) {
                    database.GetComponent<Animator> ().SetInteger ("UnitState", (int) UnitState.WalkLeft);
                } else if (Mathf.Abs (offset.x) < Mathf.Abs (offset.y)) {
                    database.GetComponent<Animator> ().SetInteger ("UnitState", (int) UnitState.WalkDown);
                }
            }
        }
    }

}