using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : Interactable
{

    
    public override void InteractItem()
    {
        base.InteractItem();
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
