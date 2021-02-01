using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using AdvanceUI;


public class AQOptionButton : AdvanceButton
{
    private AnswerData answerData;

    protected override void OnEnable()
    {
        base.OnEnable();
        OnClickEvent.AddListener(ExecuteOption);
        UIIDHolder.Panels[UIIDHolder.QAPanel].OnHidePanel.AddListener(HidePanel);
        if (Managers.Instance == null)
            return;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        OnClickEvent.RemoveListener(ExecuteOption);
        UIIDHolder.Panels[UIIDHolder.QAPanel].OnHidePanel.RemoveListener(HidePanel);
        if (Managers.Instance == null)
            return;
    }

    public void InitializeButton(AnswerData data)
    {
        answerData = data;
        ComponentText = answerData.Answer;
        SetGraphic();
    }

    public void ExecuteOption()
    {
        var qaPanel = UIIDHolder.Panels[UIIDHolder.QAPanel] as QAPanel;
        if (!answerData.isCorrect)
        {
            qaPanel.HidePanel();
            GameManager.Instance.CompilateStage(false);
        }
        else
        {
            EventManager.OnCorrectAnswer.Invoke();
            qaPanel.LoadNextQuestion();
        }
    }
}
