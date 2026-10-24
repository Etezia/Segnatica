using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(InventoryManager))]
public class InventoryView : MonoBehaviour
{
    private InventoryManager inventoryManager;

	private void Awake()
	{
		inventoryManager = GetComponent<InventoryManager>();
	}

	private void OnEnable()
	{
		inventoryManager.OnAddItem += ActivateCell;
		inventoryManager.OnDropItem += DisableCell;
		inventoryManager.OnUpdateSlot += UpdateCell;
	}

	private void OnDisable()
	{
		inventoryManager.OnAddItem -= ActivateCell;
		inventoryManager.OnDropItem -= DisableCell;
		inventoryManager.OnUpdateSlot -= UpdateCell;
	}

	private void ActivateCell(SlotEntry slotEntry)
	{
		if (slotEntry.Item.IsStackable)
			slotEntry.Cell.ItemsCountText.gameObject.SetActive(true);
		slotEntry.Cell.ItemIcon.gameObject.SetActive(true);
	}

	private void DisableCell(SlotEntry slotEntry)
	{
		slotEntry.Cell.ItemsCountText.gameObject.SetActive(false);
		slotEntry.Cell.ItemIcon.gameObject.SetActive(false);
	}

	private void UpdateCell(SlotEntry slotEntry)
	{
		if (slotEntry.Item != null)
		{
			slotEntry.Cell.ItemsCountText.text = slotEntry.ItemsCount.ToString();
			slotEntry.Cell.ItemIcon.sprite = slotEntry.Item.Icon;
		}
		else
		{
			slotEntry.Cell.ItemsCountText.text = "";
			slotEntry.Cell.ItemIcon.sprite = null;
		}
	}
}
