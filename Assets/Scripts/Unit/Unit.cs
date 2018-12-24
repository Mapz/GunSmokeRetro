using System;
using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour {
    public float HP = 5;
    public float moveSpeed = 35;
    public Animator animator;
    public List<WeaponsBehavior> weapons;
    public Team team;
    public bool dead = false;
    // Start is called before the first frame update
    protected void Awake () {

    }

    // Update is called once per frame
    protected void Update () {
        UpdatePosition ();
    }

    protected abstract void UpdatePosition ();

    void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.tag == "Bullet") {
            BulletBehavior bbh = other.gameObject.GetComponent<BulletBehavior> ();
            if (bbh.team != this.team) {
                this.HP -= bbh.damage;
                if (this.HP <= 0) {
                    animator.SetInteger ("UnitState", (int) UnitState.Die);
                    Die ();
                } else {
                    Destroy (other.gameObject);
                }
            }
        } else if (other.gameObject.tag.StartsWith ("Unit")) {

        }
    }

    void Die () {
        new EnumTimer (() => {
            Destroy (gameObject);
        }, 3).StartTimeout (this);
    }

}