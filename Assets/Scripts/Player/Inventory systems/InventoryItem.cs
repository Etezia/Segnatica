using UnityEngine;
using UnityEngine.UI;

public class InventoryItem
{
    public ItemData ItemData { get; private set; }

    public int Id => ItemData.Id;

	public InventoryItem(ItemData itemData) => ItemData = itemData;
}
