using UnityEngine;

[CreateAssetMenu(fileName = "PlacementData", menuName = "Scriptable Objects/PlacementData")]
public class PlacementData : ScriptableObject
{
	[field: SerializeField] public LayerMask ForbiddenPlacementLayers { get; private set; }
	[field: SerializeField] public Material AllowedMountingMaterial { get; private set; }
	[field: SerializeField] public Material ForbiddenMountingMaterial { get; private set; }
}
