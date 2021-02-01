using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EditorTestGrid : MonoBehaviour
{
    [Button("New Grid")]
    private void TestGrid()
    {
        GameObject GridParent = new GameObject("NewGrid");
        GridParent.transform.SetParent(transform);
        GridManager.Instance.GenerateGrid(GridParent);
        GridParent.transform.position = transform.position;
        GridParent.transform.rotation = transform.rotation;
    }   
}
