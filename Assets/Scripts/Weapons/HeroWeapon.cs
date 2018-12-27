using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroWeapon : WeaponsBehavior {

    protected override void Fire () {
        foreach (WeaponShot shot in WeaponShots) {
            BulletBehavior bbh = ObjectMgr<BulletBehavior>.Instance.Create (() => Instantiate (shot.bulletObject).GetComponent<BulletBehavior> ());
            bbh.transform.position = this.transform.position + shot.shotPos;
            bbh.Init (this.holdingWeapon.m_team, shot.shotDamage, shot.bulletSpeed, shot.shotAim, shot.bulletSprite);
        }
    }
}