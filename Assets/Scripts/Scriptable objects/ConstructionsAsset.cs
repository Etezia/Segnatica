using UnityEngine;

[CreateAssetMenu(fileName = "ConstructionsAsset", menuName = "Scriptable Objects/Constructions Asset")]
public class ConstructionsAsset : ScriptableObject
{
	[field: SerializeField] public ConstructionInfo Foundation { get; private set; }
	[field: SerializeField] public ConstructionInfo Floor { get; private set; }
	[field: SerializeField] public ConstructionInfo Wall { get; private set; }
	[field: SerializeField] public ConstructionInfo Door { get; private set; }
	[field: SerializeField] public ConstructionInfo Window { get; private set; }
	[field: SerializeField] public ConstructionInfo Column { get; private set; }
}
