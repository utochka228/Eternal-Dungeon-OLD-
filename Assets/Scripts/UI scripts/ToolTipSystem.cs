using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class ToolTipSystem : MonoBehaviour
{
    public static ToolTipSystem i;
    public ToolTipItem toolTip;
    UIFader fader;
    private void Awake() {
        i = this;
    }
    private void Start() {
        fader = toolTip.GetComponent<UIFader>();
    }
    static readonly Vector2 offset = new Vector2(Screen.width/2f, Screen.height/2f);
    public static void Show(Item item, Transform uiPosition){
        Vector2 pos = RectTransformUtility.WorldToScreenPoint(null, uiPosition.position);
        string content = "";
        Type itemType = item.GetType();
        var fields = itemType.GetFields().Where(x => x.GetCustomAttribute(typeof(ToolTipInfoAttribute)) != null);
        foreach (var field in fields)
        {
            ToolTipInfoAttribute myAttribute = (ToolTipInfoAttribute)field.GetCustomAttribute(typeof(ToolTipInfoAttribute));
            content += myAttribute.GetDescText + ":" + field.GetValue(item) + "\n";
        }   
        i.toolTip.SetText(content, item.itemName);
        i.toolTip.GetComponent<RectTransform>().anchoredPosition = pos - offset;
        i.toolTip.actionButtHolder.gameObject.SetActive(true);
        i.toolTip.gameObject.SetActive(true);
        i.fader.Fade();
    }
    public static void SetItemActions(Slot invSlot, Item it){
        i.toolTip.ClearActions();

        Type slotType = invSlot.GetType();
        var methods = slotType.GetMethods().Where(x => x.GetCustomAttribute(typeof(ItemActionAttribute)) != null);
        foreach (var action in methods)
        {
            bool createButton = false;
            ItemActionAttribute myAttribute = (ItemActionAttribute)action.GetCustomAttribute(typeof(ItemActionAttribute));
            if(myAttribute.IsMustUsed == false){
                for (int i = 0; i < it.actions.Length; i++)
                {
                    if(it.actions[i].itemAcitons == myAttribute.GetItemActionEnum){
                        createButton = true;
                        break;
                    }
                }
            }else createButton = true;
            
            if(createButton == true)
                i.toolTip.CreateActionButton(()=>{action.Invoke(invSlot, new object[]{});}, myAttribute.GetActionName);
        }
    }
    [HideInInspector]
    public static void Hide(){
        i.toolTip.ClearActions();
        i.toolTip.actionButtHolder.gameObject.SetActive(false);
        i.toolTip.gameObject.SetActive(false);
    }
}
