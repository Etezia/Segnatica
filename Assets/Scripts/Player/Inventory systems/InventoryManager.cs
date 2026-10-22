using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Transform cellPanel;
    [SerializeField] private InventoryCell inventoryCellPrefab;

    [SerializeField] private int initialCellCount = 35;

	private List<SlotEntry> slotEntries;
	private Dictionary<int, int> itemCountById;

	public int SlotsCount { get; private set; } = 0;
	public int UsedSlotsCount { get; private set; }
	public bool IsFull { get; private set; } = false;

	public event Action OnAddItem;
	public event Action OnDropItem;
	
	private void Awake()
	{
		slotEntries = new List<SlotEntry>();
		itemCountById = new Dictionary<int, int>();

		for (var i = 0; i < initialCellCount; i++)
		{
			var cell = Instantiate(inventoryCellPrefab, cellPanel);
			slotEntries.Add(new SlotEntry(cell));
			cell.Index = i;
		}
		SlotsCount = initialCellCount;
		print(SlotsCount);
	}

	private void OnEnable()
	{
		foreach (var entry in slotEntries)
			entry.Cell.OnRMBClick += RemoveItemsFromSlot;
	}

	private void OnDisable()
	{
		foreach (var entry in slotEntries)
			entry.Cell.OnRMBClick -= RemoveItemsFromSlot;
	}

	public int AddItemsIfPossible(ItemData item, int itemsCount)
	{
		var isStackable = item.IsStackable;

		if (!itemCountById.ContainsKey(item.Id))
			itemCountById.Add(item.Id, 0);

		for (var i = 0; i < SlotsCount; i++)
		{
			var currSlotEntry = slotEntries[i];
			var currCell = currSlotEntry.Cell;

			if (currSlotEntry.Item == null)
			{
				currCell.ItemIcon.gameObject.SetActive(true);
				currCell.ItemIcon.sprite = item.Icon;
				currSlotEntry.Item = item;

				if (isStackable)
				{
					currSlotEntry.ItemsCount = Mathf.Clamp(itemsCount, 0, item.MaxStackSize);
					itemsCount -= currSlotEntry.ItemsCount;
					itemCountById[item.Id] += currSlotEntry.ItemsCount;

					currCell.ItemsCountText.gameObject.SetActive(true);
					currCell.ItemsCountText.text = currSlotEntry.ItemsCount.ToString();
				}
				else
				{
					itemCountById[item.Id]++;
					currSlotEntry.ItemsCount = 1;
					itemsCount -= 1;
				}

				UsedSlotsCount++;

				OnAddItem?.Invoke();
			}
			else
			{
				var initSlotItemsCount = currSlotEntry.ItemsCount;
				if (!isStackable || item.Id != currSlotEntry.Item.Id || initSlotItemsCount == item.MaxStackSize)
					continue;

				currSlotEntry.ItemsCount = Mathf.Clamp(initSlotItemsCount + itemsCount, 0, item.MaxStackSize);
				itemsCount -= currSlotEntry.ItemsCount - initSlotItemsCount;

				currCell.ItemsCountText.text = currSlotEntry.ItemsCount.ToString();
			}

			if (i == SlotsCount - 1 && (!isStackable || isStackable && currSlotEntry.ItemsCount == item.MaxStackSize))
			{
				IsFull = true;
				return itemsCount;
			}

			if (itemsCount == 0)
				break;
		}

		return itemsCount;
	}

	public void RemoveItemsFromSlot(int index)
	{
		var entry = slotEntries[index];
		if (entry.Item == null)
			return;

		var cell = entry.Cell;

		var itemInstance = Instantiate(entry.Item.PhysicalObject, PlayerSockets.Instance.ItemDropPoint.position, Quaternion.identity);
		itemInstance.Count = entry.ItemsCount;

		entry.Item = null;
		entry.ItemsCount = 0;

		entry.Cell.ItemIcon.sprite = null;
		entry.Cell.ItemIcon.gameObject.SetActive(false);
		entry.Cell.ItemsCountText.text = "";
		entry.Cell.ItemsCountText.gameObject.SetActive(false);

		UsedSlotsCount--;

		OnDropItem?.Invoke();
	}

	private void RemoveItems(int count)
	{

	}

	private void AddItemsToSlot(InventoryCell cell, ItemData item)
	{
		
	}
}
//отделение вьюшки от модели предполагается к 3786му году. готовьтесь