using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.All)]
public class ToolTipInfoAttribute : Attribute{
    string descriptionText;
    public ToolTipInfoAttribute(string descriptionText)
    {
        this.descriptionText = descriptionText;
    }
    public string GetDescText { get{return descriptionText;}}
}
[AttributeUsage(AttributeTargets.All)]
public class ItemActionAttribute : Attribute{
    bool mustUsed = false;
    ItemActionsEnum itemActionEnum;
    string actionName = "";
    public ItemActionAttribute(bool mustUsed, string actionName)
    {
        //if mustUsed = true, then method marked this attribute with this flag
        //100% will apear in item actions
        //if mustUsed = false - nothing
        this.mustUsed = mustUsed;
        this.actionName = actionName;
    }
    public ItemActionAttribute(ItemActionsEnum itemActionEnum, string actionName)
    {
        this.itemActionEnum = itemActionEnum;
        this.actionName = actionName;
    }
    public string GetActionName{get{return actionName;}}
    public bool IsMustUsed{get{return mustUsed;}}
    public ItemActionsEnum GetItemActionEnum{get{return itemActionEnum;}}
}
