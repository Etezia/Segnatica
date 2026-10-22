using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/Item Data")]
public class ItemData : ScriptableObject
{
	private static int instatncesCount = 0;

	[field: SerializeField] public int Id { get; protected set; }
	[field: SerializeField] public string Name { get; protected set; }
	[field: SerializeField] public Sprite Icon { get; protected set; }
	[field: SerializeField, TextArea(8, 15)] public string Description { get; protected set; }
	[field: SerializeField] public ItemType Type { get; protected set; }

	[field: SerializeField] public bool IsStackable { get; protected set; }
	[field: SerializeField] public bool IsEquipable { get; protected set; }
	[field: SerializeField] public int MaxStackSize { get; protected set; }

	[field: SerializeField] public PickableItem PhysicalObject { get; protected set; }
	//[field: SerializeField] public Transform ObjectHoldingPoint { get; protected set; }
	//[field: SerializeField] public Transform ObjectUsingAnimation { get; protected set; }

	public ItemData()
	{
		Type = ItemType.Other;
	}
}
