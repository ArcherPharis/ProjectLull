using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputComponent : MonoBehaviour
{
    PlayerInput playerInput;
    [SerializeField] float walkingSpeed = 5.0f;
    float acceleration = 1.0f;
    float currentSpeed = 0f;
    Vector2 moveInput;

    private void Awake()
    {

        if (playerInput == null)
        {
            playerInput = new PlayerInput();
        }
    }

    private void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.Enable();
        }
    }

    private void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.Disable();
        }
    }

    private void Start()
    {
        playerInput.Player.Move.performed += Move;
        playerInput.Player.Move.canceled += Move;
    }
    private void Update()
    {
        transform.position += new Vector3(moveInput.x, 0, moveInput.y) * Time.deltaTime * walkingSpeed;
    }



    private void Move(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        moveInput = obj.ReadValue<Vector2>();
    }




}
