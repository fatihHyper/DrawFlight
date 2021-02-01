using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

public class HeatmappingManager : Singleton<HeatmappingManager>
{
    public Gradient HeatMapGradient;
    public float[,] HeatMapTable;

    private GridData gridData;
    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;

        SceneController.Instance.OnSceneLoaded.AddListener(GetGridData);
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;

        SceneController.Instance.OnSceneLoaded.RemoveListener(GetGridData);
    }
    
    public void GetGridData()
    {
        gridData = GridManager.Instance.GridData;
        HeatMapTable = new float[GridManager.Instance.Grid.GetLength(0), GridManager.Instance.Grid.GetLength(0)];
    }

    public void SetValue(int x, int y, float value)
    {
        if (GridManager.Instance.IsXYValid(x, y) && gridData.WalkableTable[x, y])
        {
            HeatMapTable[x, y] = Mathf.Clamp(value, gridData.HeatMapMinValue, gridData.HeatMapMaxValue);
            HeatMapColor(x, y, value);
        }
    }

    public void SetValue(GridTile gridTile, float value)
    {
        int x, y;
        GridManager.Instance.GetXY(gridTile, out x, out y);
        SetValue(x, y, value);        
    }

    public float GetValue(int x, int y)
    {
        if (GridManager.Instance.IsXYValid(x, y))
            return HeatMapTable[x, y];
        return -1;
    }

    public float GetValue(GridTile gridTile)
    {
        int x, y;
        GridManager.Instance.GetXY(gridTile, out x, out y);
        return GetValue(x, y);
    }

    public void AddValue(int x, int y, float value)
    {
        SetValue(x, y, GetValue(x, y) + value);
    }


    public void AddValueWithRange(GridTile gridTile, float value, int fullValueRange, int totalRange)
    {
        int originX, originY;
        GridManager.Instance.GetXY(gridTile, out originX, out originY);

        int lowerValueAmount = Mathf.RoundToInt((float)value / (totalRange - fullValueRange));

        for (int x = 0; x < totalRange; x++)
        {
            for (int y = 0; y < totalRange - x; y++)
            {
                int radius = x + y;
                float addValueAmount = value;
                if (radius > fullValueRange)
                    addValueAmount -= lowerValueAmount * (radius - fullValueRange);

                AddValue(originX + x, originY + y, addValueAmount);

                if (x != 0)
                    AddValue(originX - x, originY + y, addValueAmount);
                if (y != 0)
                {
                    AddValue(originX + x, originY - y, addValueAmount);
                    if (x != 0)
                        AddValue(originX - x, originY - y, addValueAmount);
                }

            }
        }
    }


    public void HeatMapColor(int x, int y, float value)
    {
        float colorValue = Mathf.InverseLerp(0, 100, value);
        GridManager.Instance.Grid[x,y].gameObject.GetComponent<MeshRenderer>().material.color = HeatMapGradient.Evaluate(colorValue);
    }
}
