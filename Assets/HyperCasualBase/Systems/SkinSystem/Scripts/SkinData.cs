using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class SkinTier
{
    [ValueDropdown("TreeViewOfInts")]
    public int TierAmount = 25;


    private IEnumerable TreeViewOfInts = new ValueDropdownList<int>()
{
    { "10", 10 },
    { "20", 20 },
    { "25", 25 },
    { "50", 50 },
};
}

[System.Serializable]
public class Skin
{
    bool hasIncorrectValues
    {
        get
        {
            int value = 0;
            for (int i = 0; i < skinTiers.Count; i++)
            {
                value += skinTiers[i].TierAmount;
            }

            return (value != 100) ? true : false;
        }
    }

    bool iconIsNull { get { return SkinIcon == null; } }

    public string SkinID;
    public SkinType SkinType = SkinType.FullBody;
    public SkinCategory SkinCategory = SkinCategory.Sellable;
    [ShowIf("isExculisive")]
    [InfoBox("Values defined are not equal to 100", InfoMessageType.Error, VisibleIf = "hasIncorrectValues")]
    public List<SkinTier> skinTiers;
    [InfoBox("Skin Icon is Null", VisibleIf = "iconIsNull")]
    public Sprite SkinIcon;
    [HideIf("isExculisive")]
    public ExchangeType ExchangeType = ExchangeType.Coin;

    [Range(0, 5000)]
    [HideIf("isExculisive")]
    public int SkinCost;

    private bool isExculisive { get { return SkinCategory == SkinCategory.Exculisive; } }
}

public class SkinData : SerializedScriptableObject
{
    public List<Skin> Skins = new List<Skin>();
}
