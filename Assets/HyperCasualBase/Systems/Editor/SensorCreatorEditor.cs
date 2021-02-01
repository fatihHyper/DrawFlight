using UnityEngine;
using Sirenix.OdinInspector.Editor;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(CarSensorController))]
public class SensorCreatorEditor : OdinEditor
{
    private bool isDrawn = false;

    CarSensorController carSensorController;
    protected override void OnEnable()
    {
        if (Application.isPlaying) return;
        base.OnEnable();
        carSensorController = (CarSensorController)target;

        if (!isDrawn)
        {
            carSensorController.CreateSensors();
            isDrawn = true;
        }


    }

}
