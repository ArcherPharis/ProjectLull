using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject sidearmSlotOne;
    [SerializeField] GameObject primarySlotOne;
    [SerializeField] GameObject currentlyEquippedWeapon;
    //public int currentWeaponAmmo;
    [SerializeField] List<Item> inventory = new List<Item>();
    [SerializeField] int PistolAmmo;
    [SerializeField] int ShotGunAmmo;
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

    public int SHAmmo
    {
        get { return ShotGunAmmo; }
        set { ShotGunAmmo = value; }
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
        //switch (CurrentWeapon().name)
        //{
        //    case "ISP":
        //        CurrentWeapon().ReloadWeapon();
        //        break;

        //    case "Listberg":
        //        CurrentWeapon().ReloadWeapon();
        //        break;
        //}

        CurrentWeapon().ReloadWeapon();

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
