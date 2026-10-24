using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJournalUI : MonoBehaviour
{
	[SerializeField] private PlayerCameraController playerCameraController;

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
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		playerCameraController.enabled = true;

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
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		playerCameraController.enabled = false;

		journalPanel.SetActive(true);
		inventoryPanel.SetActive(true);
	}
}
