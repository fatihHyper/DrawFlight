using UnityEngine;

[System.Serializable]
public class SensorData
{
    [Range(1, 100)]
    public int RayCount = 2;
    [Range(0, 2)]
    public float RayOffset = 0f;
    [Range(0.1f, 20)]
    public float RayDistance = 1;
    [Range(0.1f, 1)]
    public float IntervalSeconds = 0.5f;

    [HideInInspector]
    public Side Side = Side.BACK;

    [HideInInspector]
    public Transform carTf;

    [HideInInspector]
    public Vector3 rayStartPoint;

    [HideInInspector]
    public Bounds bounds;

}


