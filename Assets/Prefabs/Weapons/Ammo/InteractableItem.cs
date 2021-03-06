using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteractableItem : Interactable
{

    
    public override void InteractItem()
    {
        base.InteractItem();
        Debug.Log("We reached here.");
        Inventory playerInventory = GameObject.Find("Player").GetComponent<Inventory>();
        if (playerInventory.UtilityBelt().Count < 6)
        {
            if (playerInventory.nearItem)
            {

                playerInventory.UtilityBelt().Add(item);
                item.GiveItemToInventory();
                playerInventory.UpdateUtility();

                if(playerInventory.currentlyEquippedItem == null)
                {
                    playerInventory.currentlyEquippedItem = item;
                }

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
