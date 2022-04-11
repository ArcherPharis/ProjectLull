using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField]CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] float lookSensitivity;
    [SerializeField] float aimSensitivity;
    [SerializeField] Image aimingCrosshair;
    [SerializeField] LayerMask aimColliderMask = new LayerMask();
    [SerializeField] Transform whatIsBeingAimedAt;
    [SerializeField] Transform bulletProjectilePrefab;
    [SerializeField] Transform tempBulletSpawn; //TODO will get this from each weapon instead in the future.
    Animator animator;
    ThirdPersonController thirdPersonController;
    InputComponent playerInput;

    Vector3 mousePosition = Vector3.zero;

    private void Awake()
    {
        if(aimVirtualCamera == null)
        {
            aimVirtualCamera = GameObject.Find("PlayerAimCamera").GetComponent<CinemachineVirtualCamera>();
        }
        thirdPersonController = GetComponent<ThirdPersonController>();
        playerInput = GetComponent<InputComponent>();
        animator = GetComponent<Animator>();
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


    }

    void RayCastCenter() 
    {
        Vector2 centerOfScreen = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(centerOfScreen); //creates a ray originating from camera and going to the center of the screen.
        
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 99999f, aimColliderMask))//for now it aims at everything can be useful for allies we don't wanna aim at.
        {
            whatIsBeingAimedAt.position = raycastHit.point;
            mousePosition = raycastHit.point;
            Vector3 aimTarget = mousePosition;
            aimTarget.y = transform.position.y;
            Vector3 aimdDirection = (aimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimdDirection, Time.deltaTime * 20f);
        }
    }

    void PlayerAim()
    {

        if (playerInput.aim && !playerInput.sprint)
        {
            thirdPersonController.SetPlayerRotateAim(false);
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
            aimingCrosshair.enabled = true;
            RayCastCenter();
        }
        else
        {
            thirdPersonController.SetPlayerRotateAim(true);
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(lookSensitivity);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
            aimingCrosshair.enabled = false;
        }
    }

    void PlayerShoot(Vector3 mouseDirection)
    {
        if (playerInput.aim && !playerInput.sprint)
        {
            if (playerInput.shoot)
            {
                Debug.Log("We're shooting");
                Vector3 aimDirection = (mouseDirection - tempBulletSpawn.position).normalized;
                Instantiate(bulletProjectilePrefab, tempBulletSpawn.position, Quaternion.LookRotation(aimDirection, Vector3.up));
                playerInput.shoot = false;
            }
        }
            playerInput.shoot = false;//prevents bool from being set to true when not aiming.
    }
}
