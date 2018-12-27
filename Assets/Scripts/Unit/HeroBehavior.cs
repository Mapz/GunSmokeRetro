using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HeroBehavior : Unit {

    private Rigidbody2D heroRigid;

    private Vector3 positionToMove;

    private Vector3 screenPositionBuff;

    // Start is called before the first frame update
    void Start () {
        heroRigid = this.GetComponent<Rigidbody2D> ();
    }

    // Update is called once per frame
    new void Update () {
        if (m_isDead || m_isPause) return;
        base.Update ();
        UpdateFire ();
    }

    protected override void UpdatePosition () {
        positionToMove = this.transform.position;
        if (Input.GetKey ("w")) {
            positionToMove += Vector3.up * this.m_moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey ("s")) {
            positionToMove += Vector3.down * this.m_moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey ("a")) {
            positionToMove += Vector3.left * this.m_moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey ("d")) {
            positionToMove += Vector3.right * this.m_moveSpeed * Time.deltaTime;
        }

        // 主角不能出界
        if (!Game.pointInScreen (positionToMove, ref screenPositionBuff)) {
            positionToMove = screenPositionBuff;
        }

        heroRigid.MovePosition (positionToMove);
    }

    void UpdateFire () {
        if (Input.GetKey ("k")) {
            m_animator.SetInteger ("UnitState", (int) UnitState.ShootUp);
            m_weapons[0].active = true;
            m_weapons[1].active = false;
            m_weapons[2].active = false;
        } else if (Input.GetKey ("j")) {
            m_animator.SetInteger ("UnitState", (int) UnitState.ShootLeft);
            m_weapons[0].active = false;
            m_weapons[1].active = true;
            m_weapons[2].active = false;
        } else if (Input.GetKey ("l")) {
            m_animator.SetInteger ("UnitState", (int) UnitState.ShootRight);
            m_weapons[0].active = false;
            m_weapons[1].active = false;
            m_weapons[2].active = true;
        } else {
            m_animator.SetInteger ("UnitState", (int) UnitState.WalkUp);
            m_weapons[0].active = false;
            m_weapons[1].active = false;
            m_weapons[2].active = false;
        }
    }

    protected override void Die () {
        game.SetGameState (GameState.HeroFail);
    }

}