using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public GameObject itemHolder;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] int startSize = 9;
    public int freeSlots;
    public int Size {
        get{
            return inventory.Count;
        }
    }
    List<Slot> inventory = new List<Slot>();
    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < startSize; i++)
        {
            AddSlot();
        }
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
}
