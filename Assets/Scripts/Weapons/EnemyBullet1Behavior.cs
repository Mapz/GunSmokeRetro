using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet1Behavior : BulletBehavior {

    private Vector3 moveSpeed;

    protected override void _Init () {
        shotAim = GameObject.Find ("Hero").transform.position - this.transform.position;
        moveSpeed = bulletSpeed * shotAim.normalized;
        this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveSpeed.x, moveSpeed.y);
    }

}