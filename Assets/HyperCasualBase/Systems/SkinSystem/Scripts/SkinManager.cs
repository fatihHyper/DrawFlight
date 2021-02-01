using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public enum SkinType
{
    Hat,
    Face,
    Pants,
    FullBody
}

public enum SkinCategory
{
    Sellable,
    Exculisive,
}

public class SkinManager : Singleton<SkinManager>
{
    public SkinData SkinData;

    [ShowInInspector]
    [ReadOnly]
    private Dictionary<string, SkinType> SkinArchive {
        get
        {
            PlayerData playerData = SaveLoadManager.LoadPDP<PlayerData>(SavedFileNameHolder.PlayerData, new PlayerData());
            playerData.UnlockedSkins["None"] = SkinType.FullBody;
            SaveLoadManager.SavePDP(playerData, SavedFileNameHolder.PlayerData);

            return playerData.UnlockedSkins;
        }
    }

    [HideInInspector]
    public UnityEvent OnSkinBuy = new UnityEvent();

    /// <summary>
    /// Checks if skin is gained by the player this consider saved data.
    /// </summary>
    /// <param name="skinID"></param>
    /// <param name="skinType"></param>
    /// <returns></returns>
    [Button]
    public bool GetSkin(string skinID, out SkinType skinType)
    {
        if (!SkinArchive.ContainsKey(skinID))
        {
            skinType = SkinType.FullBody;
            return false;
        }
        return SkinArchive.TryGetValue(skinID, out skinType);
    }

    [Button]
    public bool GetSkin(string skinID)
    {
        return SkinArchive.ContainsKey(skinID);
    }

    /// <summary>
    /// Checks if given id is the target type. Use this to filter skins
    /// </summary>
    /// <param name="skinID"></param>
    /// <param name="targetType"></param>
    /// <returns></returns>
    public bool GetSkinByType(string skinID, SkinType targetType)
    {
        for (int i = 0; i < SkinData.Skins.Count; i++)
        {
            if (string.Equals(SkinData.Skins[i].SkinID, skinID))
            {
                return SkinData.Skins[i].SkinType == targetType;
            }
        }

        return false;
    }

    public Skin GetExculsiveSkin(out int percentage)
    {
        PlayerData playerData = SaveLoadManager.LoadPDP<PlayerData>(SavedFileNameHolder.PlayerData, new PlayerData());

        if (playerData == null)
            playerData = new PlayerData();

        percentage = playerData.CurrentLoadingSkinTier;

        if (!string.IsNullOrEmpty(playerData.CurrentLoadingSkin))
        {
            if (!GetSkin(playerData.CurrentLoadingSkin))
            {
                for (int i = 0; i < SkinData.Skins.Count; i++)
                {
                    if (string.Equals(SkinData.Skins[i].SkinID, playerData.CurrentLoadingSkin))
                    {
                        if (percentage <= SkinData.Skins[i].skinTiers.Count)
                            return SkinData.Skins[i];
                    }
                }
            }
        }


        for (int i = 0; i < SkinData.Skins.Count; i++)
        {
            if (SkinData.Skins[i].SkinCategory == SkinCategory.Exculisive)
            {
                if (!GetSkin(SkinData.Skins[i].SkinID))
                    return SkinData.Skins[i];
            }
        }

        return null;
    }

    /// <summary>
    /// You must use the id from SkinData, manual data writing might result on bugs.
    /// </summary>
    /// <param name="skinID"></param>
    [Button]
    public void SaveSkin(string skinID, SkinType skinType)
    {
        AnalitycsManager.Instance.LogEvent("Skin_Event", "SkinPurchuse", skinID);
        PlayerData playerData = SaveLoadManager.LoadPDP<PlayerData>(SavedFileNameHolder.PlayerData, new PlayerData());
        playerData.CurrentSkin = skinID;
        SaveLoadManager.SavePDP(playerData, SavedFileNameHolder.PlayerData);
        SkinArchive[skinID] = skinType;
        SaveLoadManager.SavePDP(SkinArchive, "SkinData");
        OnSkinBuy.Invoke();
    }
}