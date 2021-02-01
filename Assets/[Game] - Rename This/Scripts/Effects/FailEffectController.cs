using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class FailEffectController : EffectControllerBase
{

    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;

        GameManager.Instance.OnStageFail.AddListener(PlayParticleEffect);

    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;

        GameManager.Instance.OnStageFail.RemoveListener(PlayParticleEffect);
    }

    private void OnParticleSystemStopped()
    {
        LevelManager.Instance.ReloadLevel();
    }
}
