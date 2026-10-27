using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(InventoryManager))]
public class HotbarManager : MonoBehaviour
{
	[SerializeField] private PlayerSockets playerSockets;
	[SerializeField] private InventoryManager inventoryManager;
	private Transform weaponHolderPoint;

	[SerializeField] private Image itemHighlightImage;
	private SlotEntry currActiveSlot;
	private PickableItem currEquippedItem;

    [SerializeField] private InputActionAsset inputSystem;
	private InputAction selectHotbarSlot1;
	private InputAction selectHotbarSlot2;
	private InputAction selectHotbarSlot3;
	private InputAction selectHotbarSlot4;
	private InputAction selectHotbarSlot5;
	private InputAction selectHotbarSlot6;
	private InputAction selectHotbarSlot7;
	private InputAction selectHotbarSlot8;

	private void Awake()
	{
		weaponHolderPoint = playerSockets.WeaponHolderPoint;
		inventoryManager = GetComponent<InventoryManager>();

		selectHotbarSlot1 = inputSystem.FindAction("SelectHotbarSlot1");
		selectHotbarSlot2 = inputSystem.FindAction("SelectHotbarSlot2");
		selectHotbarSlot3 = inputSystem.FindAction("SelectHotbarSlot3");
		selectHotbarSlot4 = inputSystem.FindAction("SelectHotbarSlot4");
		selectHotbarSlot5 = inputSystem.FindAction("SelectHotbarSlot5");
		selectHotbarSlot6 = inputSystem.FindAction("SelectHotbarSlot6");
		selectHotbarSlot7 = inputSystem.FindAction("SelectHotbarSlot7");
		selectHotbarSlot8 = inputSystem.FindAction("SelectHotbarSlot8");
	}

	private void OnEnable()
	{
		selectHotbarSlot1.performed += SelectHotbarItem1;
		selectHotbarSlot2.performed += SelectHotbarItem2;
		selectHotbarSlot3.performed += SelectHotbarItem3;
		selectHotbarSlot4.performed += SelectHotbarItem4;
		selectHotbarSlot5.performed += SelectHotbarItem5;
		selectHotbarSlot6.performed += SelectHotbarItem6;
		selectHotbarSlot7.performed += SelectHotbarItem7;
		selectHotbarSlot8.performed += SelectHotbarItem8;
	}

	private void OnDisable()
	{
		selectHotbarSlot1.performed -= SelectHotbarItem1;
		selectHotbarSlot2.performed -= SelectHotbarItem2;
		selectHotbarSlot3.performed -= SelectHotbarItem3;
		selectHotbarSlot4.performed -= SelectHotbarItem4;
		selectHotbarSlot5.performed -= SelectHotbarItem5;
		selectHotbarSlot6.performed -= SelectHotbarItem6;
		selectHotbarSlot7.performed -= SelectHotbarItem7;
		selectHotbarSlot8.performed -= SelectHotbarItem8;
	}

	private void SelectHotbarItem1(InputAction.CallbackContext context) => SelectHotbarItem(0);

	private void SelectHotbarItem2(InputAction.CallbackContext context) => SelectHotbarItem(1);

	private void SelectHotbarItem3(InputAction.CallbackContext context) => SelectHotbarItem(2);

	private void SelectHotbarItem4(InputAction.CallbackContext context) => SelectHotbarItem(3);

	private void SelectHotbarItem5(InputAction.CallbackContext context) => SelectHotbarItem(4);

	private void SelectHotbarItem6(InputAction.CallbackContext context) => SelectHotbarItem(5);

	private void SelectHotbarItem7(InputAction.CallbackContext context) => SelectHotbarItem(6);

	private void SelectHotbarItem8(InputAction.CallbackContext context) => SelectHotbarItem(7);

	private void SelectHotbarItem(int index)
    {
		if (inventoryManager.HotbarSize < index)
			return;

		if (currEquippedItem)
		{
			Destroy(currEquippedItem.gameObject);
			currEquippedItem = null;
		}

		var currActiveSlot = inventoryManager.SlotEntries[index];

		if (!itemHighlightImage.gameObject.activeSelf)
			itemHighlightImage.gameObject.SetActive(true);
		itemHighlightImage.transform.position = currActiveSlot.Cell.transform.position;

		var currItem = currActiveSlot.Item;
		if (currItem.Type == ItemType.Weapon || currItem.Type == ItemType.Tool)
		{
			currEquippedItem = Instantiate(currItem.PhysicalObject, weaponHolderPoint);
			currEquippedItem.transform.localPosition = Vector3.zero;
			currEquippedItem.transform.localRotation = Quaternion.Euler(Vector3.zero);

			currEquippedItem.gameObject.layer = 0;
			currEquippedItem.GetComponent<PickableItem>().enabled = false;
			currEquippedItem.GetComponent<InteractableItem>().enabled = false;

			if (currEquippedItem.TryGetComponent<Animator>(out var animator))
				animator.enabled = true;
			if (currEquippedItem.TryGetComponent<MeleeWeapon>(out var weaponScript))
				weaponScript.enabled = true;
		}
	}

	private void DeselectHotbarItem()
	{
		if (itemHighlightImage.gameObject.activeSelf)
			itemHighlightImage.gameObject.SetActive(false);

		if (currEquippedItem)
		{
			Destroy(currEquippedItem.gameObject);
			currEquippedItem = null;
		}
	}
}
