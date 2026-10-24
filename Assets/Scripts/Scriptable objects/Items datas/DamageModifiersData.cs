using UnityEngine;

[CreateAssetMenu(fileName = "DamageModifiersData", menuName = "Scriptable Objects/Damage Modifiers Data")]
public class DamageModifiersData : ScriptableObject
{
	[field: SerializeField] public float EnemyDamageMod { get; private set; }
	[field: SerializeField] public float VegetationDamageMod { get; private set; }
	[field: SerializeField] public float MineralDamageMod { get; private set; }
	[field: SerializeField] public float ConstructionDamageMod { get; private set; }
}
