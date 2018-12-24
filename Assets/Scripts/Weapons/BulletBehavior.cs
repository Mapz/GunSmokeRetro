using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBehavior : MonoBehaviour {
    [System.NonSerialized]
    public Team team;
    [System.NonSerialized]
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
        // 重设Box Collider大小
        SpriteRenderer sr = this.GetComponent<SpriteRenderer> ();
        sr.sprite = bulletSprite;
        Vector2 S = sr.sprite.bounds.size;
        gameObject.GetComponent<BoxCollider2D> ().size = S;
        gameObject.GetComponent<BoxCollider2D> ().offset = new Vector2 (0, 0);
        _Init ();
        initialized = true;
    }

    protected abstract void _Init ();

}