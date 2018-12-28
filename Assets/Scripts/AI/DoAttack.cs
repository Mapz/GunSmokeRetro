using System.Collections;
using System.Collections.Generic;
using BT;
using UnityEngine;

public abstract class DoAttack : BTAction {

    protected GameObject _target;
    protected Vector3 _destination;
    protected Transform _trans;
    protected List<WeaponsBehavior> _weapons;
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

    protected abstract void UpdateFaceDirection ();

    protected virtual bool CanAttack () {
        return true;
    }

    protected override BTResult Execute () {

        if (CheckDead ()) {
            return BTResult.Ended;
        }
        if (!CanAttack ()) {
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