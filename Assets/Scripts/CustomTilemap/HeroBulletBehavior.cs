using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBulletBehavior : BulletBehavior {

    public float maxRange; //Time as Range
    private Vector3 moveSpeed;
    private float timePassed = 0;

    protected override void _Init () {
        moveSpeed = bulletSpeed * shotAim.normalized;
        this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveSpeed.x, moveSpeed.y);
    }

    void Update () {
        if (!initialized) return;
        UpdateRange ();
        // UpdateMove ();
    }

    void UpdateRange () {
        timePassed += Time.deltaTime;
        if (timePassed >= maxRange) {
            Destroy (this.gameObject);
        }
    }

}