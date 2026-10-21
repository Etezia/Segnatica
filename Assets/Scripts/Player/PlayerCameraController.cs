using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    [SerializeField] private float yRotationSpeed = 1f;
    [SerializeField] private float xRotationSpeed = 1f;
	[SerializeField] private float upLimitAngle = -90f;
	[SerializeField] private float downLimitAngle = 85f;

	private float currRotationX = 0f;

	[SerializeField] private InputActionAsset inputSystem;
	private InputAction rotateCameraAction;

	private void Awake()
	{
		rotateCameraAction = inputSystem.FindAction("RotateCamera");
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Start() => currRotationX = 0f;

	private void Update()
	{
		var inputMouseVector = rotateCameraAction.ReadValue<Vector2>();

		currRotationX -= inputMouseVector.y * xRotationSpeed;
		currRotationX = Mathf.Clamp(currRotationX, upLimitAngle, downLimitAngle);
		transform.localRotation = Quaternion.Euler(currRotationX, 0f, 0f);

		playerTransform.Rotate(0f, inputMouseVector.x * yRotationSpeed, 0f);
	}
}
