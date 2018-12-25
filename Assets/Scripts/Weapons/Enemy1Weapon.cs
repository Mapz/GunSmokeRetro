using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Weapon : WeaponsBehavior {

    protected override void Fire () {
        foreach (WeaponShot shot in WeaponShots) {
            var bulletObject = Instantiate (shot.bulletObject) as GameObject;
            BulletBehavior bbh = bulletObject.GetComponent<BulletBehavior> ();
            bbh.Init (this.holdingWeapon.team, shot.shotDamage, shot.bulletSpeed, shot.shotAim, shot.bulletSprite,holdingWeapon);
            // int facing = holdingWeapon.animator.GetInteger("UnitState");
            bulletObject.transform.position = this.holdingWeapon.transform.position + bbh.shotAim.normalized * 8;
        }
    }
}