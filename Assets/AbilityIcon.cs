using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIcon : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] GameObject coolDownImage;
    [SerializeField] Player player;
    RectTransform rect;

    // Start is called before the first frame update
    void Start()
    {
        rect = coolDownImage.GetComponent<RectTransform>();
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
            Vector2 rector2 = rect.sizeDelta;
            rector2.y = 0;
            //rector2.y =  Mathf.Lerp(0, 100, player.CurrentlyEquippedAbility().coolDown);
        }
    }
}
