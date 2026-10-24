using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ConstructionInfo
{
	[field: SerializeField] public string Name { get; private set; }
	[field: SerializeField] public Sprite Icon { get; private set; }
	[field: SerializeField] public ConstructionPlacementHandler PreviewObject { get; private set; }
	[field: SerializeField] public Construction ConstructionObject { get; private set; }
	[field: SerializeField] public LayerMask AllowedPlacementLayers { get; private set; }
}
