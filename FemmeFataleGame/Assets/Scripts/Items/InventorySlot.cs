using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public GameObject currentItem; //the item current held in this slot
    [HideInInspector] public InventoryController controller;

    Image background;
    Color normalColor;

    void Awake()
    {
        background = GetComponent<Image>();
        if (background != null) normalColor = background.color;
    }
    
    public bool IsEmpty
    {
        get
        {
            // Empty if there's no item, it was destroyed, or it was moved out of this slot
            if (currentItem == null || currentItem.transform.parent != transform)
            {
                currentItem = null;
                return true;
            }
            return false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        controller.SelectSlot(this);
    }

    public void SetSelected(bool selected)
    {
        if (background != null)
            background.color = selected ? Color.pink : normalColor;
    }
    
    
}
