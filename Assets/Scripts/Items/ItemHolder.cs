using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour, IInteractable
{
    [SerializeField] Item testItem; //Delete this
    Item myItem;

    [SerializeField] SpriteRenderer sprite;

    void Start()
    {
        if(testItem != null)
            SetItem(testItem, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItem(Item newItem, bool isGameWorldItem){
        if(isGameWorldItem){
            myItem = newItem;
        }else{
            myItem = Instantiate(newItem);
        }
        sprite.sprite = myItem.sprite;
        transform.localScale = new Vector3(myItem.itemWorldScale, myItem.itemWorldScale, 1f);
    }

    public void Interact()
    {
        PickUp();
    }

    void PickUp(){
        if(myItem.name.Replace("(Clone)", "") == "DeathPlayerLoot"){
            DeathPlayerLoot playerLoot = (DeathPlayerLoot) myItem;
            foreach (var item in playerLoot.loot)
            {
                Inventory.instance.AddItem(item);
            }
            PlayerSaves playerSaves = SaveSystem.instance.saves.playerSaves;
            playerSaves.deathPointData.createDeathPoint = false;
            Debug.Log("&&&&&&");
            Destroy(gameObject);
            return;
        }

        Debug.Log("Item was picked!");
        Inventory.instance.AddItem(myItem);
        Destroy(gameObject);
    }
}
