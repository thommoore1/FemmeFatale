using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;
    CanvasGroup canvasGroup;
    Transform dragLayer;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (dragLayer == null)
            dragLayer = GetComponentInParent<Canvas>().rootCanvas.transform;

        originalParent = transform.parent;
        transform.SetParent(dragLayer);
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        InventorySlot originalSlot = originalParent.GetComponent<InventorySlot>();
        InventorySlot dropSlot = eventData.pointerEnter != null
            ? eventData.pointerEnter.GetComponentInParent<InventorySlot>()
            : null;

        if (dropSlot != null && dropSlot != originalSlot)
        {
            if (dropSlot.currentItem != null) // swap
            {
                GameObject other = dropSlot.currentItem;
                other.transform.SetParent(originalSlot.transform);
                other.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                originalSlot.currentItem = other;
            }
            else
            {
                originalSlot.currentItem = null;
            }

            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            transform.SetParent(originalParent); // snap back
        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}
