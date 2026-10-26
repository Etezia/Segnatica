using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(BoxCollider))]
public class AttachmentPointsHolder : MonoBehaviour
{
	[SerializeField] private Transform FoundationAttachmentPoint;
	[SerializeField] private Transform VerticalPartitionAttachmentPoint;
	[SerializeField] private Transform HorizontalPartitionAttachmentPoint;

	public Dictionary<ConstructionType, Transform> AttachmentPointsDict { get; private set; }

	private void Awake()
	{
		AttachmentPointsDict = new Dictionary<ConstructionType, Transform>();

		if (FoundationAttachmentPoint)
			AttachmentPointsDict.Add(ConstructionType.Foundation, FoundationAttachmentPoint);
		if (VerticalPartitionAttachmentPoint)
			AttachmentPointsDict.Add(ConstructionType.VerticalPartition, VerticalPartitionAttachmentPoint);
		if (HorizontalPartitionAttachmentPoint)
			AttachmentPointsDict.Add(ConstructionType.HorizontalPartition, HorizontalPartitionAttachmentPoint);
	}
}
