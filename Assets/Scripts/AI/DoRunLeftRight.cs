using System.Collections;
using System.Collections.Generic;
using BT;
using UnityEngine;
public class DoRunLeftRight : BTAction {


    private Transform _trans;


    public DoRunLeftRight ( BTPrecondition precondition = null) : base (precondition) {
    }

    public override void Activate (Database database) {
        base.Activate (database);
        _trans = database.transform;
    }

    protected override BTResult Execute () {

        if (CheckDead ()) {
            return BTResult.Ended;
        }

        UpdateFaceDirection ();

        Move ();
        return BTResult.Running;
    }

    private void UpdateFaceDirection () {

    }

    private bool CheckDead () {
        if (database.GetComponent<Unit> ())
            return database.GetComponent<Unit> ().m_isDead;
        return true;
    }


    private void Move () {
        database.GetComponent<Rigidbody2D> ().velocity = Vector3.right * Random.Range(-1,1) * database.GetComponent<Unit> ().m_moveSpeed;
    }
}