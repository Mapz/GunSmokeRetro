using System.Collections;
using BT;
using UnityEngine;

public class CheckBehindTarget : BTPrecondition {

    private GameObject _target;
    private Transform _trans;

    public CheckBehindTarget (GameObject target) {

        _target = target;
    }

    public override void Activate (Database database) {
        base.Activate (database);
        _trans = database.transform;
    }

    public override bool Check () {
        if (_target == null) return false;
        return _target.transform.position.y > _trans.position.y;
    }
}