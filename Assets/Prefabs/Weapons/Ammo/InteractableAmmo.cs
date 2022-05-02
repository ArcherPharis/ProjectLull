using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteractableAmmo : Interactable
{
    public override void InteractItem()
    {
        base.InteractItem();
        Debug.Log("We reached here.");
        Inventory playerInventory = GameObject.Find("Player").GetComponent<Inventory>();

        if (playerInventory.nearItem)
        {

            if (playerInventory.AmmoPouch().Any(x => x.name == item.name)) //TODO we need to add on max stack functionality.
            {//cont. so if the item also has not reached maxed capacity, it won't add or give ammo.
                Debug.Log("Item already exists in inventory.");
                item.GiveItemToInventory();
                playerInventory.UpdateAmmoAmount();
                Destroy(gameObject);
            }
            else
            {
                playerInventory.AmmoPouch().Add(item);
                item.GiveItemToInventory();
                playerInventory.UpdateAmmoAmount();
                Destroy(gameObject);
            }

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            Inventory inventory = other.GetComponent<Inventory>();
            inventory.quededItem = this;
            inventory.nearItem = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Inventory inventory = other.GetComponent<Inventory>();
            inventory.quededItem = null;
            inventory.nearItem = false;
        }
    }
}
