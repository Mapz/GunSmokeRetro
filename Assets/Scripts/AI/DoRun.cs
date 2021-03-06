using System.Collections;
using System.Collections.Generic;
using BT;
using UnityEngine;

public class DoRun : BTAction {

    private GameObject _target;
    private Vector3 _destination;

    private Transform _trans;

    private float _distance;

    public DoRun (GameObject target, float distance, BTPrecondition precondition = null) : base (precondition) {
        _target = target;
        _distance = distance;
    }

    public override void Activate (Database database) {
        base.Activate (database);
        _trans = database.transform;
    }

    protected override BTResult Execute () {

        if (CheckDead ()) {
            return BTResult.Ended;
        }
        UpdateDestination ();
        UpdateFaceDirection ();

        if (CheckArrived ()) {
            return BTResult.Ended;
        }
        MoveToDestination ();
        return BTResult.Running;
    }

    private void UpdateDestination () {
        _destination = _target.transform.position;
    }

    private void UpdateFaceDirection () {
        if (database.GetComponent<Animator> ().GetInteger ("UnitState") == (int) UnitState.CrawlDown) return;
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

    private bool CheckDead () {
        if (_target && database.GetComponent<Unit> ())
            return _target.GetComponent<Unit> ().m_isDead || database.GetComponent<Unit> ().m_isDead;
        return true;
    }

    private bool CheckArrived () {
        Vector3 offset = _destination - _trans.position;
        return offset.sqrMagnitude < _distance * _distance;
    }

    private void MoveToDestination () {
        Vector3 direction = (_destination - _trans.position).normalized;
        database.GetComponent<Unit> ().SetVelocity (direction * database.GetComponent<Unit> ().m_moveSpeed);
    }
}