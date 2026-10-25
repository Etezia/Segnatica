using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BuildingPlacementSystem))]
public class BuildManager : MonoBehaviour
{
	[SerializeField] private BuildingPlacementSystem buildingPlacementSystem;
	[SerializeField] private ConstructionsAsset currConstructionAsset;

	[SerializeField] private InputActionAsset inputSystem;
	private InputAction debugSpawnAction;

	private void Awake()
	{
		debugSpawnAction = inputSystem.FindAction("DebugConstructionSpawn");
		buildingPlacementSystem.enabled = false;
	}

	private void OnEnable()
	{
		buildingPlacementSystem.OnPlacementCancelling += DisablePlacementSystem;
		debugSpawnAction.performed += SpawnConstruction;
	}

	private void OnDisable()
	{
		buildingPlacementSystem.OnPlacementCancelling -= DisablePlacementSystem;
		debugSpawnAction.performed -= SpawnConstruction;
	}

	private void SpawnConstruction(InputAction.CallbackContext context)
	{
		EnablePlacementSystem();
		buildingPlacementSystem.SpawnConstruction(currConstructionAsset.Foundation);
	}

	private void EnablePlacementSystem() => buildingPlacementSystem.enabled = true;

	private void DisablePlacementSystem() => buildingPlacementSystem.enabled = false;
}
