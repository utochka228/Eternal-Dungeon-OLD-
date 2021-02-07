using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    Canvas canvas;
    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    Transform inventoryPanel;
    Slot slot;
    public DropSlot oldSlot;
    

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = PlayerUI.instance.gameCanvas.GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        inventoryPanel = PlayerUI.instance.inventoryPanel;
        slot = GetComponent<Slot>();
        oldSlot = transform.parent.GetComponent<DropSlot>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin drag!");
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
        transform.SetParent(inventoryPanel);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End drag!");
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        if(eventData.pointerCurrentRaycast.gameObject == null){
            slot.DropItem();
            ReturnToPrevPosition();
        }
    }
    public void ReturnToPrevPosition(){
        transform.SetParent(oldSlot.transform);
        rectTransform.anchoredPosition = oldSlot.slotRect.rect.center;
    }
}
