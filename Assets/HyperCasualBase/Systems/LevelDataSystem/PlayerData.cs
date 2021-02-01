using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;



[System.Serializable]
public class PlayerData 
{
    public PlayerData()
    {
        UnlockedSkins = new Dictionary<string, SkinType>();
        CurrencyData = new Dictionary<ExchangeType, int>();
        RateUsData = new RateUsData();
        CurrentSkin = "None";
    }

    [BoxGroup("Skin Data")]
    [ValueDropdown("GetSkins")]
    public string CurrentSkin;
    [BoxGroup("Skin Data")]
    [ValueDropdown("GetSkins")]
    public string CurrentLoadingSkin;
    [BoxGroup("Skin Data")]
    public int CurrentLoadingSkinTier;
    [BoxGroup("Skin Data")]
    [ShowInInspector]
    public Dictionary<string, SkinType> UnlockedSkins = new Dictionary<string, SkinType>();

    [BoxGroup("Currency Data")]
    [ShowInInspector]
    public Dictionary<ExchangeType, int> CurrencyData = new Dictionary<ExchangeType, int>();

    public RateUsData RateUsData;


#if UNITY_EDITOR
    private static List<string> GetSkins()
    {
        string[] guid = UnityEditor.AssetDatabase.FindAssets("t:SkinData");
        SkinData skinData = UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(guid[0]), (typeof(SkinData))) as SkinData;
        List<string> keys = new List<string>();
        foreach (var item in skinData.Skins)
        {
            if (!keys.Contains(item.SkinID))
                keys.Add(item.SkinID);
        }
        return keys;
    }
#endif
}
