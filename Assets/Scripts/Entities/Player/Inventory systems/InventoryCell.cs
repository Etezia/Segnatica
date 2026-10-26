using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour, IPointerClickHandler , IPointerEnterHandler, IPointerExitHandler
{
    [field: SerializeField] public Image ItemIcon { get; private set; }
    [field: SerializeField] public TMP_Text ItemsCountText { get; private set; }

    public int Index { get; set; }

    public event Action<int> RMBClicked;
    public event Action<int> PointerEntered;
    public event Action<int> PointerExited;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
			RMBClicked?.Invoke(Index);
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEntered?.Invoke(Index);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExited?.Invoke(Index);
    }
}
