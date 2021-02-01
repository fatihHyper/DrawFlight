using UnityEngine;

public interface ISensor
{
    void UpdateRayProperties();

    SensorResultData GetSensorResultData(LayerMask sensorLayerMask);

    void CreateSensor(Transform tf, Bounds bounds, Side key,in SensorData sensorData);

    void SetRayProperties();

    void DrawProperties();
}