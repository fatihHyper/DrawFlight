using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using AdvanceUI;


public class QAPanel : AdvancePanel
{

    public GameObject QAOptionPrefab;
    public Transform Holder;
    public TextMeshProUGUI QuestionText;
    private int questionIndex;

    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;

        LevelManager.Instance.OnLevelStart.AddListener(InitializePanel);
        EventManager.OnAskQuestion.AddListener(InitializeQuestion);

    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;

        LevelManager.Instance.OnLevelStart.RemoveListener(InitializePanel);
        EventManager.OnAskQuestion.RemoveListener(InitializeQuestion);
    }


    public override void ShowPanelAnimated()
    {
        InitializePanel();
    }

    [Button]
    private void InitializePanel()
    {
        questionIndex = 0;
        InitializeQuestion();
    }

    private void InitializeQuestion()
    {
        ShowPanelAnimated(() =>
        {
            for (int i = 0; i < LevelManager.Instance.CurrentLevel.QADatas[questionIndex].AnswerDatas.Count; i++)
            {
                var optionButton = Instantiate(QAOptionPrefab, Holder.transform).GetComponent<AQOptionButton>();
                optionButton.InitializeButton(LevelManager.Instance.CurrentLevel.QADatas[questionIndex].AnswerDatas[i]);
                optionButton.AnimationDelay = 0.2f * i;
                optionButton.ShowPanelAnimated();
            }
        });


        QuestionText.SetText(LevelManager.Instance.CurrentLevel.QADatas[questionIndex].Question);
    }

    public void LoadNextQuestion()
    {
        HidePanelAnimated(() =>
        {
            questionIndex++;
            if (questionIndex >= LevelManager.Instance.CurrentLevel.QADatas.Count)
            {
                GameManager.Instance.CompilateStage(true);
                return;
            }

            InitializeQuestion();
        });

    }
}
