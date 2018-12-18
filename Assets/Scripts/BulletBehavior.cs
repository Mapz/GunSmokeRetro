using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBehavior : MonoBehaviour {
    public Team team;
    public float damage;
    protected float bulletSpeed;
    protected Sprite bulletSprite;

    protected Vector3 shotAim;
    protected bool initialized = false;

    public void Init (Team _team, float _damage, float _bulletSpeed, Vector3 _shotAim, Sprite _bulletSprite) {
        team = _team;
        damage = _damage;
        bulletSpeed = _bulletSpeed;
        bulletSprite = _bulletSprite;
        shotAim = _shotAim;
        _Init ();
        initialized = true;
    }

    protected abstract void _Init ();

}