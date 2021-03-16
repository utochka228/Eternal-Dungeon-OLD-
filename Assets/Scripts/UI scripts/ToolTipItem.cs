using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ToolTipItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI contentField;
    [SerializeField] TextMeshProUGUI headerField;
    [SerializeField] GameObject actionButton;
    public Transform actionButtHolder;
    [SerializeField] LayoutElement layoutElement;
    [SerializeField] int characterWrapLimit;
    public void SetText(string content, string header = ""){
        if(string.IsNullOrEmpty(header)){
            headerField.gameObject.SetActive(false);
        }else{
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }
        contentField.text = content;
        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength >characterWrapLimit)? true : false;
    }
    public void CreateActionButton(UnityAction action, string buttonName){
        GameObject button = Instantiate(actionButton, actionButtHolder);
        Button interactButton = button.GetComponent<Button>();
        interactButton.onClick.AddListener(action);
        interactButton.onClick.AddListener(ToolTipSystem.Hide);
        TextMeshProUGUI title = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        title.text = buttonName;
    }
    public void ClearActions(){
        for (int i = 0; i < actionButtHolder.childCount; i++)
        {
            Destroy(actionButtHolder.GetChild(i).gameObject);
        }
    }
    //For button component
    public void Hide(){
        ClearActions();
        actionButtHolder.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
