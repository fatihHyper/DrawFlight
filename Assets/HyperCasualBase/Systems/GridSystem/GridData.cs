using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum GridType { HorizantalGrid, VerticalGrid }
public enum GridPivotPoint { UpLeft , Center}

public class GridData : SerializedScriptableObject
{
    [Tooltip("x axis")]
    public int Width;
    [Tooltip("y axis")]
    public int Height;
    public float GridOffset;
    public Vector3 StartPos = new Vector3(0, 0, 0);

    public GameObject GridPrefab;
    public GridType GridType;
    public GridPivotPoint GridPivotPoint;

    public float HeatMapMaxValue;
    public float HeatMapMinValue;

    public bool IsDiagonalPathEnabled;

    public LayerMask GridLayer;

    [BoxGroup(" Walkable table")]
    [TableMatrix(DrawElementMethod = "DrawCell")]
    public bool[,] WalkableTable = new bool[10, 10];



    //[ShowInInspector, DoNotDrawAsReference]
    //[TableMatrix(HorizontalTitle = "Transposed Custom Cell Drawing", DrawElementMethod = "DrawCell", Transpose = true)]
    //public bool[,] Transposed { get { return WalkableTable; } set { WalkableTable = value; } }

#if UNITY_EDITOR

    private static bool DrawCell(Rect rect, bool value)
    {
        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            value = !value;
            GUI.changed = true;
            Event.current.Use();
        }

        EditorGUI.DrawRect(rect.Padding(1), value ? new Color(0.1f, 0.8f, 0.2f) : new Color(0, 0, 0, 0.5f));
        return value;
    }

#endif
}

