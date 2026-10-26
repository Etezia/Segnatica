using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(InventoryManager))]
public class InventoryView : MonoBehaviour
{
	[SerializeField] private GameObject descriptionPanel;
	[SerializeField] private TMP_Text itemNameText;
	[SerializeField] private TMP_Text itemDescriptionsText;

	private InventoryManager inventoryManager;

	private void Awake()
	{
		inventoryManager = GetComponent<InventoryManager>();
	}

	private void OnEnable()
	{
		//inventoryManager.ItemAdded += ActivateCell;
		//inventoryManager.ItemDropped += DisableCell;
		inventoryManager.SlotUpdated += UpdateCell;

		inventoryManager.PointerEnteredCell += ActivateAndUpdateDescriptionPanel;
		inventoryManager.PointerExitedCell += DisableDescriptionPanel;
	}

	private void OnDisable()
	{
		//inventoryManager.ItemAdded -= ActivateCell;
		//inventoryManager.ItemDropped -= DisableCell;
		inventoryManager.SlotUpdated -= UpdateCell;

		inventoryManager.PointerEnteredCell -= ActivateAndUpdateDescriptionPanel;
		inventoryManager.PointerExitedCell -= DisableDescriptionPanel;
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

			if (!slotEntry.Cell.ItemIcon.gameObject.activeSelf)
				ActivateCell(slotEntry);
		}
		else
		{
			slotEntry.Cell.ItemsCountText.text = "";
			slotEntry.Cell.ItemIcon.sprite = null;

			DisableCell(slotEntry);
		}
	}

	private void ActivateAndUpdateDescriptionPanel(ItemData itemData)
	{
		itemNameText.text = itemData.Name;
		itemDescriptionsText.text = itemData.Description;
		descriptionPanel.SetActive(true);
	}

	private void DisableDescriptionPanel(ItemData itemData) => descriptionPanel.SetActive(false);
}