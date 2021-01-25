using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
public class DropSlot : MonoBehaviour, IDropHandler
{
    [HideInInspector] public RectTransform slotRect;
    Image image;
    [SerializeField] Slot slot;
    public Slot MySlot{
        get{ return slot;}
        set{
            if(value != null){
                slot = value;
                if(!slot.IsEmpty())
                    IncreaseAlpha();
                else
                    DecreaseAlpha();
            }
            else
                DecreaseAlpha();
        }
    }
    private void Awake() {
        slotRect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void Start() {
        if(slot.IsEmpty()){
            DecreaseAlpha();
        }
    }

    void DecreaseAlpha(){
        Color color = image.color;
        color.a = 0.6f;
        image.color = color;
    }
    void IncreaseAlpha(){
        Color color = image.color;
        color.a = 1f;
        image.color = color;
    }

    public void OnDrop(PointerEventData eventData)
    {
         if(eventData.pointerEnter != null){
            RectTransform icon = eventData.pointerDrag.GetComponent<RectTransform>();
            icon.transform.SetParent(transform);
            icon.anchoredPosition = slotRect.rect.center;
            Slot draggedSlot = eventData.pointerDrag.GetComponent<Slot>();
            //This old to dragged
            DropSlot draggedOldDrop = draggedSlot.GetComponent<DragDrop>().oldSlot;
            draggedOldDrop.MySlot = slot;
            slot.GetComponent<DragDrop>().oldSlot = draggedOldDrop;
            RectTransform thisRect = slot.GetComponent<RectTransform>();
            thisRect.transform.SetParent(draggedOldDrop.transform);
            thisRect.anchoredPosition = draggedOldDrop.slotRect.rect.center;
            //Dragged to this
            MySlot = draggedSlot;
            draggedSlot.GetComponent<DragDrop>().oldSlot = this;
         }
    }
}
