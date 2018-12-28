using System;
using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour, PauseAble {
    public float m_HP = 5;
    public float m_moveSpeed = 35;
    public Animator m_animator;
    public List<WeaponsBehavior> m_weapons;
    public Team m_team;
    protected Game game;
    private Vector3 _pauseVelocityBuff;
    protected bool m_isPause = false;
    public bool m_isDead = false;
    public Vector3 m_positionFffsetOnCreate;

    void Awake () {
        game = GameObject.Find ("Game").GetComponent<Game> ();

    }

    protected void Update () {
        if (m_isPause) return;
        UpdatePosition ();
        CheckOutOffOutterScreen();
    }

    private void CheckOutOffOutterScreen () {
        if (!Game.pointInOutterScreen (transform.position)) {
            ObjectMgr<Unit>.Instance.Destroy (this);
        }
    }

    protected virtual void UpdatePosition () { }
    protected virtual void Die () { }

    void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.tag == "Bullet") {
            if (!this.m_isDead) {
                BulletBehavior bbh = other.gameObject.GetComponent<BulletBehavior> ();
                if (bbh.team != this.m_team) {
                    this.m_HP -= bbh.damage;
                    if (this.m_HP <= 0) {
                        _Die ();
                    }
                    Destroy (other.gameObject);
                }
            } else if (other.gameObject.tag.StartsWith ("Unit")) {

            }
        }

    }

    public void Pause (bool _pause) {
        if (_pause) {
            foreach (WeaponsBehavior weapon in m_weapons) {
                weapon.active = false;
            }
            if (null != GetComponent<Rigidbody2D> ()) {
                _pauseVelocityBuff = GetComponent<Rigidbody2D> ().velocity;
                GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
            }
            if (null != GetComponent<BTTree> ()) {
                GetComponent<BTTree> ().isRunning = false;
            }
        } else {
            if (null != GetComponent<Rigidbody2D> ()) {
                GetComponent<Rigidbody2D> ().velocity = _pauseVelocityBuff;
            }
            if (null != GetComponent<BTTree> ()) {
                GetComponent<BTTree> ().isRunning = true;
            }
        }
        m_isPause = _pause;

    }

    protected virtual void SetAnimDied () {
        m_animator.SetInteger ("UnitState", (int) UnitState.Die);
    }

    void _Die () {
        this.m_isDead = true;
        //播放死掉的动画
        SetAnimDied ();
        // animator.Play ("Die");
        //停火
        foreach (WeaponsBehavior weapon in m_weapons) {
            weapon.active = false;
        }
        // GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
        Destroy (GetComponent<Rigidbody2D> ());
        Destroy (GetComponent<Collider2D> ());
        Die ();
        new EnumTimer (() => {
            ObjectMgr<Unit>.Instance.Destroy (this);
        }, 3).StartTimeout (this);
    }
}