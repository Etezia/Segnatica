using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Inventory Item", menuName = "Scriptable Objects/InventoryItem")]
public class ItemData : ScriptableObject
{
	private static int instatncesCount = 0;

	[field: SerializeField] public int Id { get; protected set; }
	[field: SerializeField] public string Name { get; protected set; }
	[field: SerializeField] public RawImage Icon { get; protected set; }
	[field: SerializeField, TextArea(8, 15)] public string Description { get; protected set; }
	[field: SerializeField] public ItemType Type { get; protected set; }

	[field: SerializeField] public bool IsStackable { get; protected set; }
	[field: SerializeField] public bool IsEquipable { get; protected set; }
	[field: SerializeField] public int MaxStackSize { get; protected set; }

	[field: SerializeField] public GameObject PhysicalObject { get; protected set; }

	public ItemData()
	{
		Type = ItemType.Other;
	}

#if UNITY_EDITOR

#endif
}
