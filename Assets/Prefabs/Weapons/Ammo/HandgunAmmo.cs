using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ammo/Handgum Ammo")]
public class HandgunAmmo : Item
{

    public override void SetCorrectItemCount()
    {
        base.SetCorrectItemCount();
        AmmoCount = playerInventory.PAmmo;
    }

    public override void GiveItemToInventory()
    {
        base.GiveItemToInventory();
        playerInventory.PAmmo += Amount;
        AmmoCount = playerInventory.PAmmo;
    }

    public override int AmmoType()
    {
        playerInventory = GameObject.Find("Player").GetComponent<Inventory>();
        return playerInventory.PAmmo;
    }




}
