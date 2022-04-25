using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<AbilityBase> abilities;
    [SerializeField] AbilityBase currentEquippedAbility;
    public Player playerInstance;

    public AbilityBase CurrentlyEquippedAbility()
    {
        return currentEquippedAbility;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentEquippedAbility = abilities[0];
        playerInstance = this;
        
    }

    public void UseToggledAbility()
    {
        currentEquippedAbility.ApplySkillEffect();
    }

    public void StopUsingToggledAbility()
    {
        currentEquippedAbility.UnapplySkillEffect();
    }

    public void UseUpdatableSkillEffect()
    {
        currentEquippedAbility.UpdatableEffects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
