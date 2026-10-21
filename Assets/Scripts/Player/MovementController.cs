using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{
	[SerializeField] private CharacterController charController;
	[SerializeField] private CharacterMovementData movementData;
	[SerializeField] private float gravityForce;
	private float currHorizontalSpeed;
	private float currVerticalSpeed;

	private Vector3 currVelocity = new Vector3();

    [SerializeField] private bool isHoldToRun = true;
    [SerializeField] private bool isHoldToCrouch = true;
	private bool isRunning = false;
    private bool isCrouching = false;

    [SerializeField] private InputActionAsset inputSystem;
    private InputAction moveAction;
    private InputAction runAction;
    private InputAction crouchAction;
	private InputAction jumpAction;

	public event Action OnJump;

	private void Awake()
	{
		charController = GetComponent<CharacterController>();

		moveAction = inputSystem.FindAction("Move");
		runAction = inputSystem.FindAction("Run");
		crouchAction = inputSystem.FindAction("Crouch");
		jumpAction = inputSystem.FindAction("Jump");

		ApplyDefaultMovementValues();
		SubscribeOnInputActions();
	}

	private void OnEnable()
	{
		moveAction?.Enable();
		runAction?.Enable();
		crouchAction?.Enable();
		jumpAction?.Enable();
	}

	private void OnDisable()
	{
		moveAction?.Disable();
		runAction?.Disable();
		crouchAction?.Disable();
		jumpAction?.Disable();
	}

	private void Update()
	{
		ApplyGravityForce();
		var inputMoveVector = moveAction.ReadValue<Vector2>();

		if (charController.isGrounded)
		{
			currVelocity = transform.rotation * new Vector3(
				inputMoveVector.x * currHorizontalSpeed, currVerticalSpeed, inputMoveVector.y * movementData.SideSpeedCoeff * currHorizontalSpeed);

			if (inputMoveVector.y < 0)
				currVelocity.z *= movementData.BackwardsSpeedCoeff;
		}
		else
		{
			currVelocity += new Vector3(
				inputMoveVector.x * currHorizontalSpeed, 0f, inputMoveVector.y * movementData.SideSpeedCoeff * currHorizontalSpeed) * movementData.InAirControlCoeff;
			currVelocity.y = currVerticalSpeed;
		}

		charController.Move(currVelocity * Time.deltaTime);
	}

	private void ApplyGravityForce()
	{
		if (charController.isGrounded && currVerticalSpeed < 0) 
			currVerticalSpeed = -2f;
		else
			currVerticalSpeed -= gravityForce * Time.deltaTime;
	}

	private void Jump(InputAction.CallbackContext context)
	{
		if (charController.isGrounded)
			currVerticalSpeed += movementData.JumpForce;
		OnJump?.Invoke();
	}

    private void SubscribeOnInputActions()
    {
		if (isHoldToRun)
		{
            runAction.performed += SwitchToRun;
			runAction.canceled += SwitchToWalk;
		}
		else
		{
			runAction.performed += ToggleRun;
		}

		if (isHoldToCrouch)
		{
			crouchAction.performed += SwitchToCrouch;
			crouchAction.canceled += SwitchToWalk;
		}
		else
		{
			crouchAction.performed += ToggleCrouch;
		}

		jumpAction.performed += Jump;
	}

	private void ApplyDefaultMovementValues()
	{
		currHorizontalSpeed = movementData.BasicSpeed;
		isRunning = false;
		isCrouching = false;
	}

    private void SwitchToRun(InputAction.CallbackContext context)
    {
		currHorizontalSpeed = movementData.RunSpeed;
		isRunning = true;
		isCrouching = false;
	}

    private void SwitchToCrouch(InputAction.CallbackContext context)
    {
		currHorizontalSpeed = movementData.CrouchSpeed;
		isRunning = false;
		isCrouching = true;
    }

    private void SwitchToWalk(InputAction.CallbackContext context)
    {
		ApplyDefaultMovementValues();
	}

	private void ToggleRun(InputAction.CallbackContext context)
	{
		if (isRunning)
			SwitchToWalk(context);
		else
			SwitchToRun(context);
	}

	private void ToggleCrouch(InputAction.CallbackContext context)
	{
		if (isCrouching)
			SwitchToWalk(context);
		else
			SwitchToCrouch(context);
	}
}