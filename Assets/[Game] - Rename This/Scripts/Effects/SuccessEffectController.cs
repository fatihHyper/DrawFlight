using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class SuccessEffectController : EffectControllerBase
{



    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;

        GameManager.Instance.OnStageSuccess.AddListener(PlayParticleEffect);
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;

        GameManager.Instance.OnStageSuccess.RemoveListener(PlayParticleEffect);
    }

    private void OnParticleSystemStopped()
    {
        LevelManager.Instance.LoadNextLevel();
    }
}
