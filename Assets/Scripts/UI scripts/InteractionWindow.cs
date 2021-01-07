using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionWindow : MonoBehaviour
{
    public static InteractionWindow instance;

    [SerializeField] RectTransform windowRect;
    [SerializeField] Transform holder;
    [SerializeField] Transform actions;
    [SerializeField] GameObject interactButtonPrefab;
    private void Awake() {
        instance = this;
    }
    public void ShowWindow(Slot slot){
        ClearWindow();

        foreach (var action in slot.itemActions)
        {
            string buttonName = action.Key;
            GameObject button = Instantiate(interactButtonPrefab, actions);
            button.transform.name = buttonName + "Button";
            Button interactButton = button.GetComponent<Button>();
            interactButton.onClick.AddListener(action.Value);
            interactButton.onClick.AddListener(HideWindow);
            TextMeshProUGUI title = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            title.text = buttonName;
        }

        holder.gameObject.SetActive(true);
    }


    public void HideWindow(){
        ClearWindow();
        holder.gameObject.SetActive(false);
    }

    void ClearWindow(){
        for (int i = 0; i < actions.childCount; i++)
        {
            Destroy(actions.GetChild(i).gameObject);
        }
    }
}
