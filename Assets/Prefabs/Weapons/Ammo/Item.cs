using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : ScriptableObject
{
    [SerializeField] int amount;
    public Sprite itemImage;
    public Inventory playerInventory;
    public int AmmoCount;
    public int Amount
    {
        get { return amount; }
        private set { }
    }
    public virtual void SetCorrectItemCount()
    {

        playerInventory = GameObject.Find("Player").GetComponent<Inventory>();
    }

    public virtual void GiveItemToInventory()
    {
        playerInventory = GameObject.Find("Player").GetComponent<Inventory>();
        
    }

    public virtual void ApplyItemEffects()
    {

    }
    
    public virtual int AmmoType()
    {
        return 0;
    }

}
