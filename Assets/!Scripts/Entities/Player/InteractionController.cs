using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;

	[SerializeField] private Material selectionHighlightShader;
    [SerializeField] private LayerMask includedLayers;
	[SerializeField] private LayerMask interactableLayers;
	[SerializeField] private float maxInteractDistance;

    [SerializeField] private InputActionAsset inputSystem;
    private InputAction interactAction;

	private Collider currTargetCollider;
	private GameObject currTarget;

	private void Awake() => interactAction = inputSystem.FindAction("Interact");

	private void OnEnable() => interactAction.performed += Interact;

	private void OnDisable() => interactAction.performed -= Interact;

	private void Update()
	{
		if (Physics.Raycast(transform.position, transform.forward, out var hitInfo, maxInteractDistance, includedLayers))
		{
			if (hitInfo.collider != currTargetCollider)
			{
				if (currTarget && currTarget.TryGetComponent<InteractableItem>(out var interactableItem))
					interactableItem.ResetMaterials();

				currTargetCollider = hitInfo.collider;

				if ((1 << currTargetCollider.gameObject.layer & interactableLayers) != 0)
				{
					currTarget = currTargetCollider.gameObject;
					currTarget.GetComponent<InteractableItem>().AddMaterial(selectionHighlightShader);
				}
				else
				{
					currTarget = null;
				}
			}
		}
		else
		{
			if (currTarget && currTarget.TryGetComponent<InteractableItem>(out var interactableItem))
				interactableItem.ResetMaterials();

			currTargetCollider = null;
			currTarget = null;
		}
	}

	private void Interact(InputAction.CallbackContext context)
	{
		if (currTarget == null)
			return;

		if (currTarget.TryGetComponent<PickableItem>(out var pickableItemComponent))
		{
			if (!inventoryManager.IsFull)
			{
				var itemsLeft = inventoryManager.AddItemsIfPossible(pickableItemComponent.ItemData, pickableItemComponent.Count);

				if (itemsLeft == 0)
				{
					Destroy(currTarget);
					currTarget = null;
				}
				else
				{
					pickableItemComponent.Count = itemsLeft;
				}
			}
		}
	}
}