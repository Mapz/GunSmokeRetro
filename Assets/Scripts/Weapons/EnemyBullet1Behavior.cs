using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet1Behavior : BulletBehavior {

    private Vector3 moveSpeed;

    private void Awake () {
        game = GameObject.Find ("Game").GetComponent<Game> ();
    }

    protected override void _Init (Unit holder) {
        if (game.m_hero) {
            shotAim = game.m_hero.transform.position - holder.transform.position;
            moveSpeed = bulletSpeed * shotAim.normalized;
            this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveSpeed.x, moveSpeed.y);
        } else {
            Destroy (gameObject);
        }
    }

    void Update () {
        UpdateOutOfScreen ();
    }

}