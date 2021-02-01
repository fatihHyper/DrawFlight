using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeTestGrid : MonoBehaviour
{
    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;

        SceneController.Instance.OnSceneLoaded.AddListener(TestGrid);
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;

        SceneController.Instance.OnSceneLoaded.RemoveListener(TestGrid);
    }


    private void TestGrid()
    {
        GameObject GridParent = new GameObject("NewGrid");
        GridParent.transform.SetParent(transform);
        GridManager.Instance.GenerateGrid(GridParent);
        GridParent.transform.position = transform.position;
        GridParent.transform.rotation = transform.rotation;
    }
}
