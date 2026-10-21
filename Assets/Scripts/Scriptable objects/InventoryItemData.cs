using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Item", menuName = "Scriptable Objects/InventoryItem")]
public class InventoryItemData : ScriptableObject
{
	[field: SerializeField] public int Id { get; private set; }
	[field: SerializeField] public string Name { get; private set; }
	[field: SerializeField] public string Description { get; private set; }

	[field: SerializeField] public bool IsStackable { get; private set; }
	[field: SerializeField] public bool IsEquipable { get; private set; }
	[field: SerializeField] public int MaxStackSize { get; private set; }
	[field: SerializeField] public GameObject PhysicalObject { get; private set; }
}
