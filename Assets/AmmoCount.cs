using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoCount : MonoBehaviour
{
    [SerializeField] Image weaponImage;
    Inventory inventory;
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("Player").GetComponent<Inventory>();
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

        weaponImage.sprite = inventory.CurrentWeapon().weaponSprite;

        text.text = inventory.CurrentWeapon().CurrentAmmo.ToString();
    }
}
