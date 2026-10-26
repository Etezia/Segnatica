using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Transform backpackPanelTransform;
	[SerializeField] private Transform hotbarTransform;
    [SerializeField] private InventoryCell inventoryCellPrefab;

	[SerializeField] private int hotbarSize = 8;
	[SerializeField] private int backpackSize = 35;
	private int inventorySize => hotbarSize + backpackSize;

	private List<SlotEntry> slotEntries;
	private Dictionary<int, int> itemCountById;

	public int SlotsCount { get; private set; } = 0;
	public int UsedSlotsCount { get; private set; }
	public bool IsFull { get; private set; } = false;

	public event Action InventoryChanged;

	public event Action<SlotEntry> ItemAdded;
	public event Action<SlotEntry> ItemDropped;
	public event Action<SlotEntry> SlotUpdated;

	public event Action<ItemData> PointerEnteredCell;
	public event Action<ItemData> PointerExitedCell;

	private void Awake()
	{
		slotEntries = new List<SlotEntry>();
		itemCountById = new Dictionary<int, int>();

		for (var i = 0; i < hotbarSize + backpackSize; i++)
		{
			var cell = Instantiate(inventoryCellPrefab, i < hotbarSize ? hotbarTransform : backpackPanelTransform);
			slotEntries.Add(new SlotEntry(cell));
			cell.Index = i;
		}
		SlotsCount = backpackSize;
	}

	private void OnEnable()
	{
		foreach (var entry in slotEntries)
		{
			entry.Cell.RMBClicked += RemoveItemsFromSlot;
			entry.Cell.PointerEntered += OnPointerEnteredCell;
			entry.Cell.PointerExited += OnPointerExitedCell;
		}
	}

	private void OnDisable()
	{
		foreach (var entry in slotEntries)
		{
			entry.Cell.RMBClicked -= RemoveItemsFromSlot;
			entry.Cell.PointerEntered -= OnPointerEnteredCell;
			entry.Cell.PointerExited -= OnPointerExitedCell;
		}
	}

	public int AddItemsIfPossible(ItemData item, int itemsCount)
	{
		var isStackable = item.IsStackable;

		if (!itemCountById.ContainsKey(item.Id))
			itemCountById.Add(item.Id, 0);

		for (var i = item.IsEquipable ? 0 : hotbarSize; i < inventorySize; i++)
		{
			var currSlotEntry = slotEntries[i];
			var currCell = currSlotEntry.Cell;

			if (currSlotEntry.Item == null)
			{
				currSlotEntry.Item = item;

				if (isStackable)
				{
					currSlotEntry.ItemsCount = Mathf.Clamp(itemsCount, 0, item.MaxStackSize);
					itemsCount -= currSlotEntry.ItemsCount;
					itemCountById[item.Id] += currSlotEntry.ItemsCount;
				}
				else
				{
					itemCountById[item.Id]++;
					currSlotEntry.ItemsCount = 1;
					itemsCount -= 1;
				}

				UsedSlotsCount++;

				SlotUpdated?.Invoke(currSlotEntry);
				ItemAdded?.Invoke(currSlotEntry);
				InventoryChanged?.Invoke();
			}
			else
			{
				var initSlotItemsCount = currSlotEntry.ItemsCount;
				if (!isStackable || item.Id != currSlotEntry.Item.Id || initSlotItemsCount == item.MaxStackSize)
					continue;

				currSlotEntry.ItemsCount = Mathf.Clamp(initSlotItemsCount + itemsCount, 0, item.MaxStackSize);
				itemsCount -= currSlotEntry.ItemsCount - initSlotItemsCount;

				currCell.ItemsCountText.text = currSlotEntry.ItemsCount.ToString();

				SlotUpdated?.Invoke(currSlotEntry);
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
		print("go remove");
		var entry = slotEntries[index];
		if (entry.Item == null)
			return;

		var cell = entry.Cell;

		var itemInstance = Instantiate(entry.Item.PhysicalObject, PlayerSockets.Instance.ItemDropPoint.position, Quaternion.identity);
		itemInstance.Count = entry.ItemsCount;
		print("dropped");

		entry.Item = null;
		entry.ItemsCount = 0;

		UsedSlotsCount--;

		SlotUpdated?.Invoke(entry);
		ItemDropped?.Invoke(entry);
		InventoryChanged?.Invoke();
	}

	private void RemoveItems(int count)
	{

	}

	private void OnPointerEnteredCell(int index)
	{
		if (slotEntries[index].Item)
			PointerEnteredCell?.Invoke(slotEntries[index].Item);
	}

	private void OnPointerExitedCell(int index)
	{
		if (slotEntries[index].Item)
			PointerExitedCell?.Invoke(slotEntries[index].Item);
	}
}