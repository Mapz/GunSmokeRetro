using System.Collections;
using System.Collections.Generic;
using BT;
using UnityEngine;

public class DoKeepDistanceAround : BTAction {

    private GameObject _target;

    private float _tolerance = 0.01f;
    private Transform _trans;

    private float _distance;

    public DoKeepDistanceAround (GameObject target, float distance, BTPrecondition precondition = null) : base (precondition) {
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
        if (CheckDistance ()) {
            return BTResult.Ended;
        }
        UpdateFaceDirection ();
        MoveToDestination ();
        return BTResult.Running;
    }

    private void UpdateFaceDirection () {
        if (database.GetComponent<Animator> ().GetInteger ("UnitState") == (int) UnitState.CrawlDown) return;
        Vector3 offset = _target.transform.position - _trans.position;
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

    private bool CheckDistance () {
        // Debug.Log ("Distance:" + (_target.transform.position - _trans.position).sqrMagnitude);
        return (_target.transform.position - _trans.position).sqrMagnitude >= _distance * _distance;
    }

    private bool CheckDead () {
        if (_target && database.GetComponent<Unit> ())
            return _target.GetComponent<Unit> ().m_isDead || database.GetComponent<Unit> ().m_isDead;
        return true;
    }

    private void MoveToDestination () {
        Vector3 direction = -(_target.transform.position - _trans.position).normalized;
        // Debug.Log ("Direction:" + direction);
        database.GetComponent<Unit> ().SetVelocity (direction * database.GetComponent<Unit> ().m_moveSpeed);
    }
}