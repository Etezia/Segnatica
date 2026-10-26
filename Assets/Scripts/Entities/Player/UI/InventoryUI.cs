using System;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;

	[SerializeField] private TMP_Text filledCellsCountText;

	private void OnEnable()
	{
		RefreshFilledCellsCountText();

		inventoryManager.InventoryChanged += RefreshFilledCellsCountText;
	}
	
	private void OnDisable()
	{
		inventoryManager.InventoryChanged -= RefreshFilledCellsCountText;
	}

	private void RefreshFilledCellsCountText() => 
		filledCellsCountText.text = $"Slots used: {inventoryManager.UsedSlotsCount} / {inventoryManager.SlotsCount}";
}