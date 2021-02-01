using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;



public class LevelLoadingPanel : AdvanceUI.AdvancePanel
{

    public Transform BlackKnob;
    public Transform CutoutKnob;

    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;

        LevelManager.Instance.OnLevelFinish.AddListener(ShowPanel);
        SceneController.Instance.OnSceneLoaded.AddListener(HidePanel);
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;

        LevelManager.Instance.OnLevelFinish.RemoveListener(ShowPanel);
        SceneController.Instance.OnSceneLoaded.RemoveListener(HidePanel);
    }

    public override void ShowPanel()
    {
        base.ShowPanel();

        Sequence sequence = DOTween.Sequence();
        BlackKnob.localPosition = new Vector3(0, -1950, 0);
        CutoutKnob.localScale = Vector3.zero;
        BlackKnob.transform.localScale = Vector3.one * 0.3f;
        sequence
            .Append(BlackKnob.DOLocalMoveY(0, 1f).SetEase(Ease.OutSine))
            .Append(BlackKnob.DOScale(Vector3.one * 6, 1f).SetEase(Ease.InExpo));
        
    }

    public override void HidePanel()
    {
        Sequence sequence = DOTween.Sequence();

        sequence
            .Append(CutoutKnob.DOScale(Vector3.one * 6, 1f).SetEase(Ease.InExpo))
            .AppendCallback(() => base.HidePanel());
    }
}
