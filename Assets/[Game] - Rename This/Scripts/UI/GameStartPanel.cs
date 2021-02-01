using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvanceUI;

public class GameStartPanel : AdvancePanel
{
    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;

        LevelManager.Instance.OnLevelStart.AddListener(HidePanel);
        SceneController.Instance.OnSceneLoaded.AddListener(ShowPanel);
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;

        LevelManager.Instance.OnLevelStart.RemoveListener(HidePanel);
        SceneController.Instance.OnSceneLoaded.RemoveListener(ShowPanel);
    }
}
