﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBehavior : MonoBehaviour, PauseAble {
    [System.NonSerialized]
    public Team team;
    [System.NonSerialized]
    public float damage;
    protected float bulletSpeed;
    protected Sprite bulletSprite;

    [System.NonSerialized]
    public Vector3 shotAim;
    protected bool initialized = false;
    protected Game game;

    private Vector3 _pauseVelocityBuff;

    protected bool m_isPaused = false;

    private void Awake () {
        game = GameObject.Find ("Game").GetComponent<Game> ();
    }

    public void Init (Team _team, float _damage, float _bulletSpeed, Vector3 _shotAim, Sprite _bulletSprite, Unit holder = null) {
        team = _team;
        damage = _damage;
        bulletSpeed = _bulletSpeed;
        if (Vector3.zero != _shotAim)
            shotAim = _shotAim;
        // 重设Box Collider大小 如果需要的话
        if (_bulletSprite) {
            bulletSprite = _bulletSprite;
            SpriteRenderer sr = this.GetComponent<SpriteRenderer> ();
            sr.sprite = bulletSprite;
            Vector2 S = sr.sprite.bounds.size;
            gameObject.GetComponent<BoxCollider2D> ().size = S;
            gameObject.GetComponent<BoxCollider2D> ().offset = new Vector2 (0, 0);
        }
        _Init (holder);
        initialized = true;
    }

    public void Pause (bool _pause) {
        if (_pause) {
            if (null != GetComponent<Rigidbody2D> ()) {
                _pauseVelocityBuff = GetComponent<Rigidbody2D> ().velocity;
                GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
            }
        } else {
            GetComponent<Rigidbody2D> ().velocity = _pauseVelocityBuff;
        }
        m_isPaused = _pause;

    }

    protected abstract void _Init (Unit holder = null);

    protected void UpdateOutOfScreen () {

        if (!Game.pointInOutterScreen (transform.position)) {
            ObjectMgr<BulletBehavior>.Instance.Destroy (this);
        }

    }
}