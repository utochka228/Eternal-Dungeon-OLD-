using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RelocPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelNumber;
    [SerializeField] RectTransform playerText;
    [SerializeField] RectTransform circleButton;
    public RectTransform upLine;
    public RectTransform downLine;
    [SerializeField] RelocationButton button;
    public void SetData(int level){
        if(level % 2 == 0){
            circleButton.anchorMax = new Vector2(1, 0.5f);
            circleButton.anchorMin = new Vector2(1, 0.5f);
            circleButton.anchoredPosition = new Vector2(-circleButton.anchoredPosition.x, circleButton.anchoredPosition.y);
            upLine.localScale = new Vector3(-1, 1, 1);
            downLine.localScale = new Vector3(-1, -1, 1);
            playerText.anchoredPosition = new Vector2(-playerText.anchoredPosition.x, playerText.anchoredPosition.y);
        }
        levelNumber.text = level.ToString();
        button.indexPoint = level;
        if(level > 1 && level < Relocation.dungeonSize){
            Debug.Log("!!!!!");
            upLine.gameObject.SetActive(true);
            int myIndex = transform.GetSiblingIndex();
            Transform neighbour = transform.parent.GetChild(myIndex - 1);
            RelocPanel neighbourReloc = neighbour.GetComponent<RelocPanel>();
            neighbourReloc.downLine.gameObject.SetActive(true);
        }
    }
    public void MarkPlayer(){
        playerText.gameObject.SetActive(true);
        button.myButton.interactable = false;
    }
    public void UnMarkPlayer(){
        playerText.gameObject.SetActive(false);
        button.myButton.interactable = true;
    }
}
