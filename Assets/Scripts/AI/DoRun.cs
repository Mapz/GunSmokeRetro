using System.Collections;
using System.Collections.Generic;
using BT;
using UnityEngine;

public class DoRun : BTAction {

    private GameObject _target;
    private Vector3 _destination;

    private float _tolerance = 0.01f;
    private Transform _trans;

    public DoRun (GameObject target, BTPrecondition precondition = null) : base (precondition) {
        _target = target;
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
            return _target.GetComponent<Unit> ().dead || database.GetComponent<Unit> ().dead;
        return true;
    }

    private bool CheckArrived () {
        Vector3 offset = _destination - _trans.position;
        return offset.sqrMagnitude < _tolerance * _tolerance;
    }

    private void MoveToDestination () {
        Vector3 direction = (_destination - _trans.position).normalized;
        database.GetComponent<Rigidbody2D> ().velocity = direction * database.GetComponent<Unit> ().moveSpeed;
    }
}