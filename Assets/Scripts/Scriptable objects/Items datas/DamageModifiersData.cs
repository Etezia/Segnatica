using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageModifiersData", menuName = "Scriptable Objects/Damage Modifiers Data")]
public class DamageModifiersData : ScriptableObject
{
	[SerializeField] private float playerDamageMod;
	[SerializeField] private float basicEnemyDamageMod;
	[SerializeField] private float vegetationDamageMod;
	[SerializeField] private float mineralDamageMod;
	[SerializeField] private float constructionDamageMod;

	public Dictionary<EntityType, float> DamageModifiers { get; private set; }

	private void Awake()
	{
		DamageModifiers = new Dictionary<EntityType, float>()
		{
			{ EntityType.Player, playerDamageMod },
			{ EntityType.BasicEnemy, basicEnemyDamageMod },
			{ EntityType.Vegetation, vegetationDamageMod },
			{ EntityType.Mineral, mineralDamageMod },
			{ EntityType.Construction, constructionDamageMod }
		};
	}
}
