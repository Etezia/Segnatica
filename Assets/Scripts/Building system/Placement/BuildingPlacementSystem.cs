using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingPlacementSystem : MonoBehaviour
{
	[SerializeField] private LayerMask includeLayers;
	[SerializeField] private LayerMask bracingDetectorsLayer;
	[SerializeField] private int constructionLayer = 10;

	[SerializeField] private Transform playerCameraTransform;
	[SerializeField] private Transform constructionHolderPoint;
	[SerializeField] private float maxPlacementDistance = 10f;
	[SerializeField] private float rotationSpeed = 5f;

	private ConstructionInfo currConstructionInfo;
	private ConstructionPlacementHandler currConstruction;
	private BracingPointsHolder currBracingPointsHolder;

	[SerializeField] private InputActionAsset inputSystem;
	private InputAction cancelPlacementAction;
	private InputAction rotateAction;
	private InputAction mountConstructionAction;

	public event Action OnConstructionMounting;
	public event Action OnPlacementCancelling;

	private void Awake()
	{
		cancelPlacementAction = inputSystem.FindAction("CancelPlacement");
		rotateAction = inputSystem.FindAction("Rotate");
		mountConstructionAction = inputSystem.FindAction("MountConstruction");
	}

	private void OnEnable()
	{
		rotateAction?.Enable();
		cancelPlacementAction?.Enable();
		mountConstructionAction?.Enable();
		cancelPlacementAction.performed += CancelPlacemnent;
		mountConstructionAction.performed += TryMountConstruction;
	}

	private void OnDisable()
	{
		rotateAction?.Disable();
		cancelPlacementAction.performed -= CancelPlacemnent;
		mountConstructionAction.performed -= TryMountConstruction;
		cancelPlacementAction?.Disable();
		mountConstructionAction?.Disable();
	}
	private void Update()
	{
		if (currConstruction) 
		{
			var mouseScrollDelta = rotateAction.ReadValue<Vector2>();
			constructionHolderPoint.Rotate(currConstruction.transform.up, mouseScrollDelta.y * rotationSpeed);

			if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out var hitInfo, maxPlacementDistance, includeLayers))
			{
				currConstruction.gameObject.SetActive(true);

				if ((1 << hitInfo.collider.gameObject.layer & bracingDetectorsLayer) != 0)
				{
					if (!currBracingPointsHolder || currBracingPointsHolder.gameObject != hitInfo.collider.gameObject)
						currBracingPointsHolder = hitInfo.collider.gameObject.GetComponent<BracingPointsHolder>();
						
					if (currBracingPointsHolder.BracingPointsDict.ContainsKey(currConstructionInfo.Type))
					{
						var bracePointTransform = currBracingPointsHolder.BracingPointsDict[currConstructionInfo.Type];
						constructionHolderPoint.position = bracePointTransform.position;
						constructionHolderPoint.rotation = bracePointTransform.rotation *
							Quaternion.Euler(new Vector3(0f, constructionHolderPoint.transform.rotation.y - constructionHolderPoint.transform.rotation.y % 90f, 0f));
					}
					else
					{
						constructionHolderPoint.position = hitInfo.point;
					}
				}
				else
				{
					constructionHolderPoint.position = hitInfo.point;
				}
			}
			else
			{
				currConstruction.gameObject.SetActive(false);
			}

			playerCameraTransform.hasChanged = false;
		}
	}

	public void SpawnConstruction(ConstructionInfo constructionInfo)
	{
		Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out var hitInfo);

		currConstruction = Instantiate(constructionInfo.PreviewObject, hitInfo.point, Quaternion.identity);
		if ((hitInfo.point - playerCameraTransform.position).magnitude > maxPlacementDistance)
			currConstruction.gameObject.SetActive(false);

		currConstruction.transform.SetParent(constructionHolderPoint);
		currConstruction.transform.localPosition = Vector3.zero;
		currConstruction.transform.localRotation = Quaternion.Euler(Vector3.zero);
		currConstructionInfo = constructionInfo;
	}

	private void TryMountConstruction(InputAction.CallbackContext context)
	{
		if (currConstruction && currConstruction.CanBeMounted)
		{
			Destroy(currConstruction.gameObject);
			currConstruction = null;

			var mountedConstruction = Instantiate(currConstructionInfo.ConstructionObject, constructionHolderPoint.position, constructionHolderPoint.rotation);
			currConstructionInfo = null;

			OnConstructionMounting?.Invoke();
		}
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
