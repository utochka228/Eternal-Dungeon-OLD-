using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

struct ShopProduct{
    public Item item;
    public int count;

    public ShopProduct(Item it, int _count){
        item = it;
        count = _count;
    }
}
public class Shop : MonoBehaviour
{
    Dictionary<string, ShopProduct> shopItems = new Dictionary<string, ShopProduct>();
    [SerializeField] GameObject shopItemPref;
    [SerializeField] Transform shopHolder;
    void Start()
    {
        ProcessShop();
        ChangeCategory("Food");
    }

    void Update()
    {
        
    }

    void ProcessShop(){
        Item[] items = Resources.LoadAll<Item>("Items/");
        foreach (var item in items)
        {
            string typeName = item.GetType().ToString();
            int count = 1;
            if(item.isStackable){
                count = Random.Range(0, 10);
            }else {
                count = 1;
            }
            shopItems.Add(typeName, new ShopProduct(item, count));
        }
    }

    public void UpdatePlayerInventory(){
        
    }

    public void ChangeCategory(string category){
        ClearShopItems();
        var items = shopItems.Where(x => x.Key == category).Select(x => x.Value);
        foreach (var itemProduct in items)
        {
            GameObject itemPref = Instantiate(shopItemPref, shopHolder);
            ShopItem shopIt = itemPref.GetComponent<ShopItem>();
            shopIt.SetItem(itemProduct.item, itemProduct.count);
        }
    }

    void ClearShopItems(){
        for (int i = 0; i < shopHolder.childCount; i++)
        {
            Destroy(shopHolder.GetChild(i).gameObject);
        }
    }

    public void Buy(){

    }
    public void Sell(){

    }
}
