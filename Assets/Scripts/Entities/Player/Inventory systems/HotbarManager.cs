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
	private InputAction deselectHotbarSlot;

	private PickableItem[] hotbarItems;

	private void Awake()
	{
		weaponHolderPoint = playerSockets.WeaponHolderPoint;
		inventoryManager = GetComponent<InventoryManager>();
		hotbarItems = new PickableItem[inventoryManager.HotbarSize];

		selectHotbarSlot1 = inputSystem.FindAction("SelectHotbarSlot1");
		selectHotbarSlot2 = inputSystem.FindAction("SelectHotbarSlot2");
		selectHotbarSlot3 = inputSystem.FindAction("SelectHotbarSlot3");
		selectHotbarSlot4 = inputSystem.FindAction("SelectHotbarSlot4");
		selectHotbarSlot5 = inputSystem.FindAction("SelectHotbarSlot5");
		selectHotbarSlot6 = inputSystem.FindAction("SelectHotbarSlot6");
		selectHotbarSlot7 = inputSystem.FindAction("SelectHotbarSlot7");
		selectHotbarSlot8 = inputSystem.FindAction("SelectHotbarSlot8");
		deselectHotbarSlot = inputSystem.FindAction("DeselectHotbatSlot");
	}

	private void OnEnable()
	{
		inventoryManager.ItemAdded += OnInvenoryItemAdded;
		inventoryManager.ItemDropped += OnInvenoryItemDropped;

		selectHotbarSlot1.performed += SelectHotbarItem1;
		selectHotbarSlot2.performed += SelectHotbarItem2;
		selectHotbarSlot3.performed += SelectHotbarItem3;
		selectHotbarSlot4.performed += SelectHotbarItem4;
		selectHotbarSlot5.performed += SelectHotbarItem5;
		selectHotbarSlot6.performed += SelectHotbarItem6;
		selectHotbarSlot7.performed += SelectHotbarItem7;
		selectHotbarSlot8.performed += SelectHotbarItem8;
		deselectHotbarSlot.performed += DeselectHotbarItem;
	}

	private void OnDisable()
	{
		inventoryManager.ItemAdded -= OnInvenoryItemAdded;
		inventoryManager.ItemDropped -= OnInvenoryItemDropped;

		selectHotbarSlot1.performed -= SelectHotbarItem1;
		selectHotbarSlot2.performed -= SelectHotbarItem2;
		selectHotbarSlot3.performed -= SelectHotbarItem3;
		selectHotbarSlot4.performed -= SelectHotbarItem4;
		selectHotbarSlot5.performed -= SelectHotbarItem5;
		selectHotbarSlot6.performed -= SelectHotbarItem6;
		selectHotbarSlot7.performed -= SelectHotbarItem7;
		selectHotbarSlot8.performed -= SelectHotbarItem8;
		deselectHotbarSlot.performed -= DeselectHotbarItem;
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

		UnequipCurrItem();

		currActiveSlot = inventoryManager.SlotEntries[index];

		if (!itemHighlightImage.gameObject.activeSelf)
			itemHighlightImage.gameObject.SetActive(true);
		itemHighlightImage.transform.position = currActiveSlot.Cell.transform.position;

		if (currActiveSlot.Item != null)
			EquipCurrItem();
	}

	private void EquipCurrItem()
	{
		if (currActiveSlot == null)
			return;

		var currItem = currActiveSlot.Item;
		var currSlotIndex = currActiveSlot.Cell.Index;

		if (hotbarItems[currSlotIndex] != null)
		{
			hotbarItems[currSlotIndex].gameObject.SetActive(true);
			currEquippedItem = hotbarItems[currSlotIndex];
		}
		else
		{
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

				hotbarItems[currSlotIndex] = currEquippedItem;
			}
		}
	}

	private void UnequipCurrItem()
	{
		if (currEquippedItem)
		{
			currEquippedItem.gameObject.SetActive(false);
			currEquippedItem = null;
		}
	}

	private void DeselectHotbarItem(InputAction.CallbackContext context)
	{
		if (itemHighlightImage.gameObject.activeSelf)
			itemHighlightImage.gameObject.SetActive(false);

		currActiveSlot = null;
		UnequipCurrItem();
	}

	private void OnInvenoryItemAdded(SlotEntry entry)
	{
		if (currActiveSlot != null && entry == currActiveSlot && !currEquippedItem)
			EquipCurrItem();
	}

	private void OnInvenoryItemDropped(SlotEntry entry)
	{

		var currSlotIndex = entry.Cell.Index;
		if (currSlotIndex >= inventoryManager.HotbarSize)
			return;

		if (entry == currActiveSlot)
			UnequipCurrItem();

		if (hotbarItems[currSlotIndex] != null)
		{
			Destroy(hotbarItems[currSlotIndex].gameObject);
			hotbarItems[currSlotIndex] = null;
		}
	}
}