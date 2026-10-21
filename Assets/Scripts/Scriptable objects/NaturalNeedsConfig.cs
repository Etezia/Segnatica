using UnityEngine;

[CreateAssetMenu(fileName = "Natural Needs Config", menuName = "Scriptable Objects/NaturalNeedsData")]
public class NaturalNeedsConfig : ScriptableObject
{
	[field: SerializeField] public float HungerIncreasingCoeff { get; private set; }
	[field: SerializeField] public float ThurstIncreasingCoeff { get; private set; }
	[field: SerializeField] public float MaxSatiety { get; private set; }
	[field: SerializeField] public float MaxHydration { get; private set; }
}
