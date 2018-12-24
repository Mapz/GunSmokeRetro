using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Weapon : WeaponsBehavior {

    protected override void Fire () {
        foreach (WeaponShot shot in WeaponShots) {
            var bulletObject = Instantiate (shot.bulletObject) as GameObject;
            bulletObject.transform.position = this.transform.position + shot.shotPos;
            BulletBehavior bbh = bulletObject.GetComponent<BulletBehavior> ();
            bbh.Init (this.holdingWeapon.team, shot.shotDamage, shot.bulletSpeed, shot.shotAim, shot.bulletSprite);
        }
    }
}