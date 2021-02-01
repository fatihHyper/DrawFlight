using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MultiChoicePanel : AdvanceUI.AdvancePanel
{
    [HideInInspector]
    public MCControllerBase MCControllerBase;
    [HideInInspector]
    public MultiChoiceData MultiChoiceData;

    public Transform GraphicsHolder;
    public GameObject ChoiceButtonPrefab;

    public void ShowPanel(MCControllerBase mCControllerBase)
    {
        MCControllerBase = mCControllerBase;
        MultiChoiceData = MCControllerBase.MultiChoiceData;
        ShowPanel();
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
        CreateButtons();
    }
    private void CreateButtons()
    {
        foreach (Choice choice in MultiChoiceData.Choices)
        {
            MultiChoiceButton multiChoiceButton = Instantiate(ChoiceButtonPrefab, GraphicsHolder).GetComponent<MultiChoiceButton>();
            multiChoiceButton.Initialize(choice);
        }
    }

}
