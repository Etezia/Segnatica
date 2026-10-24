using UnityEngine;

[CreateAssetMenu(fileName = "CharacterHealthData", menuName = "Scriptable Objects/Entity Health Data")]
public class EntityHealthData : ScriptableObject
{
	[field: SerializeField] public float MaxHealth { get; private set; }
	[field: SerializeField] public float MaxArmor { get; private set; }
}
