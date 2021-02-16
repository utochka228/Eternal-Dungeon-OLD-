using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthHearts : MonoBehaviour
{
    [SerializeField] int heartValue;
    [SerializeField] Transform heartsHolder;
    [SerializeField] GameObject heartPrefab;
    [SerializeField] TextMeshProUGUI playerHealthText;

    List<Heart> hearts = new List<Heart>();
    int currentHeartCount;

    public void UpdateHearts(PlayerStats playerStats)
    {
        //Hearts count
        int maxHealth = playerStats.MaxHealth;
        int heartCount = maxHealth/heartValue;
        if(currentHeartCount != heartCount){
            if(heartCount > currentHeartCount){
                //Add
                int newCount = heartCount - currentHeartCount;
                SpawnHearts(newCount, playerStats);
            }
            if(heartCount < currentHeartCount){
                //Remove
                int removeCount = currentHeartCount - heartCount;
                RemoveHearts(removeCount);
            }
            
            currentHeartCount = heartCount;
        }
        //Health control heart fill sprite
        int playerHealth = playerStats.Health;
        playerHealthText.text = playerHealth + "/" + maxHealth;
        float filledHearts = (float)playerHealth / heartValue;
        Debug.Log(filledHearts + "filled hearts");
        if(filledHearts >= 0 && filledHearts < 1){
            hearts[0].SetFillLevel(filledHearts);
        }
        if(filledHearts >= 1){
            int intPart = (int)filledHearts;
            float decPart = filledHearts - (float)intPart;
            Debug.Log(intPart + "int part, " + decPart + "decpart");
            for (int i = 0; i < intPart; i++)
            {
                hearts[i].SetFillLevel(1f);
            }
            if(intPart < hearts.Count){
                int index = 0;
                index = intPart;
                hearts[index].SetFillLevel(decPart);
            }
        }
        if(filledHearts < (float)hearts.Count-1){
            for (int i = (int)filledHearts+1; i < hearts.Count; i++)
            {
                hearts[i].SetFillLevel(0f);
            }
        }
    }
    void SpawnHearts(int count, PlayerStats playerStats){

        for (int i = 0; i < count; i++)
        {
            GameObject heartObj = Instantiate(heartPrefab, heartsHolder);
            Heart heart = heartObj.GetComponent<Heart>();
            if(playerStats.Health != playerStats.MaxHealth)
                heart.SetFillLevel(0f);
            hearts.Add(heart);
        }
    }
    void RemoveHearts(int count){
        int heartsCount = hearts.Count;
        for (int i = heartsCount-1; i >= heartsCount-count; i--)
        {
            Heart heart = hearts[i];
            hearts.Remove(heart);
            Destroy(heart.gameObject);
        }
    }
}
