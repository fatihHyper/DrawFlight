using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using AdvanceUI;
using System;

public class CoinPanel : AdvancePanel
{
    public TextMeshProUGUI CoinDisplayText;
    bool isCoinGet;
    private void OnEnable()
    {
       
        EventManager.OnCoinPickUp.AddListener(() => isCoinGet = true);
    }

    private void OnDisable()
    {
        EventManager.OnCoinPickUp.RemoveListener(() => isCoinGet = true);
    }

    private void Update()
    {
        if (isCoinGet)
        {
            SetCoinCount();
            isCoinGet = false;
            
        }
        
    }
    private void SetCoinCount()
    {
        int cointCount = PlayerPrefs.GetInt(PlayerPrefKeys.CoinCount, 1);
        cointCount++;
        PlayerPrefs.SetInt(PlayerPrefKeys.CoinCount, cointCount);
        CoinDisplayText.text = cointCount.ToString();
    }
}
