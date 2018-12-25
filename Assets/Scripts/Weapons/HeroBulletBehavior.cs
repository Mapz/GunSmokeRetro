using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBulletBehavior : BulletBehavior {

    public float maxRange; //Time as Range
    private Vector3 moveSpeed;
    private float timePassed = 0;
    private float timeRange;

    protected override void _Init (Unit holder) {
        moveSpeed = bulletSpeed * shotAim.normalized;
        this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveSpeed.x, moveSpeed.y);
        timeRange = maxRange / moveSpeed.magnitude;
    }

    void Update () {
        if (!initialized) return;
        UpdateRange ();
        UpdateOutOfScreen ();
    }

    void UpdateRange () {
        timePassed += Time.deltaTime;
        if (timePassed >= timeRange) {
            Destroy (this.gameObject);
        }
    }

}