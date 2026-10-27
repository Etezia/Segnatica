using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Transform backpackPanelTransform;
	[SerializeField] private Transform hotbarTransform;
    [SerializeField] private InventoryCell inventoryCellPrefab;

	[SerializeField] private NaturalNeedsSystem playerNeedsSystem;

	[field: SerializeField] public int HotbarSize { get; private set; } = 8;
	[field: SerializeField] public int BackpackSize { get; private set; } = 35;

	private int inventorySize => HotbarSize + BackpackSize;
	private int lastOccupiedSlotIndex = -1;

	public List<SlotEntry> SlotEntries { get; private set; }
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
		SlotEntries = new List<SlotEntry>();
		itemCountById = new Dictionary<int, int>();

		for (var i = 0; i < HotbarSize + BackpackSize; i++)
		{
			var cell = Instantiate(inventoryCellPrefab, i < HotbarSize ? hotbarTransform : backpackPanelTransform);
			SlotEntries.Add(new SlotEntry(cell));
			cell.Index = i;
		}
		SlotsCount = BackpackSize;
		lastOccupiedSlotIndex = -1;
	}

	private void OnEnable()
	{
		foreach (var entry in SlotEntries)
		{
			entry.Cell.RMBClicked += DropItemsFromSlot;
			entry.Cell.LMBClicked += TryUseItem;
			entry.Cell.PointerEntered += OnPointerEnteredCell;
			entry.Cell.PointerExited += OnPointerExitedCell;
		}
	}

	private void OnDisable()
	{
		foreach (var entry in SlotEntries)
		{
			entry.Cell.RMBClicked -= DropItemsFromSlot;
			entry.Cell.LMBClicked -= TryUseItem;
			entry.Cell.PointerEntered -= OnPointerEnteredCell;
			entry.Cell.PointerExited -= OnPointerExitedCell;
		}
	}

	private void Reset()
	{
		playerNeedsSystem = GameObject.FindWithTag("Player").GetComponent<NaturalNeedsSystem>();

		HotbarSize = 8;
		BackpackSize = 35;
	}

	public int AddItemsIfPossible(ItemData item, int itemsCount)
	{
		var isStackable = item.IsStackable;

		if (!itemCountById.ContainsKey(item.Id))
			itemCountById.Add(item.Id, 0);

		for (var i = item.IsEquipable ? 0 : HotbarSize; i < inventorySize; i++)
		{
			var currSlotEntry = SlotEntries[i];
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
				lastOccupiedSlotIndex = i;

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
				itemCountById[item.Id] += currSlotEntry.ItemsCount - initSlotItemsCount;
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

	private void DropItemsFromSlot(int index)
	{
		var entry = SlotEntries[index];
		if (entry.Item == null)
			return;

		var cell = entry.Cell;

		var itemInstance = Instantiate(entry.Item.PhysicalObject, PlayerSockets.Instance.ItemDropPoint.position, Quaternion.identity);
		itemInstance.Count = entry.ItemsCount;

		entry.Item = null;
		entry.ItemsCount = 0;

		UsedSlotsCount--;
		lastOccupiedSlotIndex = index - 1;

		SlotUpdated?.Invoke(entry);
		ItemDropped?.Invoke(entry);
		InventoryChanged?.Invoke();
	}

	public bool TryRemoveItems(int id, int count)
	{
		if (!itemCountById.ContainsKey(id) || itemCountById[id] < count)
			return false;
		else
			itemCountById[id] -= count;

			for (var i = lastOccupiedSlotIndex; i >= 0; i--)
			{
				var currEntry = SlotEntries[i];
				if (currEntry.Item && currEntry.Item.Id == id)
				{
					if (currEntry.ItemsCount <= count)
					{
						count -= currEntry.ItemsCount;
						currEntry.ItemsCount = 0;
						currEntry.Item = null;
						UsedSlotsCount--;
						lastOccupiedSlotIndex = i - 1;

						SlotUpdated?.Invoke(currEntry);
						InventoryChanged?.Invoke();
						continue;
					}
					else
					{
						currEntry.ItemsCount -= count;
						lastOccupiedSlotIndex = i;

						SlotUpdated?.Invoke(currEntry);
						InventoryChanged?.Invoke();
						break;
					}
				}
			}

		return true;
	}

	private void OnPointerEnteredCell(int index)
	{
		if (SlotEntries[index].Item)
			PointerEnteredCell?.Invoke(SlotEntries[index].Item);
	}

	private void OnPointerExitedCell(int index)
	{
		if (SlotEntries[index].Item)
			PointerExitedCell?.Invoke(SlotEntries[index].Item);
	}

	private void TryUseItem(int index)
	{
		var item = SlotEntries[index].Item;
		if (item && item.IsUsable)
		{
			if (item is ConsumableItemData)
			{
				playerNeedsSystem.IncreaseSatiety((item as ConsumableItemData).HungerRecovery);
				playerNeedsSystem.IncreaseHydration((item as ConsumableItemData).ThirstRecovery);
			}

			RefreshItemDataAfterUsing(SlotEntries[index]);
		}
	}

	private void RefreshItemDataAfterUsing(SlotEntry slotEntry)
	{
		slotEntry.ItemsCount--;

		if (slotEntry.ItemsCount == 0)
		{
			slotEntry.Item = null;
			UsedSlotsCount--;
			lastOccupiedSlotIndex = slotEntry.Cell.Index - 1;
		}

		SlotUpdated?.Invoke(slotEntry);
		InventoryChanged?.Invoke();
	}
}