using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ConstructionPlacementHandler : MonoBehaviour
{
    [SerializeField] private PlacementData placementData;
	[SerializeField] private Vector3 overlapScaleVector = new Vector3(0.48f, 0.5f, 0.48f);


	private MeshRenderer renderer;
	private BoxCollider collider;

	private Collider[] forbiddenColliders;

	public bool CanBeMounted { get; private set; }

	private void Awake()
	{
		renderer = GetComponent<MeshRenderer>();
		collider = GetComponent<BoxCollider>();
		
		forbiddenColliders = new Collider[1];
		renderer.material = placementData.AllowedMountingMaterial;
	}

	private void Update()
	{
		if (transform.hasChanged)
		{
			var overlappedColliders = 
				Physics.OverlapBoxNonAlloc(transform.position + collider.center, Vector3.Scale(Vector3.Scale(transform.lossyScale, collider.size), overlapScaleVector),
				forbiddenColliders, transform.rotation, ~placementData.AllowedPlacementLayers, QueryTriggerInteraction.Ignore);

			if (overlappedColliders != 0)
			{
				CanBeMounted = false;
				renderer.material = placementData.ForbiddenMountingMaterial;
			}
			else
			{
				CanBeMounted = true;
				renderer.material = placementData.AllowedMountingMaterial;
			}

			transform.hasChanged = false;
		}
	}
}
