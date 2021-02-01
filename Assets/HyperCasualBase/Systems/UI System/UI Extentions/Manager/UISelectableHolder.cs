using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvanceUI;

public static class UISelectableHolder
{
    public static List<AdvanceSelectable> nSelectables = new List<AdvanceSelectable>();


    public static void AddToHolder(AdvanceSelectable nSelectable)
    {
        if (!nSelectables.Contains(nSelectable))
            nSelectables.Add(nSelectable);
    }

    public static void RemoveFromHolder(AdvanceSelectable nSelectable)
    {
        if(nSelectables.Contains(nSelectable))
            nSelectables.Remove(nSelectable);
    }

    //public static AdvanceSelectable GetSelectable(string id)
    //{
    //    for (int i = 0; i < nSelectables.Count; i++)
    //    {
    //        if(string.Equals(nSelectables[i].ID, id))
    //        {
    //            return nSelectables[i];
    //        }
    //    }

    //    Debug.LogError("UI component with the ID of " + id + "Not Found");
    //    return null;
    //}
}
