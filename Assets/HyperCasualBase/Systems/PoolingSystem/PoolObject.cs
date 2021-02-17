using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    private void Start()
    {
        if (Managers.Instance == null)
            return;

        GameManager.Instance.OnStageFail.AddListener(() => PoolingSystem.Instance.DestroyAPS(gameObject));
        GameManager.Instance.OnStageSuccess.AddListener(() => PoolingSystem.Instance.DestroyAPS(gameObject));

    }

    private void OnDestroy()
    {
        if (Managers.Instance == null)
            return;

        GameManager.Instance.OnStageFail.RemoveListener(() => PoolingSystem.Instance.DestroyAPS(gameObject));
        GameManager.Instance.OnStageSuccess.RemoveListener(() => PoolingSystem.Instance.DestroyAPS(gameObject));
    }
}
