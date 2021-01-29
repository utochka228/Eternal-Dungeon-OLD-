using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI stack;
    [SerializeField] TextMeshProUGUI price;
    [SerializeField] TextMeshProUGUI selectedCount;
    [SerializeField] GameObject border;
    int countInShop;
    public int CountInShop{
        get { return countInShop;}
        set{
            countInShop = value;
            stack.text = countInShop.ToString();
        }
    }
    int itemPrice;
    public int ItemPrice{
        get{ return itemPrice;}
        set{
            itemPrice = value;
            price.text = itemPrice.ToString();
        }
    }
    int countSelectedItems;
    public int CountSelectedItems{
        get { return countSelectedItems;}
        set{
            countSelectedItems = value;
            selectedCount.text = countSelectedItems.ToString();
            if(countSelectedItems > 0)
                border.SetActive(true);
            else
                border.SetActive(false);
        }
    }
    Item myItem;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetItem(Item it, int count){
        myItem = it;
        icon.sprite = it.sprite;
        CountInShop = count;
        ItemPrice = Random.Range(it.itemMinPrice, it.itemMinPrice + 20);
    }
    public void OnClicked(){
        if(myItem.isStackable){
            //Call slider window
        }else{
            if(countSelectedItems == 1)
                countSelectedItems = 0;
            else
                CountSelectedItems = 1;
        }
    }
}
