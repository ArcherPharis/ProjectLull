using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject sidearmSlotOne;
    [SerializeField] GameObject primarySlotOne;
    [SerializeField] GameObject currentlyEquippedWeapon;
    public Item currentlyEquippedItem;
    [SerializeField] List<Item> utilityBelt = new List<Item>();
    [SerializeField] List<Item> ammoPouch = new List<Item>();
    [SerializeField] UIInventoryManager UIManager;
    [SerializeField] int PistolAmmo;
    [SerializeField] int ShotGunAmmo;
    [SerializeField] Transform weaponSlot;
    public bool nearItem = false;
    public Interactable quededItem;
    int itemIndex = 0;

    private void Start()
    {
        if(utilityBelt.Count != 0)
        {
            currentlyEquippedItem = utilityBelt[0];
        }
    }

    public void CycleEquippedItem()
    {
        itemIndex = (itemIndex + 1) % utilityBelt.Count;
        currentlyEquippedItem =  utilityBelt[itemIndex];
    }

    public void UseItem()
    {
        currentlyEquippedItem.ApplyItemEffects();
        utilityBelt.Remove(currentlyEquippedItem);
        if(utilityBelt.Count > 0)
        {
            CycleEquippedItem();
        }
        else
        {
            currentlyEquippedItem = null;
        }
    }

    public List<Item> UtilityBelt()
    {
        return utilityBelt;
    }

    public List<Item> AmmoPouch()
    {
        return ammoPouch;
    }

    public int PAmmo
    {
        get { return PistolAmmo; }
        set { PistolAmmo = value; }
    }

    public int SHAmmo
    {
        get { return ShotGunAmmo; }
        set { ShotGunAmmo = value; }
    }

    public void UpdateAmmoAmount()
    {
        UIManager.UpdateAmmoPouch();
    }

    public void UpdateUtility()
    {
        UIManager.UpdateUtilityBelt();
    }
    public void SwitchWeapon()
    {

        if(currentlyEquippedWeapon == sidearmSlotOne)
        {
            sidearmSlotOne.SetActive(false);
            currentlyEquippedWeapon = primarySlotOne;
            CurrentWeapon().OnSwapWeapon();
            primarySlotOne.SetActive(true);
        }

        else if (currentlyEquippedWeapon == primarySlotOne)
        {
            primarySlotOne.SetActive(false);
            currentlyEquippedWeapon = sidearmSlotOne;
            CurrentWeapon().OnSwapWeapon();
            sidearmSlotOne.SetActive(true);
        }
    }

    public void ReloadWeapon()
    {

        CurrentWeapon().ReloadWeapon();

    }

    public void TurnOnInventoryScreen()
    {
        UIManager.EnableCanvas();
        
    }

    public void FireWeapon()
    {
        CurrentWeapon().StartFiring();
    }

    public void WeaponOutOfAmmo()
    {
        CurrentWeapon().OutOfAmmo();
    }



    public void EquipSidearmSlotOne() //this method is not needed, instead we will do "on change"
    {
        if(sidearmSlotOne.GetComponent<Weapon>() == null)
        {
            Debug.Log("That's not a weapon equipped to one of the slots!");
            return;
        }

        currentlyEquippedWeapon = sidearmSlotOne;
        //currentWeaponAmmo = CurrentWeapon().CurrentAmmo;
        sidearmSlotOne.SetActive(true);

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
        //currentWeaponAmmo = Mathf.Clamp(currentWeaponAmmo, 0, CurrentWeapon().MaxCapacity);
        //currentWeaponAmmo--;
        CurrentWeapon().CurrentAmmo--;
    }




}
