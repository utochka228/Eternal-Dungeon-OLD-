using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour, IInteractable
{
    GameObject shopPanel;
    void Start()
    {
        shopPanel = PlayerUI.instance.shopPanel;
    }

    void Update()
    {
        
    }

    public void Interact()
    {
        shopPanel.SetActive(true);
        Shop shop = shopPanel.GetComponent<Shop>();
        shop.UpdatePlayerInventory();
    }


}
