using UnityEngine;

[CreateAssetMenu(fileName = "NaturalNeedsConfig", menuName = "Scriptable Objects/Natural Needs Data")]
public class NaturalNeedsConfig : ScriptableObject
{
	[field: SerializeField] public float HungerIncreasingCoeff { get; private set; }
	[field: SerializeField] public float ThirstIncreasingCoeff { get; private set; }
	[field: SerializeField] public float MaxSatiety { get; private set; }
	[field: SerializeField] public float MaxHydration { get; private set; }
}
