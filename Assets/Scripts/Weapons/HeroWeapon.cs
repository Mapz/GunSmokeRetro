using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroWeapon : WeaponsBehavior {

    protected override void Fire () {
        foreach (WeaponShot shot in WeaponShots) {
            BulletBehavior bbh = BulletMgr.CreateBullet (() => Instantiate (shot.bulletObject).GetComponent<BulletBehavior> ());
            bbh.transform.position = this.transform.position + shot.shotPos;
            bbh.Init (this.holdingWeapon.team, shot.shotDamage, shot.bulletSpeed, shot.shotAim, shot.bulletSprite);
        }
    }
}