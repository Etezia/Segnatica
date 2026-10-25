using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(BoxCollider))]
public class BracingPointsHolder : MonoBehaviour
{
	[SerializeField] private Transform FoundationBracingPoint;
	[SerializeField] private Transform VerticalPartitionBracingPoint;
	[SerializeField] private Transform HorizontalPartitionBracingPoint;

	public Dictionary<ConstructionType, Transform> BracingPointsDict { get; private set; }

	private void Awake()
	{
		BracingPointsDict = new Dictionary<ConstructionType, Transform>();

		if (FoundationBracingPoint)
			BracingPointsDict.Add(ConstructionType.Foundation, FoundationBracingPoint);
		if (VerticalPartitionBracingPoint)
			BracingPointsDict.Add(ConstructionType.VerticalPartition, VerticalPartitionBracingPoint);
		if (HorizontalPartitionBracingPoint)
			BracingPointsDict.Add(ConstructionType.HorizontalPartition, HorizontalPartitionBracingPoint);
	}
}
