using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItemData", menuName = "Scriptable Objects/Consumable Item Data")]
public class ConsumableItemData : ItemData
{
	[field: SerializeField] public int HungerRecovery { get; private set; }
	[field: SerializeField] public int ThirstRecovery { get; private set; }

	public ConsumableItemData()
	{
		Type = ItemType.Consumable;
	}
}
