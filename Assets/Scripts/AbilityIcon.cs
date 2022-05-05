using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIcon : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Image coolDownImage;
    [SerializeField] Player player;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAbilityIcon();
        CoolDownScale();

    }

    void UpdateAbilityIcon()
    {
        image.sprite = player.CurrentlyEquippedAbility().GetSkillIcon();
    }

    void CoolDownScale()
    {
        if(player.elapsedTime < player.CurrentlyEquippedAbility().coolDown)
        {
            coolDownImage.fillAmount += 1 / player.CurrentlyEquippedAbility().coolDown * Time.deltaTime;
            
        }
    }


}
