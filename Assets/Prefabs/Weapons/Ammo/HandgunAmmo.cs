using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ammo/Handgum Ammo")]
public class HandgunAmmo : Item
{


    public override void GiveItemToInventory()
    {
        base.GiveItemToInventory();
        playerInventory.PAmmo += Amount;
    }



}
