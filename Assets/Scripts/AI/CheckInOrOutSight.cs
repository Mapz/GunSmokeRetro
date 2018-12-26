using System.Collections;
using BT;
using UnityEngine;

public class CheckInOrOutSight : BTPrecondition {
    private float _sightLength;
    private GameObject _target;
    private Transform _trans;
    private bool _isIn;

    public CheckInOrOutSight (float sightLength, GameObject target, bool isIn) {
        _sightLength = sightLength;
        _target = target;
        _isIn = isIn;
    }

    public override void Activate (Database database) {
        base.Activate (database);
        _trans = database.transform;
    }

    public override bool Check () {
        if (_target == null) return false;
        Vector3 offset = _target.transform.position - _trans.position;
        if (_isIn)
            return offset.sqrMagnitude <= _sightLength * _sightLength;
        else {
            return offset.sqrMagnitude > _sightLength * _sightLength;
        }
    }
}