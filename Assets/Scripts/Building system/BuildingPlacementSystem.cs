using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingPlacementSystem : MonoBehaviour
{
	[SerializeField] private LayerMask includeLayers;
	[SerializeField] private int ignoreRaycastLayer = 2;
	[SerializeField] private int constructionLayer = 10;

	[SerializeField] private Transform playerCameraTransform;
	[SerializeField] private Transform constructionHolderPoint;
	[SerializeField] private float maxPlacementDistance = 10f;
	[SerializeField] private float rotationSpeed = 5f;

	private ConstructionInfo currConstructionInfo;
	private ConstructionPlacementHandler currConstruction;

	[SerializeField] private InputActionAsset inputSystem;
	private InputAction cancelPlacementAction;
	private InputAction rotateAction;

	public event Action OnConstructionMounting;
	public event Action OnPlacementCancelling;

	private void Awake()
	{
		cancelPlacementAction = inputSystem.FindAction("CancelPlacement");
		rotateAction = inputSystem.FindAction("Rotate");
	}

	private void OnEnable()
	{
		rotateAction?.Enable();
		cancelPlacementAction?.Enable();
		cancelPlacementAction.performed += CancelPlacemnent;
	}

	private void OnDisable()
	{
		rotateAction?.Disable();
		cancelPlacementAction.performed -= CancelPlacemnent;
		cancelPlacementAction?.Disable();
	}
	private void Update()
	{
		if (currConstruction) 
		{
			var mouseScrollDelta = rotateAction.ReadValue<Vector2>();
			currConstruction.transform.Rotate(currConstruction.transform.up, mouseScrollDelta.y * rotationSpeed);

			if (playerCameraTransform.hasChanged)
			{
				if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out var hitInfo, maxPlacementDistance, includeLayers))
				{
					currConstruction.gameObject.SetActive(true);
					constructionHolderPoint.position = hitInfo.point;
				}
				else
				{
					currConstruction.gameObject.SetActive(false);
				}

				playerCameraTransform.hasChanged = false;
			}
		}
	}

	public void SpawnConstruction(ConstructionInfo constructionInfo)
	{
		CancelPlacemnent(new InputAction.CallbackContext());
		print("start spawn");
		Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out var hitInfo);

		currConstruction = Instantiate(constructionInfo.PreviewObject, hitInfo.point, Quaternion.identity);
		currConstruction.gameObject.SetActive(false);
		currConstruction.transform.SetParent(constructionHolderPoint);
		currConstruction.transform.localPosition = Vector3.zero;
		currConstructionInfo = constructionInfo;
	}

	public void CancelPlacemnent(InputAction.CallbackContext context)
	{
		if (currConstruction)
		{
			Destroy(currConstruction.gameObject);
			currConstruction = null;
			currConstructionInfo = null;

			OnPlacementCancelling?.Invoke();
		}
	}
}
