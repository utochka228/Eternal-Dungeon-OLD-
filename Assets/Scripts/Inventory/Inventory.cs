using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public GameObject itemHolder;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] int startSize = 9;
    int currentSize;
    public int freeSlots;
    public int Size {
        get{
            return inventory.Count;
        }
    }
    public List<Slot> inventory = new List<Slot>();

    Item playerHandItem;
    [SerializeField] SpriteRenderer handSpriteHolder;
    [SerializeField] SpriteRenderer playerSkin;
    private void Awake() {
        instance = this;
    }

    void Start()
    {
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

    //On click button
    public void UseHandItem(){
        if(playerHandItem != null){
            playerHandItem.UseItem(transform);
            Debug.Log("Invenotory -> UseHandItem()");
        }
    }

    public void FlipXHandItem(){
        Transform handItem = handSpriteHolder.transform;
        handItem.localPosition = new Vector3(-handItem.localPosition.x, handItem.localPosition.y, handItem.localPosition.z);
        handItem.localEulerAngles = new Vector3(handItem.localEulerAngles.x, handItem.localEulerAngles.y, -handItem.localEulerAngles.z);
    }

    public void EquipItem(Item it){
        playerHandItem = it;
        handSpriteHolder.sprite = it.sprite;
        handSpriteHolder.transform.localPosition = it.spawnOffset;
        handSpriteHolder.transform.localRotation = Quaternion.Euler(0, 0, it.spawnRotation);
        if(playerSkin.flipX)
            FlipXHandItem();
    }

    public bool HandsEmpty(){
        return playerHandItem == null;
    }

    public bool itHasFreeSpace(){
        return freeSlots > 0 ? true : false;
    }

    void AddSlot(){
        GameObject _slotPrefab = Instantiate(slotPrefab);
        _slotPrefab.transform.SetParent(PlayerUI.instance.inventoryPanel, false);
        Slot slot = _slotPrefab.GetComponent<Slot>();
        inventory.Add(slot);
        freeSlots++;
        currentSize++;
        Debug.Log("%%%%");

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
            AddSlot();
    }

    public void AddItem(Item item){
        if(item == null)
            return;

        foreach (var slot in inventory)
        {
            if(slot.IsEmpty()){
                slot.AddItem(item);
                freeSlots--;
                break;
            } else if(item.isStackable){
                Item stackItem = slot.TryGetStackItem();
                if(item != null){
                    if(item.name == stackItem.name){
                        slot.AddItem(item);
                        break;
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
        Transform inventoryPanel = PlayerUI.instance.inventoryPanel;
        for (int i = 0; i < inventoryPanel.childCount; i++)
        {
            Transform child = inventoryPanel.GetChild(i);
            Destroy(child.gameObject);
        }
    }
}
