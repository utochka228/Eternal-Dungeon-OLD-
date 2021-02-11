using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
public class Shop : MonoBehaviour
{
    Dictionary<string, ShopProduct> shopItems = new Dictionary<string, ShopProduct>();
    [SerializeField] GameObject shopItemPref;
    [SerializeField] Transform shopHolder;
    [SerializeField] Transform playerInventoryHolder;
    [SerializeField] GameObject buyButton;
    [SerializeField] TextMeshProUGUI buyPrice;
    [SerializeField] GameObject sellButton;
    [SerializeField] TextMeshProUGUI sellPrice;
    [SerializeField] TextMeshProUGUI money;

    List<ShopItem> selectedShopProducts = new List<ShopItem>();
    int totalBuySum;
    public int TotalBuySum{
        get{return totalBuySum;}
        set{
            totalBuySum = value;
            buyPrice.text = totalBuySum.ToString();
        }
    }
    List<ShopItem> selectedInventoryProducts = new List<ShopItem>();
    int totalSellSum;
    public int TotalSellSum{
        get{return totalSellSum;}
        set{
            totalSellSum = value;
            sellPrice.text = totalSellSum.ToString();
        }
    }
    //Call when product was selected
    public void ProcessProductToShopList(ShopItem shopItem, ShopListAction shopAction){
        if(shopAction == ShopListAction.Add){
            if(shopItem.thisMerchantItem){
                if(selectedShopProducts.Contains(shopItem))
                    return;
                selectedShopProducts.Add(shopItem);
                buyButton.SetActive(true);
            }
            else{
                if(selectedInventoryProducts.Contains(shopItem))
                    return;
                selectedInventoryProducts.Add(shopItem);
                sellButton.SetActive(true);
            }
        }
        if(shopAction == ShopListAction.Remove){
            if(shopItem.thisMerchantItem){
                selectedShopProducts.Remove(shopItem);
                if(selectedShopProducts.Count == 0)
                    buyButton.SetActive(false);
            }else{
                selectedInventoryProducts.Remove(shopItem);
                if(selectedInventoryProducts.Count == 0)
                    sellButton.SetActive(false);
            }
        }
    }
    void Start()
    {   
        Inventory inventory = Inventory.instance;
        inventory.inventoryUpdated += UpdatePlayerInventory;
        inventory.OnMoneyChanged += UpdateShopMoney;
        UpdateShopMoney(inventory.Money);
        ProcessShop();
    }
    void UpdateShopMoney(int newMoneyValue){
        money.text = newMoneyValue.ToString();
    }
    //Called once for merchant`s items
    void ProcessShop(){
        Item[] items = Resources.LoadAll<Item>("Items/");
        foreach (var item in items)
        {
            string typeName = item.GetType().ToString();
            int count = 1;
            if(item.isStackable){
                count = Random.Range(1, 10);
            }else {
                count = 1;
            }
            shopItems.Add(typeName, new ShopProduct(item, count));
        }
        FillShop();
    }
    //Called when inventory changes 
    public void UpdatePlayerInventory(){
        ClearPlayerShopInventory();
        FillShopInventory();
    }
    void FillShopInventory(){
        var playerInventory = Inventory.instance.inventory;
        foreach (var slot in playerInventory)
        {
            if(slot.IsEmpty())
                continue;
            GameObject itemPref = Instantiate(shopItemPref, playerInventoryHolder);
            ShopItem shopIt = itemPref.GetComponent<ShopItem>();
            shopIt.shop = this;
            shopIt.thisMerchantItem = false;
            shopIt.SetItem(slot.TryGetStackItem(), slot.itemStack.Count, slot);
        }
    }
    void ClearPlayerShopInventory(){
        for (int i = 0; i < playerInventoryHolder.childCount; i++)
        {
            Destroy(playerInventoryHolder.GetChild(i).gameObject);
        }
    }
    void FillShop(){
        for (int i = 0; i < shopHolder.childCount; i++)
        {
            Transform category = shopHolder.GetChild(i);
            string categoryName = category.name;
            var items = shopItems.Where(x => x.Key == categoryName).Select(x => x.Value);
            foreach (var itemProduct in items)
            {
                GameObject itemPref = Instantiate(shopItemPref, category);
                ShopItem shopIt = itemPref.GetComponent<ShopItem>();
                shopIt.shop = this;
                shopIt.SetItem(itemProduct.item, itemProduct.count);
            }
        }
    }
    //Switching shop categories 
    public void ChangeCategory(string category){
        for (int i = 0; i < shopHolder.childCount; i++)
        {
            Transform categoryPanel = shopHolder.GetChild(i);
            string categoryName = categoryPanel.name;
            if(categoryName == category){
                categoryPanel.gameObject.SetActive(true);
            }else{
                categoryPanel.gameObject.SetActive(false);
            }
        }
    }
    public void Buy(){
        int playerMoney = Inventory.instance.Money;
        if(playerMoney < TotalBuySum){
            Debug.Log("Not enough of money");
            return;
        }
        else{
            foreach (var item in selectedShopProducts)
            {
                Item it = Instantiate(item.myItem);
                it.Count = item.CountSelectedItems;
                Inventory.instance.AddItem(it);
                item.OnBoughtItem();
            }
            buyButton.SetActive(false);
            selectedShopProducts.Clear();
            Inventory.instance.Money -= TotalBuySum;
            totalBuySum = 0;
        }
    }
    public void Sell(){
        foreach (var item in selectedInventoryProducts)
        {
            int count = item.CountSelectedItems;
            item.myInvSlot.Remove(count);
        }
        sellButton.SetActive(false);
        selectedInventoryProducts.Clear();
        Inventory.instance.Money += TotalSellSum;
        TotalSellSum = 0;
    }
}
struct ShopProduct{
    public Item item;
    public int count;

    public ShopProduct(Item it, int _count){
        item = it;
        count = _count;
    }
}
public enum ShopListAction { Add, Remove}
