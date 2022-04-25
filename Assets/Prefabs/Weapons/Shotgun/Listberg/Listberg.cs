using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Listberg : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        SetBulletDamage();
        AmmoType = inventory.SHAmmo;
    }

    public override void OnSwapWeapon()
    {
        AmmoType = inventory.SHAmmo;
    }
    public override void StartFiring()
    {
        base.StartFiring();
    }

    public override void OutOfAmmo()
    {
        base.OutOfAmmo();
    }
    public override void ReloadWeapon()
    {
        AmmoType = inventory.SHAmmo;
        base.ReloadWeapon();
        inventory.SHAmmo = AmmoType;

    }



}
