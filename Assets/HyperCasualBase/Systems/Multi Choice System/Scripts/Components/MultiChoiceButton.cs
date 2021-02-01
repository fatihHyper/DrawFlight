using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiChoiceButton : AdvanceUI.AdvanceButton
{
    private Choice Choice;


    protected override void OnEnable()
    {
        base.OnEnable();
        OnClickEvent.AddListener(ExecuteChoice);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        OnClickEvent.RemoveListener(ExecuteChoice);
    }

    private void ExecuteChoice()
    {
        var panel = UIIDHolder.Panels[UIIDHolder.MultiChoicePanel] as MultiChoicePanel;
        panel.MCControllerBase.Choice = Choice;
        panel.MCControllerBase.Do();
        panel.HidePanel();
    }

    public void Initialize(Choice choice)
    {
        Choice = choice;
        IconSprite = choice.ChoiceData.Icon;
        SetGraphic();
        UIIDHolder.Panels[UIIDHolder.MultiChoicePanel].OnHidePanel.AddListener(() => Destroy(gameObject));
    }

    private void OnDestroy()
    {
        UIIDHolder.Panels[UIIDHolder.MultiChoicePanel].OnHidePanel.RemoveListener(() => Destroy(gameObject));
    }
}
