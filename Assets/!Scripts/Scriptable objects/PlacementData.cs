using UnityEngine;

[CreateAssetMenu(fileName = "PlacementData", menuName = "Scriptable Objects/PlacementData")]
public class PlacementData : ScriptableObject
{
	[field: SerializeField] public LayerMask AllowedPlacementLayers { get; private set; }
	[field: SerializeField] public Material AllowedMountingMaterial { get; private set; }
	[field: SerializeField] public Material ForbiddenMountingMaterial { get; private set; }
	[field: SerializeField] public Vector3 OverlappingColliderScale { get; private set; }
}
