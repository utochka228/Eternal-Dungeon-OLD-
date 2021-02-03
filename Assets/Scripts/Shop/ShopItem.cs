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
    [SerializeField] GameObject selectionBackground;
    [SerializeField] GameObject border;
    [HideInInspector] public Shop shop;
    public bool thisMerchantItem = true;
    int countInStack;
    public int CountInStack{
        get { return countInStack;}
        set{
            countInStack = value;
            stack.text = countInStack.ToString();
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
            int prevSelectedCount = countSelectedItems;
            Debug.Log("Prev sel count:" + prevSelectedCount);
            countSelectedItems = value;
            Debug.Log("Count sel it: " + countSelectedItems);
            Debug.Log($"{countSelectedItems} - {prevSelectedCount} * {ItemPrice}");
            int totalSum = (countSelectedItems-prevSelectedCount) * ItemPrice;
            Debug.Log("TOTAL SUM:" + totalSum);
            if(thisMerchantItem)
                shop.TotalBuySum += totalSum;
            else
                shop.TotalSellSum += totalSum;
            if(countSelectedItems == 0){
                shop.ProcessProductToShopList(this, ShopListAction.Remove);
                selectionBackground.SetActive(false);
            }
            if(countSelectedItems > 0){
                shop.ProcessProductToShopList(this, ShopListAction.Add);  
                selectionBackground.SetActive(true);
                border.SetActive(true);
            }
            else{
                border.SetActive(false);
            }
            selectedCount.text = countSelectedItems.ToString();
        }
    }
    public Item myItem;
    public void SetItem(Item it, int count){
        myItem = it;
        icon.sprite = it.sprite;
        CountInStack = count;
        if(thisMerchantItem)
            ItemPrice = Random.Range(it.itemMinPrice, it.itemMinPrice + 20);
        else
            ItemPrice = it.itemMinPrice;
    }
    public void OnClicked(){
        if(myItem.isStackable){
            InteractionSlider.instance.ShowSlider(SelectStack, countInStack);
        }else{
            if(CountSelectedItems == 1)
                CountSelectedItems = 0;
            else
                CountSelectedItems = 1;
        }
    }
    //Method for called slider
    void SelectStack(int selectedCount){
        CountSelectedItems = selectedCount;
    }

    public void OnBoughtItem(){
        int count = CountSelectedItems;
        CountInStack -= count;
        countSelectedItems = 0;
        border.SetActive(false);
        selectionBackground.SetActive(false);
        if(CountInStack == 0){
            Destroy(gameObject);
        }
    }
}
