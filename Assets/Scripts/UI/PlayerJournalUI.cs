using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJournalUI : MonoBehaviour
{
    [SerializeField] private GameObject journalPanel;
	[SerializeField] private GameObject inventoryPanel;
	[SerializeField] private GameObject craftingPanel;
	[SerializeField] private GameObject bestiaryPanel;

	[SerializeField] private InputActionAsset inputSystem;
	[SerializeField] private InputAction openInventoryAction;

	private void Awake()
	{
		openInventoryAction = inputSystem.FindAction("OpenInventory");
	}
}
