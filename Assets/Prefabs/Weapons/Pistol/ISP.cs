using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISP : Weapon
{

    private void Start()
    {
        SetBulletDamage(); //this will be moved to methods like swapping weapons, upgrading weapons etc. Here for now.
        AmmoType = inventory.PAmmo;
    }
    public override void StartFiring()
    {
        base.StartFiring();
    }

    public override void OnSwapWeapon()
    {
        AmmoType = inventory.PAmmo;
    }


    public override void ReloadWeapon()
    {
        AmmoType = inventory.PAmmo;
        base.ReloadWeapon();
        inventory.PAmmo = AmmoType;
        
    }


    public override void OutOfAmmo()
    {
        base.OutOfAmmo();
    }
}
