using System.Collections;
using BT;
using UnityEngine;

public class CheckInSight : BTPrecondition {
    private float _sightLength;
    private GameObject _target;

    private Transform _trans;

    public CheckInSight (float sightLength, GameObject target) {
        _sightLength = sightLength;
        _target = target;
    }

    public override void Activate (Database database) {
        base.Activate (database);
        _trans = database.transform;
    }

    public override bool Check () {
        if (_target == null) return false;
        Vector3 offset = _target.transform.position - _trans.position;
        return offset.sqrMagnitude <= _sightLength * _sightLength;
    }
}