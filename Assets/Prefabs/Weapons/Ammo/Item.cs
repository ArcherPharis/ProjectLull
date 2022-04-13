using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField] int amount;
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
