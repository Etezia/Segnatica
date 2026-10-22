using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Transform cellPanel;
    [SerializeField] private InventoryCell inventoryCellPrefab;

    [SerializeField] private int initialCellCount = 35;
	private int slotsCount = 0;

	private List<SlotEntry> slotEntries;
	private Dictionary<int, int> itemCountById;

	public bool IsFull { get; private set; } = false;
	
	private void Awake()
	{
		slotEntries = new List<SlotEntry>();
		itemCountById = new Dictionary<int, int>();

		for (var i = 0; i < initialCellCount; i++)
		{
			var cell = Instantiate(inventoryCellPrefab, cellPanel);
			slotEntries.Add(new SlotEntry(cell));
			cell.Index = slotsCount;
		}
		slotsCount = initialCellCount;
	}
	
	public int AddItemsIfPossible(InventoryItem item, int itemsCount)
	{
		print("add");
		var currItem = item.ItemData;
		var isStackable = currItem.IsStackable;

		if (!itemCountById.ContainsKey(currItem.Id))
			itemCountById.Add(currItem.Id, 0);

		for (var i = 0; i < slotsCount; i++)
		{
			var currSlotEntry = slotEntries[i];
			var currCell = currSlotEntry.Cell;

			if (currSlotEntry.Item == null)
			{
				currCell.ItemIcon.gameObject.SetActive(true);
				currCell.ItemIcon.sprite = currItem.Icon;
				currSlotEntry.Item = item;

				if (isStackable)
				{
					currSlotEntry.ItemsCount = Mathf.Clamp(itemsCount, 0, currItem.MaxStackSize);
					itemsCount -= currSlotEntry.ItemsCount;
					itemCountById[currItem.Id] += currSlotEntry.ItemsCount;

					currCell.ItemsCountText.gameObject.SetActive(true);
					currCell.ItemsCountText.text = currSlotEntry.ItemsCount.ToString();
				}
				else
				{
					itemCountById[currItem.Id]++;
					currSlotEntry.ItemsCount = 1;
					itemsCount -= 1;
				}
			}
			else
			{
				var initSlotItemsCount = currSlotEntry.ItemsCount;
				if (!isStackable || currItem.Id != currSlotEntry.Item.Id || 
					initSlotItemsCount + itemsCount > currItem.MaxStackSize)
					continue;

				currSlotEntry.ItemsCount = Mathf.Clamp(initSlotItemsCount + itemsCount, 0, currItem.MaxStackSize);
				itemsCount -= currSlotEntry.ItemsCount - initSlotItemsCount;

				currCell.ItemsCountText.text = currSlotEntry.ItemsCount.ToString();
			}

			if (i == slotsCount - 1 && (!isStackable || isStackable && currSlotEntry.ItemsCount == currItem.MaxStackSize))
			{
				IsFull = true;
				print("full");
				return itemsCount;
			}

			if (itemsCount == 0)
				break;
		}

		return itemsCount;
	}

	public void RemoveItemsFromSlot(int index)
	{

	}

	private void RemoveItems(int count)
	{

	}

	private void AddItemsToSlot(InventoryCell cell, ItemData item)
	{
		
	}
}
//отделение вьюшки от модеши предполагается к 3786му году. готовьтесь