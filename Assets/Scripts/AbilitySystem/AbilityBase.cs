using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityBase : MonoBehaviour
{
    [SerializeField] Sprite skillIcon;
    Image image;
    public float coolDown;
    public string name;
    public int ID;


    public Sprite GetSkillIcon()
    {
        return skillIcon;
        
        
    }

    public virtual void Init()
    {

    }

    public virtual void ApplySkillEffect()
    {
        image = GameObject.Find("Cooldown").GetComponent<Image>();
        image.fillAmount = 0;
        Debug.Log("Base Skill in effect");

    }

    public virtual void UnapplySkillEffect()
    {
        
    }

    public virtual void UpdatableEffects()
    {

    }
}
