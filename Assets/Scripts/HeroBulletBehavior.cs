using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBulletBehavior : BulletBehavior {
    public float maxRange; //Time as Range
    private Vector3 moveSpeed;
    private float timePassed = 0;

    protected override void _Init () {
        moveSpeed = bulletSpeed * shotAim.normalized;
        
    }

    void Update () {
        if (!initialized) return;
        UpdateRange ();
        UpdateMove ();
    }

    void UpdateMove () {
        this.transform.position += moveSpeed * Time.deltaTime;
    }
    void UpdateRange () {
        timePassed += Time.deltaTime;
        if (timePassed >= maxRange) {
            Destroy (this.gameObject);
        }
    }

}