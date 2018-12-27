using System.Collections;
using System.Collections.Generic;
using BT;
using UnityEngine;

public class DoAttack : BTAction {

    private GameObject _target;

    private Vector3 _destination;
    private Transform _trans;
    private List<WeaponsBehavior> _weapons;
    public DoAttack (GameObject target, BTPrecondition precondition = null) : base (precondition) {
        _target = target;
    }
    public override void Activate (Database database) {
        base.Activate (database);
        _trans = database.transform;
        _weapons = database.GetComponent<Unit> ().m_weapons;
    }

    private void UpdateDestination () {
        _destination = _target.transform.position;
    }

    private void UpdateFaceDirection () {
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
    protected override BTResult Execute () {

        if (CheckDead ()) {
            return BTResult.Ended;
        }
        UpdateDestination ();
        UpdateFaceDirection ();
        Attack ();
        return BTResult.Running;
    }
    protected override void Exit () {
        if (_weapons.Count > 0) {
            _weapons[0].active = false;
        }
    }
    private bool CheckDead () {
        if (_target && database.GetComponent<Unit> ())
            return _target.GetComponent<Unit> ().m_isDead || database.GetComponent<Unit> ().m_isDead;
        return true;
    }
    private void Attack () {
        if (_weapons.Count > 0) {
            _weapons[0].active = true;
        }
    }
}