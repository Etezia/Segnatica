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

		inventoryManager.OnInventoryChange += RefreshFilledCellsCountText;
	}
	
	private void OnDisable()
	{
		inventoryManager.OnInventoryChange -= RefreshFilledCellsCountText;
	}

	private void RefreshFilledCellsCountText() => 
		filledCellsCountText.text = $"Slots used: {inventoryManager.UsedSlotsCount} / {inventoryManager.SlotsCount}";
}