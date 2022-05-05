using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;
using System;
using Random = UnityEngine.Random;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] Rig aimRig;
    [SerializeField]CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] float lookSensitivity;
    [SerializeField] float aimSensitivity;
    [SerializeField] Image aimingCrosshair;
    [SerializeField] LayerMask aimColliderMask = new LayerMask();
    [SerializeField] Transform whatIsBeingAimedAt;
    [SerializeField] Player player;
    [SerializeField] CinemachineBrain brain;
    PlayerInput pInput; //consider just moving everything over here...
    public bool abilityInUse;
    Animator animator;
    ThirdPersonController thirdPersonController;
    InputComponent playerInput;
    Transform hitTransform = null;
    InpurActions inputActions;
    Inventory inventory;
    float aimRigWeight;
    float fireTime;

    
    Vector3 mousePosition = Vector3.zero;

    private void Awake()
    {
        if(aimVirtualCamera == null)
        {
            aimVirtualCamera = GameObject.Find("PlayerAimCamera").GetComponent<CinemachineVirtualCamera>();
        }
        if (inputActions == null)
        {
            inputActions = new InpurActions();
        }

        pInput = GetComponent<PlayerInput>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        playerInput = GetComponent<InputComponent>();
        animator = GetComponent<Animator>();
        inputActions.Player.Interact.performed += ctx => Interact();
        inputActions.Player.UseAbility.started += ctx => UseAbility();
        inputActions.Player.UseAbility.canceled += ctx => AbilityCancel();
        inputActions.Player.EquipSidearm.performed += ctx => SwitchWeapon();
        inputActions.Player.EquipPrimary.performed += ctx => SwitchWeapon();
        inputActions.Player.Reload.performed += ctx => ReloadWeapon();
        inputActions.Player.Inventory.performed += ctx => InventoryMenu();
        inputActions.Player.ToggleItem.performed += ctx => ToggleEquippedItem();
        inputActions.Player.UseItem.performed += ctx => UseItem();
        inventory = GetComponent<Inventory>();
        inventory.EquipSidearmSlotOne();

    }

    private void UseItem()
    {
        inventory.UseItem();
    }

    private void ToggleEquippedItem()
    {
        inventory.CycleEquippedItem();
    }

    private void InventoryMenu()
    {
        inventory.TurnOnInventoryScreen();
        if(pInput.currentActionMap.name == "Player")
        {
            pInput.SwitchCurrentActionMap("UI");
            brain.enabled = false;
        }
        else if (pInput.currentActionMap.name =="UI")
        {
            pInput.SwitchCurrentActionMap("Player");
            brain.enabled = true;
        }
        
    }

    private void AbilityCancel()
    {
        player.StopUsingToggledAbility();
    }

    void UseAbility()
    {
        //inputActions.Player.Aim.Disable(); //in the future, make all the input work here, so we can just use this instead of global variables
        player.UseToggledAbility();        
    }

    private void ReloadWeapon()
    {
        inventory.ReloadWeapon();
    }

    private void SwitchWeapon()
    {
        animator.SetLayerWeight(inventory.CurrentWeapon().AnimationID, 0);
        inventory.SwitchWeapon();
    }

    private void Interact()
    {
        if (inventory.nearItem)
        {
            inventory.quededItem.InteractItem();
            inventory.nearItem = false;
            inventory.quededItem = null;
        }
      
    }

    private void OnEnable()
    {
        if (inputActions != null)
        {
            inputActions.Enable();
        }
    }

    private void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.Disable();
        }
    }


    // Update is called once per frame
    void Update()
    {
        PlayerAim();
        PlayerShoot(mousePosition);
        aimRig.weight = Mathf.Lerp(aimRig.weight, aimRigWeight, Time.deltaTime * 20f);
        if (inputActions.Player.UseAbility.IsPressed())
        {
            player.UseUpdatableSkillEffect();
        }

    }

    

    void RayCastCenter() 
    {
        Vector2 centerOfScreen = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(centerOfScreen); //creates a ray originating from camera and going to the center of the screen.

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 99999f, aimColliderMask))//for now it aims at everything can be useful for allies we don't wanna aim at.
        {
            whatIsBeingAimedAt.position = raycastHit.point;
            mousePosition = raycastHit.point;
            hitTransform = raycastHit.transform;
            Vector3 aimTarget = mousePosition;
            aimTarget.y = transform.position.y;
            Vector3 aimdDirection = (aimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimdDirection, Time.deltaTime * 20f);

            if(hitTransform.gameObject.tag == "Enemy")
            {
                aimingCrosshair.color = Color.red;
            }
            else
            {
                aimingCrosshair.color = Color.white;
            }
        }
        else
        {
            whatIsBeingAimedAt.position = ray.GetPoint(200);
            mousePosition = ray.GetPoint(200);
            aimingCrosshair.color = Color.white;
        }
    }

    void PlayerAim()
    {

        if (playerInput.aim && !playerInput.sprint && !Globals.teleportationInUse)
        {
            thirdPersonController.SetPlayerRotateAim(false);
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.MoveSpeed = 1f;
            aimRigWeight = 1f;
            animator.SetLayerWeight(inventory.CurrentWeapon().AnimationID, Mathf.Lerp(animator.GetLayerWeight(inventory.CurrentWeapon().AnimationID), 1f, Time.deltaTime * 10f));
            aimingCrosshair.enabled = true;
            RayCastCenter();
        }
        else
        {
            thirdPersonController.SetPlayerRotateAim(true);
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(lookSensitivity);
            thirdPersonController.MoveSpeed = 2f;
            aimRigWeight = 0f;
            animator.SetLayerWeight(inventory.CurrentWeapon().AnimationID, Mathf.Lerp(animator.GetLayerWeight(inventory.CurrentWeapon().AnimationID), 0f, Time.deltaTime * 10f));
            aimingCrosshair.enabled = false;
            
        }
    }

    void PlayerShoot(Vector3 mouseDirection)
    {
        if (playerInput.aim && !playerInput.sprint && Time.time >= fireTime && !Globals.teleportationInUse)
        {
            if (playerInput.shoot && inventory.CurrentWeapon().CurrentAmmo >= 1)
            {
                if(hitTransform != null)
                {
                    if (hitTransform.GetComponent<Damagable>() != null)
                    {
                        
                        inventory.FireWeapon();
                        inventory.ReduceCurrentAmmoAmount();
                        fireTime = Time.time + 1f / inventory.CurrentWeapon().FireRate;
                    }
                    else
                    {
                        inventory.FireWeapon();
                        inventory.ReduceCurrentAmmoAmount();
                        fireTime = Time.time + 1f / inventory.CurrentWeapon().FireRate;
                    }
                }
                Vector3 aimDirection = (mouseDirection - inventory.CurrentWeapon().firingPoint.position).normalized;
                Instantiate(inventory.CurrentWeapon().fireEffect, inventory.CurrentWeapon().firingPoint.position, Quaternion.LookRotation(aimDirection, Vector3.up));
                playerInput.shoot = false;            
            }
            else if (playerInput.shoot && inventory.CurrentWeapon().CurrentAmmo <= 0)
            {
                inventory.WeaponOutOfAmmo();
            }
        }
            playerInput.shoot = false;//prevents bool from being set to true when not aiming.
            
    }

}
