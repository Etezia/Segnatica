using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BuildingPlacementSystem))]
public class BuildManager : MonoBehaviour
{
	[SerializeField] private BuildingPlacementSystem buildingPlacementSystem;
	[SerializeField] private ConstructionsAsset currConstructionAsset;

	[SerializeField] private InputActionAsset inputSystem;
	private InputAction debugFoundationSpawnAction;
	private InputAction debugWallSpawnAction;
	private InputAction debugFloorSpawnAction;

	private void Awake()
	{
		debugFoundationSpawnAction = inputSystem.FindAction("DebugFoundationSpawn");
		debugWallSpawnAction = inputSystem.FindAction("DebugWallSpawn");
		debugFloorSpawnAction = inputSystem.FindAction("DebugFloorSpawn");

		buildingPlacementSystem.enabled = false;
	}

	private void OnEnable()
	{
		buildingPlacementSystem.OnPlacementCancelling += DisablePlacementSystem;
		debugFoundationSpawnAction.performed += SpawnFoundation;
		debugWallSpawnAction.performed += SpawnWall;
		debugFloorSpawnAction.performed += SpawnFloor;
	}

	private void OnDisable()
	{
		buildingPlacementSystem.OnPlacementCancelling -= DisablePlacementSystem;
		debugFoundationSpawnAction.performed -= SpawnFoundation;
		debugWallSpawnAction.performed -= SpawnWall;
		debugFloorSpawnAction.performed -= SpawnFloor;
	}

	private void SpawnFoundation(InputAction.CallbackContext context)
	{
		ResetPlacementSystem();
		buildingPlacementSystem.SpawnConstruction(currConstructionAsset.Foundation);
	}

	private void SpawnWall (InputAction.CallbackContext context)
	{
		ResetPlacementSystem();
		buildingPlacementSystem.SpawnConstruction(currConstructionAsset.Wall);
	}

	private void SpawnFloor(InputAction.CallbackContext context)
	{
		ResetPlacementSystem();
		buildingPlacementSystem.SpawnConstruction(currConstructionAsset.Floor);
	}

	private void ResetPlacementSystem()
	{
		if (buildingPlacementSystem.enabled)
			buildingPlacementSystem.CancelPlacemnent(new InputAction.CallbackContext());
		EnablePlacementSystem();
	}

	private void EnablePlacementSystem() => buildingPlacementSystem.enabled = true;

	private void DisablePlacementSystem() => buildingPlacementSystem.enabled = false;
}
