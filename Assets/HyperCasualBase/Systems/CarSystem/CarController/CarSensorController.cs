using Sirenix.OdinInspector;
using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
public enum Side
{
    FRONT,
    BACK,
    LEFT,
    RIGHT,
    FRONTLEFTDIAGONAL,
    FRONTRIGHTDIAGONAL,
    BACKLEFTDIAGONAL,
    BACKRIGHTTDIAGONAL
}
public class CarSensorController : MonoBehaviour
{
    [SerializeField, HideInInspector]
    private Bounds bounds;

    [SerializeField]
    private LayerMask sensorLayer;

    private CarController carController;
    public CarController CarController { get { return (carController == null) ? carController = GetComponent<CarController>() : carController; } }

    [OnValueChanged("CreateSensors")]
    [OnValueChanged("SetDirtyEd")]
    public SideSensorDataPairDictionary SensorDatas;

     public Dictionary<Side, SensorResultData> SensorResultDatas = new Dictionary<Side, SensorResultData>(8);
    public Bounds Bounds { get { return (CarController.BodyCollider == null) ? default : CarController.BodyCollider.bounds; } }

    private void OnEnable()
    {
        if (Managers.Instance == null) return;
        CarController.OnCarInitialize.AddListener(CreateSensors);
        //CarController.OnCarInitialize.AddListener(ThrowRays);
    }
    private void OnDisable()
    {
        if (Managers.Instance == null) return;
        CarController.OnCarInitialize.RemoveListener(CreateSensors);
        //CarController.OnCarInitialize.RemoveListener(ThrowRays);
    }
    private void Update()
    {
        foreach (var item in SensorDatas)
        {
            DataHolder.SensorInitializerPairs[item.Key].UpdateRayProperties();
            SensorResultDatas[item.Key] = DataHolder.SensorInitializerPairs[item.Key].GetSensorResultData(sensorLayer);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (Bounds == null) return;

        if (!Application.isPlaying)
        {
            DrawSensorsRay();
        }
    }
#endif

    public void ThrowRays(CarData carData)
    {
        StartCoroutine(ThrowRaysCo(null));
    }
    private IEnumerator ThrowRaysCo(CarData carData)
    {
        while (true)
        {
            foreach (var item in SensorDatas)
            {
                DataHolder.SensorInitializerPairs[item.Key].UpdateRayProperties();
                DataHolder.SensorInitializerPairs[item.Key].GetSensorResultData(sensorLayer);
            }

            yield return new WaitForSeconds(.1f);
        }

    }
    private void DrawSensorsRay()
    {
        foreach (var item in SensorDatas)
        {
            DataHolder.SensorInitializerPairs[item.Key].DrawProperties();
        }
    }

    public void CreateSensors(CarData carData)
    {
        bounds = transform.TransformBounds(Bounds);

        foreach (var item in SensorDatas)
        {
            DataHolder.SensorInitializerPairs[item.Key].CreateSensor(transform, bounds, item.Key, item.Value);
        }


    }

    public int GetTotalHitCount()
    {
        int totalCount = 0;
        foreach (var item in SensorResultDatas)
        {
            totalCount += item.Value.hittedRayCount;
        }
        return totalCount;
    }

    [Button]
    public void CreateSensors()
    {

        CreateSensors(null);

    }

#if UNITY_EDITOR
    private void SetDirtyEd()
    {
        EditorUtility.SetDirty(this);
    }
#endif 

}

[Serializable]
public class SideSensorDataPairDictionary : UnitySerializedDictionary<Side, SensorData> { }
