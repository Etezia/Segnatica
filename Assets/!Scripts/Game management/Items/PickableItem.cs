using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(InteractableItem))]
public class PickableItem : MonoBehaviour
{
    [field: SerializeField] public ItemData ItemData { get; private set; }
	[field: SerializeField] public int Count { get; set; }

	private void Awake() => Count = 1;
}
