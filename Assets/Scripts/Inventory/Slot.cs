using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
public class Slot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI stackCountText;
    Stack<Item> itemStack = new Stack<Item>();
    [SerializeField] Image image;
    public bool IsEmpty() {return itemStack.Count == 0 ? true: false;}
        
    public Dictionary<string, UnityAction> itemActions = new Dictionary<string, UnityAction>();
    // Start is called before the first frame update
    void Start()
    {
        stackCountText.gameObject.SetActive(false);
    }

    void ApplyItemActions(){
        ItemActions itActions = itemStack.Peek().actions;
        if(itActions.isEquipable)
            itemActions.Add("Equip", new UnityAction(EquipItem));
        if(itActions.isUsable)
            itemActions.Add("Use", new UnityAction(UseItem));

        itemActions.Add("Drop", new UnityAction(DropItem));
        itemActions.Add("Remove", new UnityAction(RemoveItem));
    }

    void ClearSlot(){
        image.sprite = null;
        itemActions.Clear();
    }

    public void AddItem(Item item){
        for (int i = 0; i < item.Count; i++)
        {
            itemStack.Push(item);
            if(itemStack.Count == 1){
                image.sprite = item.sprite;
                ApplyItemActions();
            }
        }
        stackCountText.text = itemStack.Count.ToString();
        if(itemStack.Count > 1)
            stackCountText.gameObject.SetActive(true);
    }
    
    //For remove button
    public void Remove(int count = 1){
        for (int i = 0; i < count; i++)
        {
            itemStack.Pop();
            if(itemStack.Count == 0){
                ClearSlot();
                Inventory.instance.freeSlots++;
            }
        }
        stackCountText.text = itemStack.Count.ToString();
        if(itemStack.Count <= 1)
            stackCountText.gameObject.SetActive(false);
    }

    public void DropItem(){
        InteractionSlider.instance.ShowSlider(Drop, itemStack.Count);
    }
    public void RemoveItem(){
        InteractionSlider.instance.ShowSlider(Remove, itemStack.Count);
    }

    //For drop button
    void Drop(int count){
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
            Debug.Log("InventorySlot has exception: " + ex.Message);
            return null;
        }
    }
}

