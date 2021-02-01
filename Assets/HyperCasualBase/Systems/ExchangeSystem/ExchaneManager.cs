using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;


public class ExchaneManager : Singleton<ExchaneManager>
{

    private bool isInitialized = false;
    private Dictionary<ExchangeType, int> data = new Dictionary<ExchangeType, int>();

    public static UnityEvent OnExchange = new UnityEvent();

    public void Init()
    {
        Load();
        isInitialized = true;
    }

    public int GetData(ExchangeType exchangeType)
    {

        if (!isInitialized)
        {
            Init();
        }

        if (data.ContainsKey(exchangeType))
        {
            return data[exchangeType];
        }

        return 0;
    }

    [Button]
    // returns if result clamped to 0
    public bool DoExchange(ExchangeType exchangeType, int diff)
    {
        if (data.ContainsKey(exchangeType))
        {
            data[exchangeType] += diff;
        }
        else
        {
            data[exchangeType] = diff;
        }

        if (data[exchangeType] < 0)
        {
            data[exchangeType] = 0;
            if (OnExchange != null)
            {
                OnExchange.Invoke();
            }
            return false;
        }

        if(OnExchange != null)
        {
            OnExchange.Invoke();
        }

        return true;
    }

    private void Save() {
        var playerData = SaveLoadManager.LoadPDP<PlayerData>(SavedFileNameHolder.PlayerData, new PlayerData());
        playerData.CurrencyData = data;
        SaveLoadManager.SavePDP(playerData, SavedFileNameHolder.PlayerData);
    }

    private void Load() {
        data = SaveLoadManager.LoadPDP<PlayerData>(SavedFileNameHolder.PlayerData, new PlayerData()).CurrencyData;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Save();
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    public void OnDestroy()
    {
        Save();
        base.OnDestroy();
    }
}
