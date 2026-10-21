using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJournalUI : MonoBehaviour
{
    [SerializeField] private GameObject journalPanel;
	[SerializeField] private GameObject inventoryPanel;
	[SerializeField] private GameObject craftingPanel;
	[SerializeField] private GameObject bestiaryPanel;

	[SerializeField] private InputActionAsset inputSystem;
	private InputAction openInventoryAction;

	private void Awake()
	{
		openInventoryAction = inputSystem.FindAction("OpenInventory");

		MinimizeAllPanels();
	}

	private void OnEnable()
	{
		openInventoryAction.performed += SwitchInventoryDisplay;
	}

	private void OnDisable()
	{
		openInventoryAction.performed -= SwitchInventoryDisplay;
	}

	private void MinimizeAllPanels()
	{
		journalPanel.SetActive(false);
		inventoryPanel.SetActive(false);
		//craftingPanel.SetActive(false);
		//bestiaryPanel.SetActive(false);
	}

	private void SwitchInventoryDisplay(InputAction.CallbackContext context)
	{
		if (inventoryPanel.activeSelf)
			MinimizeAllPanels();
		else
			ShowInventoryPanel();
	}

	private void ShowInventoryPanel()
	{
		journalPanel.SetActive(true);
		inventoryPanel.SetActive(true);
	}
}
