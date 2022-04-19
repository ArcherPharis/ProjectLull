using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject sidearmSlotOne;
    [SerializeField] GameObject currentlyEquippedWeapon;
    public int currentWeaponAmmo;
    [SerializeField] List<Item> inventory = new List<Item>();
    [SerializeField] int PistolAmmo = 0;
    [SerializeField] Transform weaponSlot;
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

    public void SwitchWeapon()
    {
        currentlyEquippedWeapon = sidearmSlotOne;
    }

    public void FireWeapon()
    {
        CurrentWeapon().StartFiring();
    }

    public void WeaponOutOfAmmo()
    {
        CurrentWeapon().OutOfAmmo();
    }



    public void SpawnSideSlotOneWeapon() //this method is not needed, instead we will do "on change"
    {
        if(sidearmSlotOne.GetComponent<Weapon>() == null)
        {
            Debug.Log("That's not a weapon equipped to one of the slots!");
            return;
        }

        SwitchWeapon();
        
        sidearmSlotOne.SetActive(true);

        Weapon weapon = currentlyEquippedWeapon.GetComponent<Weapon>();
        currentWeaponAmmo = weapon.MaxCapacity;
    }

    //fire method will get weapon data and give it to the shooter component to deal the amount of damage;
    public float GetWeaponDataForDamage()
    {
        Weapon currentWeapon = currentlyEquippedWeapon.GetComponent<Weapon>();
        return currentWeapon.WeaponDamage;
    }

    public Weapon CurrentWeapon()
    {
        return currentlyEquippedWeapon.GetComponent<Weapon>();
    }

    public void ReduceCurrentAmmoAmount()
    {
        currentWeaponAmmo = Mathf.Clamp(currentWeaponAmmo, 0, CurrentWeapon().MaxCapacity);
        currentWeaponAmmo--;
    }




}
