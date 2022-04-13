using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Interactable: MonoBehaviour
{
    [SerializeField] Item item;
    public virtual void InteractItem()
    {
        Debug.Log("We reached here.");
        Inventory playerInventory = GameObject.Find("Player").GetComponent<Inventory>();

        if (playerInventory.nearItem)
        {

            if (playerInventory.ItemInventory().Any(x => x.name == item.name)) //TODO we need to add on max stack functionality.
            {//cont. so if the item also has not reached maxed capacity, it won't add or give ammo.
                Debug.Log("Item already exists in inventory.");
                item.GiveItemToInventory();
                Destroy(gameObject);
            }
            else
            {
                playerInventory.ItemInventory().Add(item);
                item.GiveItemToInventory();
                Destroy(gameObject);
            }

        }
    }
}
