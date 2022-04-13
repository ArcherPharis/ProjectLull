using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Item> inventory = new List<Item>();
    [SerializeField] int PistolAmmo = 0;
    public bool nearItem = false;
    public Interactable quededItem;

    public List<Item> ItemInventory()
    {
        return inventory;
    }

    public int PAmmo
    {
        get { return PistolAmmo; }
        set { PistolAmmo = value; }
    }



}
