using System.Collections;
using BT;
using UnityEngine;

public class DoAttack : BTAction {

    private string _targetName;
    private Vector3 _destination;
    private float _tolerance = 0.01f;
    private Transform _trans;

    public DoAttack (string targetName) {
        _targetName = targetName;
    }

    public override void Activate (Database database) {
        base.Activate (database);
        _trans = database.transform;
    }

    protected override BTResult Execute () {

        if (CheckDead ()) {
            return BTResult.Ended;
        }
        Attack ();
        return BTResult.Running;
    }

    private void UpdateFaceDirection () {
        Vector3 offset = _destination - _trans.position;
        if (offset.x >= 0) {
            _trans.localEulerAngles = new Vector3 (0, 180, 0);
        } else {
            _trans.localEulerAngles = Vector3.zero;
        }
    }

    private bool CheckDead () {
        return GameObject.Find ("_targetName").GetComponent<Unit> ().dead;
    }

    private void Attack () {
        database.GetComponent<Animator> ().Play ("Walk");
        Vector3 direction = (_destination - _trans.position).normalized;

    }
}