using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class InitiliazePanel : AdvanceUI.AdvancePanel
{

    private void Start()
    {
        ShowPanel();
    }

    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;

        SceneController.Instance.OnSceneLoaded.AddListener(HidePanel);

    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;

        SceneController.Instance.OnSceneLoaded.RemoveListener(HidePanel);

    }
}
