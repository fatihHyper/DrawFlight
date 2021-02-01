using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using AdvanceUI;
using System;

public class LevelPanel : AdvancePanel
{
    public TextMeshProUGUI LevelDisplayText;


    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;

        LevelManager.Instance.OnLevelStart.AddListener(ShowPanelAnimated);
        LevelManager.Instance.OnLevelFinish.AddListener(HidePanelAnimated);
        GameManager.Instance.OnStageSuccess.AddListener(SetFakeLevel);
    }



    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;

        LevelManager.Instance.OnLevelStart.RemoveListener(ShowPanelAnimated);
        LevelManager.Instance.OnLevelFinish.RemoveListener(HidePanelAnimated);
        GameManager.Instance.OnStageSuccess.AddListener(SetFakeLevel);
    }

    public override void ShowPanelAnimated()
    {
        base.ShowPanelAnimated();
        LevelDisplayText.SetText("Level " + PlayerPrefs.GetInt(PlayerPrefKeys.FakeLevel, 1));
    }

    private void SetFakeLevel()
    {
        int fakeLevel = PlayerPrefs.GetInt(PlayerPrefKeys.FakeLevel, 1);
        fakeLevel++;
        PlayerPrefs.SetInt(PlayerPrefKeys.FakeLevel, fakeLevel);
    }


}
