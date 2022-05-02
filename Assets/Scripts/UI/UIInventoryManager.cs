using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIInventoryManager : MonoBehaviour
{
    [SerializeField] GameObject AmmoPouchUI;
    [SerializeField] GameObject UtilityBeltUI;
    [SerializeField] List<UtilitySlot> utilityBeltSlots = new List<UtilitySlot>();
    [SerializeField] List<AmmoSlot> ammoPouchSlots = new List<AmmoSlot>();
    [SerializeField] Inventory playerInventory;
    [SerializeField] Canvas inventoryCanvas;
    [SerializeField] Canvas gamePlayCanvas;
    // Start is called before the first frame update
    void Start()
    {
        PopulateAmmoPouch();
        PopulateUtilityBelt();
        InitAmmoPouch();
        InitUtilityBelt();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PopulateAmmoPouch()
    {
        foreach (Transform pouchSlots in AmmoPouchUI.transform)
        {
            AmmoSlot pouchObjects = pouchSlots.gameObject.GetComponent<AmmoSlot>();
            ammoPouchSlots.Add(pouchObjects);
        }
    }

    void PopulateUtilityBelt()
    {
        foreach (Transform utilitySlots in UtilityBeltUI.transform)
        {
            UtilitySlot utilitySlot = utilitySlots.gameObject.GetComponent<UtilitySlot>();
            utilityBeltSlots.Add(utilitySlot);
        }
    }
    void InitAmmoPouch()
    {
        if (playerInventory.AmmoPouch().Count != 0)
        { 
            for (int i = 0; i < playerInventory.AmmoPouch().Count; i++)
            {
                ammoPouchSlots[i].image.enabled = true;
                ammoPouchSlots[i].text.enabled = true;
                playerInventory.AmmoPouch()[i].SetCorrectItemCount();
                ammoPouchSlots[i].text.text = playerInventory.AmmoPouch()[i].AmmoCount.ToString();
                ammoPouchSlots[i].image.sprite = playerInventory.AmmoPouch()[i].itemImage;

                }
            }
        
    }

    void InitUtilityBelt()
    {
        if (playerInventory.UtilityBelt().Count != 0)
        {
            for (int i = 0; i < playerInventory.UtilityBelt().Count; i++)
            {
                utilityBeltSlots[i].image.enabled = true;
                //utilityBeltSlots[i].text.enabled = true;
                //playerInventory.UtilityBelt()[i].SetCorrectItemCount();//not needed
                //utilityBeltSlots[i].text.text = playerInventory.AmmoPouch()[i].AmmoCount.ToString();
                utilityBeltSlots[i].image.sprite = playerInventory.UtilityBelt()[i].itemImage;

            }
        }

    }

    public void EnableCanvas()
    {
        if (!inventoryCanvas.enabled)
        {
            gamePlayCanvas.enabled = false;
            inventoryCanvas.enabled = true;
            
            for (int i = 0; i < playerInventory.AmmoPouch().Count; i++)
            {
                ammoPouchSlots[i].text.text = playerInventory.AmmoPouch()[i].AmmoType().ToString();
            }
            Time.timeScale = 0f;
        }
        else
        {
            gamePlayCanvas.enabled = true;
            inventoryCanvas.enabled = false;
            Time.timeScale = 1f;
        }


    }

    public void UpdateUtilityBelt()
    {
        foreach (Transform utilitySlots in AmmoPouchUI.transform)
        {
            UtilitySlot utilityItems = utilitySlots.gameObject.GetComponent<UtilitySlot>();
            utilityBeltSlots.Add(utilityItems);
            for (int i = 0; i < playerInventory.UtilityBelt().Count; i++)
            {
                //if (playerInventory.AmmoPouch()[i] == null)
                //{//note for tommorrow, it doesn't work because we aren't setting to null, we are removing it
                //    //so we need to find a different way to remove the affected image's sprite. ammoslot
                //    //could possibly know of what is in it, and when that becomes null, we can remove the image.
                //    Debug.Log("Are we getting here?");
                //    ammoPouchSlots[i].image.enabled = false;
                //    ammoPouchSlots[i].image.enabled = false;
                //}

                utilityBeltSlots[i].image.enabled = true;
                utilityBeltSlots[i].image.sprite = playerInventory.UtilityBelt()[i].itemImage;

            }
        }

    }



    public void UpdateAmmoPouch()
    {
        foreach(Transform pouchSlots in AmmoPouchUI.transform)
        {
            AmmoSlot pouchObjects = pouchSlots.gameObject.GetComponent<AmmoSlot>();
            ammoPouchSlots.Add(pouchObjects);
            for (int i = 0; i < playerInventory.AmmoPouch().Count; i++)
            {
     

                ammoPouchSlots[i].image.enabled = true;
                ammoPouchSlots[i].text.enabled = true;
                playerInventory.AmmoPouch()[i].SetCorrectItemCount();
                ammoPouchSlots[i].text.text = playerInventory.AmmoPouch()[i].AmmoCount.ToString();
                ammoPouchSlots[i].image.sprite = playerInventory.AmmoPouch()[i].itemImage;

            }
        }

    }
}
