using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Damagable
{
    public List<AbilityBase> abilities;
    [SerializeField] AbilityBase currentEquippedAbility;
    [SerializeField] AudioClip damageClip;
    AudioSource audioSource;
    public float MaxHealth;
    PlayerInput playerInput;
    ThirdPersonController tpc;
    public float elapsedTime;
    bool canUseAbility = false;
    public bool playerTeleported;
    GameObject storeObject = null;
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = Health;
        audioSource = GetComponent<AudioSource>();
        currentEquippedAbility = abilities[0];
        elapsedTime = currentEquippedAbility.coolDown;
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
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
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            tpc.isDisabled = true;
            animator.SetTrigger("Die");
            Invoke("BackToTitle", 6f);
            playerInput.SwitchCurrentActionMap("UI");

        }
    }

    void BackToTitle()
    {
        SceneManager.LoadScene(0);
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

    public void OnHit()
    {
        audioSource.PlayOneShot(damageClip);
        animator.SetTrigger("HitReact");
    }

    public void HitReact()
    {
        if (Health >= 0)
        {
            tpc.isDisabled = true;
        }
    }
    public void HitRecovery()
    {
        if (Health >= 0)
        {
            tpc.isDisabled = false;
        }

    }

    void PlayerTeleported(bool condition)
    {
       playerTeleported = condition;
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
