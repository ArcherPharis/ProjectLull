using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISP : Weapon
{

    private void Start()
    {
        SetBulletDamage(); //this will be moved to methods like swapping weapons, upgrading weapons etc. Here for now.
    }
    public override void StartFiring()
    {
        base.StartFiring();
    }
    public override void ReloadWeaponAmmoType(int ammoType)
    {
        base.ReloadWeaponAmmoType(ammoType);
    }


    public override void OutOfAmmo()
    {
        base.OutOfAmmo();
    }
}
