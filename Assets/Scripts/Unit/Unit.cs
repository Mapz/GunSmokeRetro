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

    protected Game game;

    void Awake () {
        game = GameObject.Find ("Game").GetComponent<Game> ();
    }

    protected void Update () {
        UpdatePosition ();
    }

    protected virtual void UpdatePosition () { }
    protected virtual void Die () { }

    void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.tag == "Bullet") {
            if (!this.dead) {
                BulletBehavior bbh = other.gameObject.GetComponent<BulletBehavior> ();
                if (bbh.team != this.team) {
                    this.HP -= bbh.damage;
                    if (this.HP <= 0) {
                        _Die ();
                    }
                    Destroy (other.gameObject);
                }
            } else if (other.gameObject.tag.StartsWith ("Unit")) {

            }
        }

    }

    void _Die () {
        this.dead = true;
        //播放死掉的动画
        animator.SetInteger ("UnitState", (int) UnitState.Die);
        // animator.Play ("Die");
        //停火
        foreach (WeaponsBehavior weapon in weapons) {
            weapon.active = false;
        }
        // GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
        Destroy (GetComponent<Rigidbody2D> ());
        Destroy (GetComponent<Collider2D> ());
        Die ();
        new EnumTimer (() => {
            Destroy (gameObject);
        }, 3).StartTimeout (this);
    }
}