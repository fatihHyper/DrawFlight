using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateUsManager : Singleton<RateUsManager>
{
    public long CheckPeriod = 60 * 60 * 24; //1 Day
    public string IosStorePath;
    public string AndroidStorePath;

    private RateUsData data = new RateUsData();
    bool isInitialized = false;
    void Start()
    {
        Load();
    }

    public bool CheckRateUs()
    {
        if (!isInitialized)
        {
            Load();
        }
        if (data.IsRated /*|| DateTime.Now.EpochTime() - CheckPeriod < data.LastShownTime*/ ) return false;

        return true;
    }

    public void RateLater() {
        data.LastShownTime = 0; //  DateTime.Now.EpochTime()
        Save();
    }

    public void Rate()
    {
#if UNITY_ANDROID
        Application.OpenURL(AndroidStorePath);
#elif UNITY_IOS
        Application.OpenURL(IosStorePath);
#else
#endif
        data.IsRated = true;
        Save();
    }

    private void Save()
    {
        PlayerData playerData = SaveLoadManager.LoadPDP<PlayerData>(SavedFileNameHolder.PlayerData, new PlayerData());
        playerData.RateUsData = data;
        SaveLoadManager.SavePDP(playerData, SavedFileNameHolder.PlayerData);
    }

    private void Load()
    {
        data = SaveLoadManager.LoadPDP<PlayerData>(SavedFileNameHolder.PlayerData, new PlayerData()).RateUsData;
        if (data == null)
        {
            data = new RateUsData();
        }
        isInitialized = true;
    }
}
