using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public string RequiredKeyName;
    [SerializeField]Animator animator;
    [SerializeField] BoxCollider triggerCollider;
    public override void InteractItem()
    {
        base.InteractItem();
        Inventory playerInventory = GameObject.Find("Player").GetComponent<Inventory>();
        if (playerInventory.currentlyEquippedItem != null)
        {
            if (playerInventory.nearItem && playerInventory.currentlyEquippedItem.name == RequiredKeyName)
            {
                Debug.Log("Hello, I am door.");
                animator.SetTrigger("DoorOpen");
                triggerCollider.enabled = false;
            }
        }
    }

    public override string Message()
    {
        return base.Message();
    }

    private void OnTriggerEnter(Collider other)
    {
 
            if (other.name == "Player" )
            {
                Inventory inventory = other.GetComponent<Inventory>();
                message = $"Find {RequiredKeyName} to open door.";
                inventory.quededItem = this;
                inventory.nearItem = true;

            if (inventory.currentlyEquippedItem != null)
            {
                if (inventory.currentlyEquippedItem.name == RequiredKeyName)
                {
                    message = "Press[f] to open";
                }
                else
                {
                    message = $"Please equip the {RequiredKeyName} to open door. ";
                }
            }

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
