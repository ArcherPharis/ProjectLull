using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoBox : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] TextMeshProUGUI text;
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

        
        if (inventory.quededItem)
        {
            image.enabled = true;
            text.text = inventory.quededItem.Message();
        }

        else
        {
            image.enabled = false;
            text.text = null;
        }
    }
}
