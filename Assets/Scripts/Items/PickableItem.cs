using UnityEngine;

public class PickableItem : MonoBehaviour
{
    [field: SerializeField] public ItemData ItemData { get; private set; }
}
