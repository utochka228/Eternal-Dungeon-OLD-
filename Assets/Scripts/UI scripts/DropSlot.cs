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
            Slot draggedSlot = eventData.pointerDrag.GetComponent<Slot>();
            DragDrop dragDrop = draggedSlot.GetComponent<DragDrop>();
            //Check if can union into One stack
                Item thisItem = MySlot.TryGetStackItem();
                Item draggedItem = draggedSlot.TryGetStackItem();
                if(thisItem != null && draggedItem != null){
                    if(thisItem.name == draggedItem.name){
                        int countInStack = MySlot.itemStack.Count;
                        int freeSpace = thisItem.maxStack - countInStack;
                        if(freeSpace > 0){
                            draggedItem.Count = freeSpace;
                            MySlot.AddItem(draggedItem);
                            draggedSlot.Remove(freeSpace);
                            dragDrop.ReturnToPrevPosition();
                            return;
                        }
                    }
                }
            icon.transform.SetParent(transform);
            icon.anchoredPosition = slotRect.rect.center;
            int firstIndex = draggedSlot.inventoryIndex;
            int secondIndex = MySlot.inventoryIndex;
            //This old to dragged
            DropSlot draggedOldDrop = dragDrop.oldSlot;
            draggedOldDrop.MySlot = slot;
            slot.GetComponent<DragDrop>().oldSlot = draggedOldDrop;
            RectTransform thisRect = slot.GetComponent<RectTransform>();
            thisRect.transform.SetParent(draggedOldDrop.transform);
            thisRect.anchoredPosition = draggedOldDrop.slotRect.rect.center;
            //Dragged to this
            MySlot = draggedSlot;
            draggedSlot.GetComponent<DragDrop>().oldSlot = this;
            SwapTwoInvElements(firstIndex, secondIndex);
         }
    }
    void SwapTwoInvElements(int first, int second){
        var inventory = Inventory.instance.inventory;
        Slot firstSlot = inventory[first];
        firstSlot.inventoryIndex = second;
        Slot secondSlot = inventory[second];
        secondSlot.inventoryIndex = first;
        inventory[first] = secondSlot;
        inventory[second] = firstSlot;
    }
}
