using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    [HideInInspector] public GameObject itemHolder;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] int startSize = 9;
    Animator playerAnimator;
    int currentSize;
    public int freeSlots;
    public int Size {
        get{return inventory.Count;}
    }
    public List<Slot> inventory = new List<Slot>();
    public Action inventoryUpdated;
    Item playerHandItem;
    [SerializeField] SpriteRenderer handSpriteHolder;
    int money = 100;
    public int Money{
        get{return money;}
        set{
            money = value;
            OnMoneyChanged?.Invoke(money);
        }
    }
    public Action<int> OnMoneyChanged;
    private void Awake() {
        instance = this;
        itemHolder = Resources.Load<GameObject>("Items/ItemHolder");
    }

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        if(PlayerPrefs.HasKey("Save")){
            LoadInventoryData();
        }
        Debug.Log("Startsize " + startSize);
        for (int i = 0; i < startSize; i++)
        {
            AddSlot();
        }

        if(PlayerPrefs.HasKey("Save")){
            PlayerSaves saves = SaveSystem.instance.saves.playerSaves;
            foreach (var slot in saves.inventory)
            {
                if(slot.itemName == "")
                    continue;
                string path = Path.Combine("Items/", slot.itemName);
                Debug.Log(path);
                Item it = Resources.Load<Item>(path);
                Item myItem = Instantiate(it);
                myItem.Count = slot.count;
                AddItem(myItem);
            }
        }
        PlayerUI.instance.attackButton.onClick.AddListener(UseHandItem);
    }

    void LoadInventoryData(){
        PlayerSaves saves = SaveSystem.instance.saves.playerSaves;
        startSize = saves.inventorySize;
    }
    public void SaveInventory(){
        PlayerSaves saves = SaveSystem.instance.saves.playerSaves;
        Debug.Log("save current size "+ currentSize);
        saves.inventorySize = currentSize;
        saves.inventory = inventory.Select(x => x.slotDataSave).ToList();
        ClearInventory();
    }

    //On click "Use" button
    public void UseHandItem(){
        if(playerHandItem != null){
            playerHandItem.UseItem(transform);
            Debug.Log("Invenotory -> UseHandItem()");
        }
    }
    public void NullItemRotation(){
        handSpriteHolder.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
    public void SetItemTransform(){
        handSpriteHolder.transform.localPosition = playerHandItem.spawnOffset;
        handSpriteHolder.transform.localRotation = Quaternion.Euler(0, 0, playerHandItem.spawnRotation);
    }
    [SerializeField] Transform attackPoint;
    public void EquipItem(Item it){
        playerHandItem = it;
        handSpriteHolder.sprite = it.sprite;
        SetItemTransform();

        string itemType = it.GetItemType();
        Debug.Log(itemType);
        if(itemType == "Weapon" || itemType == "PickAxe"){
            Weapon weapon = (Weapon)it;
            WeaponHands weaponHands = weapon.weaponHands;
            attackPoint.localPosition = weapon.attackPointOffset;
            if(weaponHands == WeaponHands.OneHand){
                playerAnimator.SetLayerWeight(1, 1f);
                playerAnimator.SetLayerWeight(2, 0f);
            }
            if(weaponHands == WeaponHands.TwoHand){
                playerAnimator.SetLayerWeight(1, 0f);
                playerAnimator.SetLayerWeight(2, 1f);
            }
        }
    }
    void UnEquipItem(){
        handSpriteHolder.sprite = null;
        playerHandItem = null;
        playerAnimator.SetLayerWeight(1, 0f);
        playerAnimator.SetLayerWeight(2, 0f);
    }
    //item not equiped
    public bool HandsEmpty(){
        return playerHandItem == null;
    }
    public void CheckHandsByItem(Item item){
        if(HandsEmpty())
            return;
        else{
            if(playerHandItem.name == item.name){
                UnEquipItem();
            }
        }
    }
    
    public bool itHasFreeSpace(){
        return freeSlots > 0 ? true : false;
    }
    //Add slot cell to inventory
    void AddSlot(){
        GameObject _slotPrefab = Instantiate(slotPrefab);
        _slotPrefab.transform.SetParent(PlayerUI.instance.inventorySlotHolder, false);
        DropSlot dSlot = _slotPrefab.GetComponent<DropSlot>();
        Slot slot = _slotPrefab.transform.GetChild(0).GetComponent<Slot>();
        dSlot.MySlot = slot;
        inventory.Add(slot);
        slot.inventoryIndex = currentSize;
        freeSlots++;
        currentSize++;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
            AddSlot();
    }
    //Add item to inventory
    public void AddItem(Item item){
        if(item == null)
            return;

        foreach (var slot in inventory)
        {
            if(item.isStackable){
                Item stackItem = slot.TryGetStackItem();
                if(stackItem != null){
                    if(item.name == stackItem.name){
                        if(slot.itemStack.Count == stackItem.maxStack)
                            continue;
                        if(slot.itemStack.Count < stackItem.maxStack)
                            slot.AddItem(item);
                        return;
                    }
                }
            } else break;
        }
        foreach (var slot in inventory)
        {
            if(slot.IsEmpty()){
                Debug.Log("Add when empty");
                slot.AddItem(item);
                freeSlots--;
                break;
            } 
        }
    }

    public void RemoveItem(Item item, int count = 0){
        if(item == null)
            return;
        foreach (var slot in inventory)
        {
            if(item.isStackable){
                Item stackItem = slot.TryGetStackItem();
                if(stackItem != null){
                    if(item.name == stackItem.name){
                        int countInStack = slot.itemStack.Count;
                        int result = count - countInStack;
                        if(result <= 0){
                            slot.Remove(count);
                            return;
                        }
                        else{
                            count -= countInStack;
                            continue;
                        }
                    }
                }
            } else break;
        }
        foreach (var slot in inventory)
        {
            if(!slot.IsEmpty()){
                Item stackItem = slot.TryGetStackItem();
                if(stackItem != null){
                    if(item.name == stackItem.name){
                        slot.Remove(1);
                        return;
                    }
                }
            } 
        }
    }

    public void ClearInventory(){
        foreach (var slot in inventory)
        {
            slot.ClearSlot();
        }
        Transform inventoryPanel = PlayerUI.instance.inventorySlotHolder;
        for (int i = 0; i < inventoryPanel.childCount; i++)
        {
            Transform child = inventoryPanel.GetChild(i);
            Destroy(child.gameObject);
        }
    }
}
