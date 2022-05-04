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
        if (playerInventory.nearItem && playerInventory.currentlyEquippedItem.name == RequiredKeyName)
        {
            Debug.Log("Hello, I am door.");
            animator.SetTrigger("DoorOpen");
            triggerCollider.enabled = false;
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
