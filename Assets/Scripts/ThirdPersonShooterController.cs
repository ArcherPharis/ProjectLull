using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;
using System;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] Rig aimRig;
    [SerializeField]CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] float lookSensitivity;
    [SerializeField] float aimSensitivity;
    [SerializeField] Image aimingCrosshair;
    [SerializeField] LayerMask aimColliderMask = new LayerMask();
    [SerializeField] Transform whatIsBeingAimedAt;
    [SerializeField] Transform bulletProjectilePrefab;
    [SerializeField] Transform hitGreen;
    [SerializeField] Transform hitRed;
    Animator animator;
    ThirdPersonController thirdPersonController;
    InputComponent playerInput;
    Transform hitTransform = null;
    InpurActions inputActions;
    Inventory inventory;
    float aimRigWeight;
    
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


        thirdPersonController = GetComponent<ThirdPersonController>();
        playerInput = GetComponent<InputComponent>();
        animator = GetComponent<Animator>();
        inputActions.Player.Interact.performed += ctx => Interact();
        inventory = GetComponent<Inventory>();
        inventory.SpawnSideSlotOneWeapon();
        
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAim();
        PlayerShoot(mousePosition);
        aimRig.weight = Mathf.Lerp(aimRig.weight, aimRigWeight, Time.deltaTime * 20f);


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

        if (playerInput.aim && !playerInput.sprint)
        {
            thirdPersonController.SetPlayerRotateAim(false);
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            aimRigWeight = 1f;
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
            aimingCrosshair.enabled = true;
            RayCastCenter();
        }
        else
        {
            thirdPersonController.SetPlayerRotateAim(true);
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(lookSensitivity);
            aimRigWeight = 0f;
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
            aimingCrosshair.enabled = false;
            
        }
    }

    void PlayerShoot(Vector3 mouseDirection)
    {
        if (playerInput.aim && !playerInput.sprint )
        {
            if (playerInput.shoot && inventory.currentWeaponAmmo >= 1)
            {
                if(hitTransform != null)
                {
                    if (hitTransform.GetComponent<Damagable>() != null)
                    {
                        Instantiate(hitGreen, whatIsBeingAimedAt.transform.position, Quaternion.identity);
                        inventory.FireWeapon();
                        inventory.ReduceCurrentAmmoAmount();
                        Damagable enemyHit = hitTransform.GetComponent<Damagable>();
                        enemyHit.DealDamage(inventory.GetWeaponDataForDamage());
                        enemyHit.Die();
                        
                    }
                    else
                    {
                        inventory.FireWeapon();
                        inventory.ReduceCurrentAmmoAmount();
                        Instantiate(hitRed, whatIsBeingAimedAt.transform.position, Quaternion.identity);
                        
                    }
                }

                Vector3 aimDirection = (mouseDirection - inventory.CurrentWeapon().firingPoint.position).normalized;
                Instantiate(bulletProjectilePrefab, inventory.CurrentWeapon().firingPoint.position, Quaternion.LookRotation(aimDirection, Vector3.up));
                playerInput.shoot = false;
                
            }
     
        }
            playerInput.shoot = false;//prevents bool from being set to true when not aiming.
            
    }

}
