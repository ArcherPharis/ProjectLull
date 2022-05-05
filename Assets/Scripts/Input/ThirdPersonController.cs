using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
	public float MoveSpeed = 2.0f;
	public float SprintSpeed = 5.335f;

	[Range(0.0f, 0.3f)]
	public float RotationSmoothTime = 0.12f;
	public float SpeedChangeRate = 10.0f;
	public float Sensitivity = 1.0f;

	public float JumpHeight = 1.2f;
	public float Gravity = -15.0f;


	public float JumpTimeout = 0.50f;
	public float FallTimeout = 0.15f;


	public bool Grounded = true;
	public float GroundedOffset = -0.14f;
	public float GroundedRadius = 0.28f;
	public LayerMask GroundLayers;


	public GameObject CinemachineCameraTarget;
	public float TopClamp = 70.0f;
	public float BottomClamp = -30.0f;
	public float CameraAngleOverride = 0.0f;
	public bool LockCameraPosition = false;
	public bool isDisabled = false;

	private float speed;
	private float animBlend;
	private float targetRotation = 0.0f;
	private float rotationVelocity;
	private float verticalVelocity;
	private float terminalV = 53.0f;

	private int speedID;
	private int groundedID;
	private int fallID;
	private int motionSpeedID;

	private Animator animator;
	private CharacterController controller;
	private InputComponent input;
	private GameObject mainCamera;


	private bool hasAnimator;
	private bool characterRotationOnMove = true;

	private void Awake()
	{
	
	mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		
	}

	private void Start()
	{
		hasAnimator = TryGetComponent(out animator);
		controller = GetComponent<CharacterController>();
		input = GetComponent<InputComponent>();

		speedID = Animator.StringToHash("Speed");
		groundedID = Animator.StringToHash("Grounded");
		fallID = Animator.StringToHash("FreeFall");
		motionSpeedID = Animator.StringToHash("MotionSpeed");

	}

	private void Update()
	{
		hasAnimator = TryGetComponent(out animator);
			
		PlayerFall();
		GroundedCheck();
	if (!isDisabled)
	{
		Move();
	}
		
	}

	private void GroundedCheck()
	{
		Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
		Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

		if (hasAnimator)
		{
			animator.SetBool(groundedID, Grounded);
		}
	}


private void Move()
	{
		float targetSpeed = input.sprint ? SprintSpeed : MoveSpeed;

		if (input.move == Vector2.zero) targetSpeed = 0.0f;

		float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

		float speedOffset = 0.1f;
		float inputMagnitude = input.analogMovement ? input.move.magnitude : 1f;
		if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
		{

			speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

			speed = Mathf.Round(speed * 1000f) / 1000f;
		}
		else
		{
			speed = targetSpeed;
		}
		animBlend = Mathf.Lerp(animBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

		Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;
		if (input.move != Vector2.zero)
		{
			targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
			float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);
        if (characterRotationOnMove)
        {
			transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
		}
				
		}


		Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;


		controller.Move(targetDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
		
		if (hasAnimator)
		{
			animator.SetFloat(speedID, animBlend);
			animator.SetFloat(motionSpeedID, inputMagnitude);
		}
	}

	private void PlayerFall()
	{
		if (Grounded)
		{
			animator.SetBool(fallID, false);

			if (verticalVelocity < 0.0f)
			{
				verticalVelocity = -2f;
			}

			if (verticalVelocity < terminalV)
			{
				verticalVelocity += Gravity * Time.deltaTime;
			}
		}
        else
        {
			animator.SetBool(fallID, true);
        }
	}

	public void SetSensitivity(float newSensitivity)
    {
		Sensitivity = newSensitivity;
    }

	public void SetPlayerRotateAim(bool rotateCharacterDirection)
	{
		characterRotationOnMove = rotateCharacterDirection;
	}
	
}