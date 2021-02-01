using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WheelHolder : MonoBehaviour
{
    private List<Wheel> wheels;
    public List<Wheel> Wheels { get { return (wheels == null) ? wheels = GetComponentsInChildren<Wheel>().ToList() : wheels; } }

    private List<Transform> wheelTransforms;
    public List<Transform> WheelTransforms { get { return (wheelTransforms == null) ? wheelTransforms = transform.Cast<Transform>().ToList() : wheelTransforms; } }

    public void Steer(float inputSteer, CarControlData carControlData)
    {
        float steerAngle = carControlData.MaxSteerAngle * inputSteer;

        for (int i = 0; i < Wheels.Count; i++)
        {
            var wheel = Wheels[i];
            wheel.WheelCollider.steerAngle = Mathf.Lerp(wheel.WheelCollider.steerAngle, steerAngle, Time.deltaTime  * carControlData.TurnSensitivity);
        }
    }
}
