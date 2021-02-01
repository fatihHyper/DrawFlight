using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SensorRayData
{
    public Vector3 rayOriginPt;
    public float avoidValue;

    public SensorRayData(Vector3 rayOriginPt, float avoidValue)
    {
        this.rayOriginPt = rayOriginPt;
        this.avoidValue = avoidValue;
    }
}

[System.Serializable]
public struct SensorResultData
{
    public int hittedRayCount;
    public float avoidSensitivity;
    public float totalAverageDistance;
    public Vector3 totalAverageNormal;

    public SensorResultData(int hittedRayCount, float avoidSensitivity, float totalAverageDistance, Vector3 midRayNormal = default)
    {
        this.totalAverageDistance = totalAverageDistance;
        this.totalAverageNormal = midRayNormal;
        this.hittedRayCount = hittedRayCount;
        this.avoidSensitivity = avoidSensitivity;
    }
}



[System.Serializable]
public class StraightSensorData : ISensor
{
    private List<SensorRayData> sensorRayDatas;
    private float sizeBetweensRay;
    private Vector3 rayPosIncreaseDir;
    private Vector3 rayDirection;
    private SensorData sensorData;

    private Vector3 GetRayOriginWorldPoint(int i)
    {
        Vector3 rayOrigin = sensorData.rayStartPoint + rayPosIncreaseDir * sizeBetweensRay * i;
        Vector3 rayOriginToWorldPt = sensorData.carTf.TransformPoint(rayOrigin);

        return rayOriginToWorldPt;
    }
    public void SetRayProperties()
    {
        for (int i = 0; i < sensorData.RayCount; i++)
        {
            Vector3 rayOrigin = sensorData.rayStartPoint + rayPosIncreaseDir * sizeBetweensRay * i;
            float avoidValue = (rayOrigin.x / sensorData.bounds.extents.x);
            SensorRayData sensorRayData = new SensorRayData(rayOrigin, avoidValue);
            sensorRayDatas.Add(sensorRayData);
        }
    }
    public void CreateSensor(Transform carTf, Bounds bounds, Side key, in SensorData sensorData)
    {
        this.sensorData = sensorData;
        this.sensorData.bounds = bounds;
        sensorData.carTf = carTf;
        sensorData.Side = key;

        sensorRayDatas = new List<SensorRayData>();

        float RayOffset = sensorData.RayOffset;
        int RayCount = sensorData.RayCount;


        switch (sensorData.Side)
        {
            case Side.FRONT:
                sensorData.rayStartPoint = new Vector3(bounds.min.x, bounds.center.y, bounds.max.z + RayOffset);
                sizeBetweensRay = bounds.size.x / (RayCount - 1);
                rayPosIncreaseDir = Vector3.right;
                rayDirection = carTf.forward;
                break;
            case Side.BACK:
                sensorData.rayStartPoint = new Vector3(bounds.min.x, bounds.center.y, bounds.min.z - RayOffset);
                sizeBetweensRay = bounds.size.x / (RayCount - 1);
                rayPosIncreaseDir = Vector3.right;
                rayDirection = -carTf.forward;
                break;
            case Side.LEFT:
                sensorData.rayStartPoint = new Vector3(bounds.min.x - RayOffset, bounds.center.y, bounds.min.z);
                sizeBetweensRay = bounds.size.z / (RayCount - 1);
                rayPosIncreaseDir = Vector3.forward;
                rayDirection = -carTf.right;
                break;
            case Side.RIGHT:
                sensorData.rayStartPoint = new Vector3(bounds.max.x + RayOffset, bounds.center.y, bounds.min.z);
                sizeBetweensRay = bounds.size.z / (RayCount - 1);
                rayPosIncreaseDir = Vector3.forward;
                rayDirection = carTf.right;
                break;
            default:
                break;
        }

        SetRayProperties();
    }

    public SensorResultData GetSensorResultData(LayerMask layerMask)
    {
        float totalAvoidSensitivity = 0.0f;
        float totalDist = 0.0f;
        Vector3 totalNormal = Vector3.zero;
        int hitCount = 0;
        for (int i = 0; i < sensorRayDatas.Count; i++)
        {
            bool isHit = Physics.Raycast(sensorRayDatas[i].rayOriginPt, rayDirection, out RaycastHit hitInfo, sensorData.RayDistance, layerMask); //layer can be added

            if (isHit)
            {
                Vector3 startPt = rayDirection * sensorData.RayOffset + sensorRayDatas[i].rayOriginPt;
                Vector3 reflectVec = Vector3.Reflect(hitInfo.point - startPt, hitInfo.normal);
                Debug.DrawLine(startPt, hitInfo.point, Color.green);
                Debug.DrawRay(hitInfo.point, reflectVec, Color.blue);
                // Debug.DrawRay(startPt, hitInfo.normal, Color.blue); test
                totalAvoidSensitivity += sensorRayDatas[i].avoidValue;
                hitCount++;
                totalDist += hitInfo.distance;
                totalNormal += reflectVec;
                //totalNormal += hitInfo.normal; test
            }
        }
        var sensorResultData = new SensorResultData( hitCount, totalAvoidSensitivity, totalDist / hitCount, (totalNormal / hitCount).normalized);

        return sensorResultData;
    }
    public void UpdateRayProperties()
    {
        switch (sensorData.Side)
        {
            case Side.FRONT:
                rayDirection = sensorData.carTf.forward;
                break;
            case Side.BACK:
                rayDirection = -sensorData.carTf.forward;
                break;
            case Side.LEFT:
                rayDirection = -sensorData.carTf.right;
                break;
            case Side.RIGHT:
                rayDirection = sensorData.carTf.right;
                break;
            default:
                break;
        }
        for (int i = 0; i < sensorRayDatas.Count; i++)
            sensorRayDatas[i].rayOriginPt = GetRayOriginWorldPoint(i);
    }
    public void DrawProperties()
    {
        if (sensorRayDatas == null) return;
        UpdateRayProperties();


        for (int i = 0; i < sensorRayDatas.Count; i++)
        {
            Vector3 rayOriginPt = sensorRayDatas[i].rayOriginPt;

            Gizmos.DrawSphere(rayDirection * sensorData.RayOffset + rayOriginPt, .05F);
            Gizmos.DrawRay(rayDirection * sensorData.RayOffset + rayOriginPt, rayDirection * sensorData.RayDistance);
        }
    }

}


