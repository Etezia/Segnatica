using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Transform cellPanel;
    [SerializeField] private InventoryCell inventoryCellPrefab;

    [SerializeField] private int initialCellCount = 35;
	private int currCellIndex = 0;

	private List<SlotEntry> slotEntries;

	public bool IsFull { get; private set; } = false;
	
	private void Awake()
	{
		slotEntries = new List<SlotEntry>();

		for (var i = 0; i < initialCellCount; i++)
		{
			var cell = Instantiate(inventoryCellPrefab, cellPanel);
			slotEntries.Add(new SlotEntry(cell));

			currCellIndex = i;
			cell.Index = currCellIndex;
		}
	}
	
	public void AddItem(InventoryItem item, int itemsCount)
	{
		var slotsCount = slotEntries.Count;
		var currItem = item.ItemData;

		for (var i = 0; i < slotsCount; i++)
		{
			var currSlotEntry = slotEntries[i];
			var currCell = currSlotEntry.Cell;
			var slotItem = currSlotEntry.Item?.ItemData;

			if (slotItem == null)
			{
				currCell.ItemIcon.gameObject.SetActive(true);
				currCell.ItemIcon.sprite = currItem.Icon;
				currSlotEntry.Item = item;

				if (currItem.IsStackable)
				{
					currSlotEntry.ItemsCount = Mathf.Clamp(itemsCount, 0, currItem.MaxStackSize);
					itemsCount -= currSlotEntry.ItemsCount;
				}

				if (!currItem.IsStackable || itemsCount == 0)
					break;
			}
			else
			{
				if (!currItem.IsStackable || currSlotEntry.ItemsCount + itemsCount >= slotItem.MaxStackSize)
					continue;


			}

			if (i == slotsCount - 1)
				IsFull = true;
		}
	}

	public void RemoveItem()
	{

	}

	private void AddItemsToSlot(InventoryCell cell, ItemData item)
	{
		
	}
}
//отделение вьюшки от модеши предполагается к 3786му году. готовьтесь