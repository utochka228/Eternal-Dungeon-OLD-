using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct SlotDataSave{
    public string itemName;
    public int count;

    public SlotDataSave(Item it, int _count){
        
        itemName = it.name.Replace("(Clone)", "");
        count = _count;
    }
}
public class Slot : MonoBehaviour
{
    public SlotDataSave slotDataSave;
    [SerializeField] TextMeshProUGUI stackCountText;
    public Stack<Item> itemStack = new Stack<Item>();
    [SerializeField] Image image;
    public bool IsEmpty() {return itemStack.Count == 0 ? true: false;}
    public int inventoryIndex;
        
    public Dictionary<string, UnityAction> itemActions = new Dictionary<string, UnityAction>();
    void Awake()
    {
        stackCountText.gameObject.SetActive(false);
        image.enabled = false;
    }
    //Apply manipulation item actions to this slot
    void ApplyItemActions(){
        ItemActions itActions = itemStack.Peek().actions;
        if(itActions.isEquipable)
            itemActions.Add("Equip", new UnityAction(EquipItem));
        if(itActions.isUsable)
            itemActions.Add("Use", new UnityAction(UseItem));

        itemActions.Add("Drop", new UnityAction(DropItem));
        itemActions.Add("Remove", new UnityAction(RemoveItem));
    }

    public void ClearSlot(){
        if(itemActions.Count == 0)
            return;
        Debug.Log("Slot cleared");
        image.sprite = null;
        image.enabled = false;
        DropSlot dropSlot = GetComponent<DragDrop>().oldSlot;
        dropSlot.MySlot = null;
        itemActions.Clear();
        Inventory.instance.freeSlots++;
        slotDataSave.itemName = "";
        slotDataSave.count = 0;
    }

    public void AddItem(Item item){
        for (int i = 0; i < item.Count; i++)
        {
            if(item.isStackable){
           //not enough of space in stack
                if(itemStack.Count == item.maxStack){
                    int AddedCount = i;
                    item.Count -= AddedCount;
                    Inventory.instance.AddItem(item);
                    item.Count += AddedCount;
                    break;
                }
            }
            itemStack.Push(item);
            Debug.Log("ITEM ADDED");
            //First adding
            if(itemStack.Count == 1){
                Debug.Log(item.itemName +" name");
                image.sprite = item.sprite;
                image.enabled = true;
                ApplyItemActions();
            }
        }
        stackCountText.text = itemStack.Count.ToString();
        if(itemStack.Count > 1){
            stackCountText.gameObject.SetActive(true);
        }
        DropSlot dropSlot = GetComponent<DragDrop>().oldSlot;
        dropSlot.MySlot = this;

        slotDataSave = new SlotDataSave(item, itemStack.Count);
        Inventory.instance.inventoryUpdated?.Invoke();
    }
    //Main Remove item method 
    public void Remove(int count = 1){
        if(count == 0)
            return;
        for (int i = 0; i < count; i++)
        {
            Item item = itemStack.Pop();
            if(itemStack.Count == 0){
                ClearSlot();
                Inventory.instance.CheckHandsByItem(item);
                break;
            }
        }
        stackCountText.text = itemStack.Count.ToString();
        if(itemStack.Count <= 1)
            stackCountText.gameObject.SetActive(false);
        Inventory.instance.inventoryUpdated?.Invoke();
    }
    //Drop button
    public void DropItem(){
        if(TryGetStackItem().isStackable){
            string dropHeader = "Drop count";
            InteractionSlider.instance.ShowSlider(Drop, itemStack.Count, dropHeader);
        }else
            Drop(1);
    }
    //Remove button
    public void RemoveItem(){
        if(TryGetStackItem().isStackable){
            string removeHeader = "Remove count";
            InteractionSlider.instance.ShowSlider(Remove, itemStack.Count, removeHeader);
        }else
            Remove(1);
    }

    //Main drop item method
    void Drop(int count){
        if(count == 0)
            return;
        GameObject itemHolder = Instantiate(Inventory.instance.itemHolder);
        ItemHolder holder = itemHolder.GetComponent<ItemHolder>();
        Item item = itemStack.Peek();
        item.Count = count;
        holder.SetItem(item, true);
        Transform player = GameSession.instance.Player.transform;
        itemHolder.transform.position = player.transform.position;
        Remove(count);
    }

    public void OnSlotPressed(){

        if(itemStack.Count == 0)
            return;

        //Activate tooltip
        InteractionWindow.instance.ShowWindow(this);
    }

    //For use button
    public void UseItem(){
        Debug.Log("Item used!");
        Item item = itemStack.Peek();
        Transform player = GameSession.instance.Player.transform;
        item.UseItem(player);

        Remove();
    }

    //For equip button
    public void EquipItem(){
        Debug.Log("Item equiped!");
        //Spawn item near of player
        Item item = itemStack.Peek();
        Inventory.instance.EquipItem(item);
        //Select slot in the inventory
    }

    public Item TryGetStackItem(){
        try{
            Item item = itemStack.Peek();
            return item;
        }
        catch(Exception ex){
            return null;
        }
    }
}

