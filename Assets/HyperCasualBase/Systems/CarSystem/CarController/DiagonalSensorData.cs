using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DiagonalSensorData : ISensor
{
    private const float MAX_ANGLE = 30.0f;
    private float angleBetween;
    private List<SensorRayData> sensorRayDatas;
    private Vector3 dirToTarget;
    private SensorData sensorData;

    public void SetRayProperties()
    {
        Vector3 tempDirToTarget = default;
        for (int i = 0; i < sensorData.RayCount; i++)
        {
            if ((i % 2) == 0)
            {
                tempDirToTarget = Quaternion.Euler(0, -angleBetween * i, 0) * dirToTarget;
                Vector3 targetPt = sensorData.rayStartPoint + tempDirToTarget;
                Vector3 targetPtToWorlPt = sensorData.carTf.TransformPoint(targetPt);
                SensorRayData sensorRayData = new SensorRayData(targetPtToWorlPt, 0);
                sensorRayDatas.Add(sensorRayData);
            }
            else
            {
                tempDirToTarget = Quaternion.Euler(0, angleBetween * i, 0) * dirToTarget;
                Vector3 targetPt = sensorData.rayStartPoint + tempDirToTarget;
                Vector3 targetPtToWorlPt = sensorData.carTf.TransformPoint(targetPt);
                SensorRayData sensorRayData = new SensorRayData(targetPtToWorlPt, 0);
                sensorRayDatas.Add(sensorRayData);
            }
         
        }
        
    }
    private Vector3 GetTargetWorldPoint(int index)
    {
        Vector3 tempDirToTarget = default;
        if ((index % 2) == 0)
        {
            tempDirToTarget = Quaternion.Euler(0, -angleBetween * index, 0) * dirToTarget;
            Vector3 targetPt = sensorData.rayStartPoint + tempDirToTarget;
            Vector3 targetPtToWorlPt = sensorData.carTf.TransformPoint(targetPt);
            return targetPtToWorlPt;
        }
        else
        {
            tempDirToTarget = Quaternion.Euler(0, angleBetween * index, 0) * dirToTarget;
            Vector3 targetPt = sensorData.rayStartPoint + tempDirToTarget;
            Vector3 targetPtToWorlPt = sensorData.carTf.TransformPoint(targetPt);
            return targetPtToWorlPt;
        }

    }
    public void CreateSensor(Transform carTf, Bounds bounds, Side key, in SensorData sensorData)
    {
        this.sensorData = sensorData;

        sensorData.carTf = carTf;
        sensorData.Side = key;
        sensorData.bounds = bounds;

        angleBetween = MAX_ANGLE / (sensorData.RayCount);
        sensorRayDatas = new List<SensorRayData>();

        switch (sensorData.Side)
        {
            case Side.FRONTLEFTDIAGONAL:
                dirToTarget = (carTf.forward - carTf.right).normalized;
                sensorData.rayStartPoint = new Vector3(bounds.min.x, bounds.center.y, bounds.max.z) + dirToTarget * sensorData.RayOffset;
                break;
            case Side.FRONTRIGHTDIAGONAL:
                dirToTarget = (carTf.forward + carTf.right).normalized;
                sensorData.rayStartPoint = new Vector3(bounds.max.x, bounds.center.y, bounds.max.z) + dirToTarget * sensorData.RayOffset;
                break;
            case Side.BACKLEFTDIAGONAL:
                dirToTarget = (-carTf.forward - carTf.right).normalized;
                sensorData.rayStartPoint = new Vector3(bounds.min.x, bounds.center.y, bounds.min.z) + dirToTarget * sensorData.RayOffset;
                break;
            case Side.BACKRIGHTTDIAGONAL:
                dirToTarget = (carTf.right - carTf.forward).normalized;
                sensorData.rayStartPoint = new Vector3(bounds.max.x, bounds.center.y, bounds.min.z) + dirToTarget * sensorData.RayOffset;
                break;
            default:
                break;
        }

        SetRayProperties();
    }
    public SensorResultData GetSensorResultData(LayerMask layerMask)
    {
        Vector3 rayStartPointToWorldPt = sensorData.carTf.TransformPoint(sensorData.rayStartPoint + dirToTarget * sensorData.RayOffset);
        float totalAvoidSensitivity = 0.0f;
        float totalDist = 0.0f;
        int hitCount = 0;
        for (int i = 0; i < sensorRayDatas.Count; i++)
        {
            Vector3 rayDirection = (sensorRayDatas[i].rayOriginPt - rayStartPointToWorldPt) ;
            bool isHit = Physics.Raycast(rayStartPointToWorldPt, rayDirection, out RaycastHit hitInfo, sensorData.RayDistance,layerMask); //layer can be added

            if (isHit)
            {
                //Debug.Log( sensorData.Side);
                //Debug.Log(hitInfo.collider.name);
                Debug.DrawLine(rayStartPointToWorldPt,hitInfo.point, Color.green);
                totalAvoidSensitivity = 1;
                hitCount++;
                totalDist += hitInfo.distance;

            }
            else
            {
                totalAvoidSensitivity = 0;
            }
        }


        var sensorResultData = new SensorResultData(hitCount, totalAvoidSensitivity, totalDist/hitCount);
        return sensorResultData;
    }
    public void UpdateRayProperties()
    {
        for (int i = 0; i < sensorRayDatas.Count; i++)
            sensorRayDatas[i].rayOriginPt = GetTargetWorldPoint(i);
    }
    public void DrawProperties()
    {
        if (sensorRayDatas == null) return;

        UpdateRayProperties();

        Vector3 offset = dirToTarget * sensorData.RayOffset;

        Vector3 rayStartPointToWorldPt = sensorData.carTf.TransformPoint(sensorData.rayStartPoint + offset);
        Gizmos.DrawSphere(rayStartPointToWorldPt, 0.05f);

        for (int i = 0; i < sensorRayDatas.Count; i++)
        {
            Vector3 rayDir = sensorRayDatas[i].rayOriginPt;
            Vector3 dirOffset = (rayDir /*- rayStartPointToWorldPt*/).normalized * sensorData.RayDistance;

            Gizmos.DrawSphere(rayDir + dirOffset, 0.05f);
            Gizmos.DrawRay(rayStartPointToWorldPt, rayDir );
        }
    }

}


