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
        if (dead) return;
        base.Update ();
        UpdateFire ();
    }

    protected override void UpdatePosition () {
        positionToMove = this.transform.position;
        if (Input.GetKey ("w")) {
            positionToMove += Vector3.up * this.moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey ("s")) {
            positionToMove += Vector3.down * this.moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey ("a")) {
            positionToMove += Vector3.left * this.moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey ("d")) {
            positionToMove += Vector3.right * this.moveSpeed * Time.deltaTime;
        }

        // 主角不能出界
        if (!game.pointInScreen (positionToMove, ref screenPositionBuff)) {
            positionToMove = screenPositionBuff;
        }
    

        heroRigid.MovePosition (positionToMove);
    }

    void UpdateFire () {
        if (Input.GetKey ("k")) {
            animator.SetInteger ("UnitState", (int) UnitState.ShootUp);
            weapons[0].active = true;
            weapons[1].active = false;
            weapons[2].active = false;
        } else if (Input.GetKey ("j")) {
            animator.SetInteger ("UnitState", (int) UnitState.ShootLeft);
            weapons[0].active = false;
            weapons[1].active = true;
            weapons[2].active = false;
        } else if (Input.GetKey ("l")) {
            animator.SetInteger ("UnitState", (int) UnitState.ShootRight);
            weapons[0].active = false;
            weapons[1].active = false;
            weapons[2].active = true;
        } else {
            animator.SetInteger ("UnitState", (int) UnitState.WalkUp);
            weapons[0].active = false;
            weapons[1].active = false;
            weapons[2].active = false;
        }
    }

}