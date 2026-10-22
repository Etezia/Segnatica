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

		inventoryManager.OnAddItem += RefreshFilledCellsCountText;
		inventoryManager.OnDropItem += RefreshFilledCellsCountText;
	}
	
	private void OnDisable()
	{
		inventoryManager.OnAddItem -= RefreshFilledCellsCountText;
		inventoryManager.OnDropItem -= RefreshFilledCellsCountText;
	}

	private void RefreshFilledCellsCountText() => 
		filledCellsCountText.text = $"Slots used: {inventoryManager.UsedSlotsCount} / {inventoryManager.SlotsCount}";
}