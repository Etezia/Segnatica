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
	}

	private void OnEnable()
	{
		debugSpawnAction?.Enable();
		debugSpawnAction.performed += SpawnConstruction;
	}

	private void OnDisable()
	{
		debugSpawnAction.performed -= SpawnConstruction;
		debugSpawnAction?.Disable();
	}

	private void SpawnConstruction(InputAction.CallbackContext context)
	{
		buildingPlacementSystem.SpawnConstruction(currConstructionAsset.Foundation);
	}
}
