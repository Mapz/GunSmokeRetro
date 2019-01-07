using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum UnitMoveType {
    ByVelocity,
    ByImpulse,
}

public abstract class Unit : MonoBehaviour, PauseAble {

    [SerializeField]
    private float _HP = 5;
    public float m_HP {
        get { return _HP; } set {
            if (GameVars.Game.m_state != GameState.InGame) return;
            _HP = value;
            if (null != m_HPBar && _HP >= 0) {
                m_HPBar.SetHP ((int) _HP);
            }
        }
    }
    public float m_moveSpeed = 35;
    public int m_money = 100;
    protected Animator m_animator;
    public List<WeaponsBehavior> m_weapons;
    public Team m_team;
    protected Game game;
    private Vector2 _pauseVelocityBuff;
    protected bool m_isPause = false;
    public bool m_isDead = false;
    public Vector3 m_positionFffsetOnCreate;
    public UnitMoveType m_moveType = UnitMoveType.ByVelocity;
    private Tweener m_moveSpeedTweener;
    public BossHPBar m_HPBar;

    void Awake () {
        game = GameObject.Find ("Game").GetComponent<Game> ();
        m_animator = GetComponent<Animator> ();
    }

    protected void Update () {
        if (m_isPause) return;
        UpdatePosition ();
        CheckOutOffOutterScreen ();
    }

    private void CheckOutOffOutterScreen () {
        if (!Game.pointInOutterScreen (transform.position)) {
            ObjectMgr<Unit>.Instance.Destroy (this);
        }
    }

    protected virtual void UpdatePosition () {

    }

    protected virtual void OnHit () {

    }

    protected virtual void Die () { }

    void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.tag == "Bullet") {
            if (!m_isDead) {
                BulletBehavior bbh = other.gameObject.GetComponent<BulletBehavior> ();
                if (bbh.team != m_team) {
                    m_HP -= bbh.damage;
                    if (m_HP <= 0) {
                        _Die ();
                    } else {
                        OnHit ();
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
                SetVelocity (Vector3.zero);
            }
            if (null != GetComponent<BTTree> ()) {
                GetComponent<BTTree> ().isRunning = false;
            }
        } else {
            if (null != GetComponent<Rigidbody2D> ()) {
                SetVelocity (_pauseVelocityBuff);
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

    public void SetVelocity (Vector3 moveSpeed) {
        if (m_moveType == UnitMoveType.ByVelocity) {
            GetComponent<Rigidbody2D> ().velocity = moveSpeed;
        } else if (m_moveType == UnitMoveType.ByImpulse) {

        }
    }

    void _Die () {
        m_isDead = true;
        //播放死掉的动画
        SetAnimDied ();
        //停火
        foreach (WeaponsBehavior weapon in m_weapons) {
            weapon.active = false;
        }
        if (m_moveSpeedTweener != null) {
            DOTween.Kill (m_moveSpeedTweener);
        }
        // 加钱
        if (GameVars.Game.m_state == GameState.InGame) {
            GameVars.money += m_money;
        }
        Destroy (GetComponent<Rigidbody2D> ());
        Destroy (GetComponent<Collider2D> ());
        Die ();
        new EnumTimer (() => {
            ObjectMgr<Unit>.Instance.Destroy (this);
        }, 3).StartTimeout (this);
    }
}