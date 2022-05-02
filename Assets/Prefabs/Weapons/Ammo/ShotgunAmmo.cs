using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ammo/Shotgun Ammo")]
public class ShotgunAmmo : Item
{
    public override void SetCorrectItemCount()
    {
        base.SetCorrectItemCount();
        AmmoCount = playerInventory.SHAmmo;
    }

    public override void GiveItemToInventory()
    {
        base.GiveItemToInventory();
        playerInventory.SHAmmo += Amount;
        AmmoCount = playerInventory.SHAmmo;
    }

    public override int AmmoType()
    {
        playerInventory = GameObject.Find("Player").GetComponent<Inventory>();
        return playerInventory.SHAmmo;
    }
}
