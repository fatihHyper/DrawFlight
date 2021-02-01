using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Wheel : MonoBehaviour
{
    private WheelCollider wheelCollider;
    private WheelSkid wheelSkid;
    public WheelCollider WheelCollider { get => wheelCollider; set => wheelCollider = value; }
    public Transform Model { get; set; }

    private void OnEnable()
    {
        wheelCollider = gameObject.AddComponent<WheelCollider>();
        wheelSkid = gameObject.AddComponent<WheelSkid>();
    }
    public void AddTorque(float motorTorque)
    {
        wheelCollider.motorTorque = motorTorque;
    }
    public void Brake(float brakeTorque)
    {
        wheelCollider.brakeTorque = brakeTorque;
    }

    public void LerpRotation(float targetAngle, float rotateLerp)
    {
        WheelCollider.steerAngle = Mathf.Lerp(WheelCollider.steerAngle, targetAngle, Time.deltaTime * rotateLerp);
    }

    public void SetWheelColliderProps(WheelColliderData wheelColliderData)
    {
        wheelColliderData.WheelColliderPreset.ApplyTo(WheelCollider);
    }

    public void SpringSystem(Rigidbody rigidbody)
    {
        //TODO : ADD PARAMETERS TO CAR DATA

        float naturalFrequency = 10.0f;
        float dampingRatio = 0.8f;
        float forceShift = 0.03f;
        bool setSuspensionDistance = true;

        //TODO : ADD PARAMETERS TO CAR DATA

        JointSpring spring = WheelCollider.suspensionSpring;

        float sqrtWcSprungMass = Mathf.Sqrt(WheelCollider.sprungMass);
        spring.spring = sqrtWcSprungMass * naturalFrequency * sqrtWcSprungMass * naturalFrequency;
        spring.damper = 2f * dampingRatio * Mathf.Sqrt(spring.spring * WheelCollider.sprungMass);

        WheelCollider.suspensionSpring = spring;

        Vector3 wheelRelativeBody = transform.InverseTransformPoint(WheelCollider.transform.position);
        float distance = rigidbody.centerOfMass.y - wheelRelativeBody.y + WheelCollider.radius;

        WheelCollider.forceAppPointDistance = distance - forceShift;

        // Make sure the spring force at maximum droop is exactly zero
        if (spring.targetPosition > 0 && setSuspensionDistance)
            WheelCollider.suspensionDistance = WheelCollider.sprungMass * Physics.gravity.magnitude / (spring.targetPosition * spring.spring);
    }
    public void AnimateWheel()
    {
        wheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);

        Model.position = position;
        Model.rotation = rotation;
    }
}
