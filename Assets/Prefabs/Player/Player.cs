using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public List<AbilityBase> abilities;
    [SerializeField] AbilityBase currentEquippedAbility;
    public float elapsedTime;
    bool canUseAbility = false;



    // Start is called before the first frame update
    void Start()
    {
        currentEquippedAbility = abilities[0];
        elapsedTime = currentEquippedAbility.coolDown;
        
    }
    public AbilityBase CurrentlyEquippedAbility()
    {
        return currentEquippedAbility;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    public void UseToggledAbility()
    {

        if (elapsedTime >= currentEquippedAbility.coolDown)
        {
            currentEquippedAbility.ApplySkillEffect();
            canUseAbility = true;
            
        }

    }

    public void StopUsingToggledAbility()
    {
        if (elapsedTime >= currentEquippedAbility.coolDown && canUseAbility)
        {
            currentEquippedAbility.UnapplySkillEffect();
            elapsedTime = 0;
            canUseAbility = false;
        }

    }

    public void UseUpdatableSkillEffect()
    {
        if (elapsedTime >= currentEquippedAbility.coolDown)
        {
            currentEquippedAbility.UpdatableEffects();
        }
    }


    // Update is called once per frame

}
