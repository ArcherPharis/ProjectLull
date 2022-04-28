using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Damagable
{
    public List<AbilityBase> abilities;
    [SerializeField] AbilityBase currentEquippedAbility;
    ThirdPersonController tpc;
    public float elapsedTime;
    bool canUseAbility = false;
    GameObject storeObject = null;
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        currentEquippedAbility = abilities[0];
        elapsedTime = currentEquippedAbility.coolDown;
        animator = GetComponent<Animator>();
        tpc = GetComponent<ThirdPersonController>();
        AbilityInit();
        
    }
    public AbilityBase CurrentlyEquippedAbility()
    {
        return currentEquippedAbility;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        Die();
    }

    public override void Die()
    {
        if (Health <= 0)
        {
            base.Die();
            tpc.isDisabled = true;
            animator.SetTrigger("Die");

        }
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

    public void AbilityInit()
    {
        currentEquippedAbility.Init();
    }

    public GameObject FindCloestStorableItem()
    {
        GameObject[] storableItems = GameObject.FindGameObjectsWithTag("Storable");
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach(GameObject storables in storableItems)
        {
            Vector3 difference = storables.transform.position - position;
            float currentDistance = difference.sqrMagnitude;
            if(currentDistance < distance)
            {
                storeObject = storables;
                distance = currentDistance;
            }
        }
        return storeObject;
    }


    // Update is called once per frame

}
