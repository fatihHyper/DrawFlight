using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class HeatmapTest : MonoBehaviour
{
    [InfoBox("Add HeatmappingManager to Managers")]

    public float Value = 90f;
    public int FullValueRange = 2;
    public int TotalRange = 4;

    [FoldoutGroup("Change Manually", expanded: false)]
    public int x, y;

    [FoldoutGroup("Change Manually", expanded: false)]
    [Button("SetValue")]
    public void SetTileValueManuel()
    {
        HeatmappingManager.Instance.SetValue(x, y, Value);
        Debug.Log(GridManager.Instance.Grid[x, y].gameObject.name + " " + HeatmappingManager.Instance.GetValue(x, y));
    }

    [FoldoutGroup("Change Manually", expanded: false)]
    [Button("GetValue")]
    public void GetTileValueManuel()
    {
        Debug.Log(GridManager.Instance.Grid[x, y].gameObject.name + " " + HeatmappingManager.Instance.GetValue(x, y));
    }

    [FoldoutGroup("Change Manually", expanded: false)]
    [Button("AddValueWithRange")]
    public void AddValueWithRangeManuel()
    {
        HeatmappingManager.Instance.AddValueWithRange(GridManager.Instance.Grid[x, y], Value, FullValueRange, TotalRange);
        Debug.Log(GridManager.Instance.Grid[x, y].gameObject.name + " " + HeatmappingManager.Instance.GetValue(x, y));
    }

    private void Update()
    {
        AddValueWithRange();
        GetValue();
    }

    public void SetValue()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                GridTile gridTile = hit.collider.gameObject.GetComponent<GridTile>();
                HeatmappingManager.Instance.SetValue(gridTile, Value);
                Debug.Log(gridTile.gameObject.name + " " + HeatmappingManager.Instance.GetValue(gridTile));
            }
        }
    }

    public void AddValueWithRange()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                GridTile gridTile = hit.collider.gameObject.GetComponent<GridTile>();
                HeatmappingManager.Instance.AddValueWithRange(gridTile, Value, FullValueRange, TotalRange);
                Debug.Log(gridTile.gameObject.name + " " + HeatmappingManager.Instance.GetValue(gridTile));

            }
        }
    }

    public void GetValue()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                GridTile gridTile = hit.collider.gameObject.GetComponent<GridTile>();
                Debug.Log(gridTile.gameObject.name + " " + HeatmappingManager.Instance.GetValue(gridTile));
            }
        }
    }

}
