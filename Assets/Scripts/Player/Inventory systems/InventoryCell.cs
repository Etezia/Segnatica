using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour, IPointerClickHandler  
{
    [field: SerializeField] public Image ItemIcon { get; private set; }
    [field: SerializeField] public TMP_Text ItemsCountText { get; private set; }

    public int Index { get; set; }

    public event Action<int> OnRMBClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
			OnRMBClick?.Invoke(Index);
	}
}
