using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUseItemSlot : MonoBehaviour
{
    public Image image;
    [SerializeField] Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory.currentlyEquippedItem != null)
        {
            image.enabled = true;
            image.sprite = inventory.currentlyEquippedItem.itemImage;
        }
        else
        {
            image.enabled = false;
            image.sprite = null;
        }
    }
}
