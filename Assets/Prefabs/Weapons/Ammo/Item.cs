using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : ScriptableObject
{
    [SerializeField] int amount;
    [SerializeField] Image itemImage;
    public Inventory playerInventory;
    public int Amount
    {
        get { return amount; }
        private set { }
    }

    public virtual void GiveItemToInventory()
    {
        playerInventory = GameObject.Find("Player").GetComponent<Inventory>();
        
    }
}
