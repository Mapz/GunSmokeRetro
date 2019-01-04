using System.Collections;
using System.Collections.Generic;
using BT;
using UnityEngine;
using UnityEngine.U2D;
public class DoRunBack : BTAction {

    private Vector3 _destination;

    private Transform _trans;

    private float _distanceToUpScreen;

    public DoRunBack (float distanceToUpScreen, BTPrecondition precondition = null) : base (precondition) {

        _distanceToUpScreen = distanceToUpScreen;

    }

    public override void Activate (Database database) {
        base.Activate (database);
        _trans = database.transform;
        _destination = new Vector3 (_trans.position.x, GameVars.ppCamera.refResolutionY / 2 - _distanceToUpScreen, 0);
    }

    protected override BTResult Execute () {

        if (CheckDead ()) {
            return BTResult.Ended;
        }

        UpdateFaceDirection ();

        if (CheckArrived ()) {
            return BTResult.Ended;
        }
        MoveToDestination ();
        return BTResult.Running;
    }

    private void UpdateFaceDirection () {

    }

    private bool CheckDead () {
        if (database.GetComponent<Unit> ())
            return database.GetComponent<Unit> ().m_isDead;
        return true;
    }

    private bool CheckArrived () {
        Vector3 offset = _destination - _trans.position;
        // Debug.Log (offset);
        return offset.sqrMagnitude < 1f;
    }

    private void MoveToDestination () {
        Vector3 direction = (_destination - _trans.position).normalized;
        database.GetComponent<Unit> ().SetVelocity (direction * database.GetComponent<Unit> ().m_moveSpeed);
    }
}