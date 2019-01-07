using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRiffle : WeaponsBehavior {
    public int m_shotPerFire;
    public float m_riffleShotCooldown;

    protected override void Fire () {
        foreach (WeaponShot shot in WeaponShots) {
            new EnumTimer (
                () => {
                    if (!Game.m_isPaused) {
                        BulletBehavior bbh = ObjectMgr<BulletBehavior>.Instance.Create (() => Instantiate (shot.bulletObject).GetComponent<BulletBehavior> ());
                        bbh.transform.parent = GameObject.Find ("Game").GetComponent<Game> ().m_level.transform;
                        bbh.Init (this.holdingWeapon.m_team, shot.shotDamage, shot.bulletSpeed, shot.shotAim, shot.bulletSprite, holdingWeapon);
                        bbh.transform.position = this.holdingWeapon.transform.position + bbh.shotAim.normalized * 8;
                    }
                },
                m_riffleShotCooldown,
                m_shotPerFire,
                () => { return Game.m_isPaused; }
            ).StartTimeout (this);
        }
    }
}