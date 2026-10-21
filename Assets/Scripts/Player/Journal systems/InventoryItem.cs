using UnityEngine;
using UnityEngine.UI;

public class InventoryItem
{
    private ItemData itemData;

    public int Id => itemData.Id;

	public InventoryItem(ItemData itemData)
    {
        this.itemData = itemData;
    }
}
