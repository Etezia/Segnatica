using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ConstructionInfo", menuName = "Scriptable Objects/Constructions Info")]
public class ConstructionInfo : ScriptableObject
{
	[field: SerializeField] public string Name { get; private set; }
	[field: SerializeField] public Sprite Icon { get; private set; }
	[field: SerializeField] public ConstructionType Type { get; private set; }
	[field: SerializeField] public ConstructionPlacementHandler PreviewObject { get; private set; }
	[field: SerializeField] public Construction ConstructionObject { get; private set; }
}
