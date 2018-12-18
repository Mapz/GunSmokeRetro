using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBehavior : Unit {

    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    new void Update () {
        if (died) return;
        base.Update ();
        UpdateFire ();
    }

    protected override void UpdatePosition () {
        if (Input.GetKey ("w")) {
            transform.position += Vector3.up * this.moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey ("s")) {
            transform.position += Vector3.down * this.moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey ("a")) {
            transform.position += Vector3.left * this.moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey ("d")) {
            transform.position += Vector3.right * this.moveSpeed * Time.deltaTime;
        }
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