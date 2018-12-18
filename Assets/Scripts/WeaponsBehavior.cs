using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeaponShot {
    public Vector3 shotPos;
    public Vector3 shotAim;
    public GameObject bulletObject;
    public int shotDamage;
    public float bulletSpeed;
    public Sprite bulletSprite;

}

public abstract class WeaponsBehavior : MonoBehaviour {
    public Unit holdingWeapon;
    public float coolDown;
    private float _coolDown;
    public bool active;
    public List<WeaponShot> WeaponShots;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update () {
        _coolDown -= Time.deltaTime;
        if (active && _coolDown <= 0) {
            Fire ();
            _coolDown = coolDown;
        }
    }

    protected abstract void Fire ();
}