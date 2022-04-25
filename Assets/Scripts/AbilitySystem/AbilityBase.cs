using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBase : ScriptableObject
{
    [SerializeField] Sprite skillIcon;
    public int ID;

    public Sprite GetSkillIcon()
    {
        return skillIcon;
        
    }

    public virtual void ApplySkillEffect()
    {
        Debug.Log("Base Skill in effect");
    }

    public virtual void UnapplySkillEffect()
    {
        
    }

    public virtual void UpdatableEffects()
    {

    }
}
