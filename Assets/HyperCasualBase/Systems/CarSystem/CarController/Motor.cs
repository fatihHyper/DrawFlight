using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Motor : InterfaceBase, IMotor
{
    private const float PERIOD_TIME = 60.0F / 1000.0F;
    private CarController carController;
    private CarSensorController carSensorController;
    private Rigidbody rbody;

    public WheelHolder FrontWheelHolder { get; set; }
    public List<WheelHolder> RearWheelHolders { get; set; }
    public GasParticleHolder GasParticleHolder { get; set; }
    public CarController CarController { get { return (carController == null) ? carController = GetComponent<CarController>() : carController; } }
    public CarSensorController CarSensorController { get { return (carSensorController == null) ? carSensorController = GetComponent<CarSensorController>() : carSensorController; } }
    public CarControlData CarControlData { get { return CarController.CarData.CarControlData; } }
    public Rigidbody Rigidbody { get { return (rbody == null) ? rbody = GetComponent<Rigidbody>() : rbody; } }

    public float CurrentSpeed
    {
        get
        {
            var wheelCollider = RearWheelHolders[0].Wheels[0].WheelCollider;
            return 2 * Mathf.PI * wheelCollider.rpm * wheelCollider.radius * PERIOD_TIME;
        }
    }
    public Vector3 TargetPoint { get; set; }

    private void OnEnable()
    {
        if (Managers.Instance == null) return;
        CarController.OnCarGraphicSet.AddListener(SetMotorProperties);
    }
    private void OnDisable()
    {
        if (Managers.Instance == null) return;
        CarController.OnCarGraphicSet.RemoveListener(SetMotorProperties);
    }
    public void SetMotorTorque(float value)
    {
        var wheelCollider = RearWheelHolders[0].Wheels[0].WheelCollider;
        if (!wheelCollider.isGrounded) return;

        //// add motor torque to front wheels
        for (int i = 0; i < FrontWheelHolder.Wheels.Count; i++)
        {
            FrontWheelHolder.Wheels[i].AddTorque(CarControlData.MotorTorque * value);
        }

        // add motor torque to rear wheels
        for (int i = 0; i < RearWheelHolders.Count; i++)
        {
            for (int j = 0; j < RearWheelHolders[i].Wheels.Count; j++)
            {
                var wheel = RearWheelHolders[i].Wheels[j];
                wheel.AddTorque(CarControlData.MotorTorque * value);
            }
        }
    }
    public void AnimateWheels()
    {
        // animate front wheels
        for (int i = 0; i < FrontWheelHolder.Wheels.Count; i++)
        {
            FrontWheelHolder.Wheels[i].AnimateWheel();
        }

        // animate rear wheels
        for (int i = 0; i < RearWheelHolders.Count; i++)
        {
            for (int j = 0; j < RearWheelHolders[i].Wheels.Count; j++)
            {
                RearWheelHolders[i].Wheels[j].AnimateWheel();
            }
        }
    }
    public void Steer(float inputSteer)
    {
        FrontWheelHolder.Steer(inputSteer, CarControlData);
    }
    public void SetBrakeTorque(float brakeValue)
    {
        brakeValue = Mathf.Abs(brakeValue);

        // add brake torque to front wheels
        for (int i = 0; i < FrontWheelHolder.Wheels.Count; i++)
        {
            FrontWheelHolder.Wheels[i].Brake(CarControlData.BrakeTorque * brakeValue);
        }

        // add brake torque to rear wheels
        for (int i = 0; i < RearWheelHolders.Count; i++)
        {
            for (int j = 0; j < RearWheelHolders[i].Wheels.Count; j++)
            {
                RearWheelHolders[i].Wheels[j].Brake(CarControlData.BrakeTorque * brakeValue);
            }
        }
    }
    private void SetMotorProperties(GasParticleHolder gasParticleHolder, WheelHolder frontWheel, List<WheelHolder> rearWheels)
    {
        GasParticleHolder = gasParticleHolder;
        FrontWheelHolder = frontWheel;
        RearWheelHolders = rearWheels;
    }

    public float GetSteerValue()
    {
        Vector3 targetPtToLocalPt = transform.InverseTransformPoint(new Vector3(TargetPoint.x, transform.position.y, TargetPoint.z));
        float steerValue = targetPtToLocalPt.normalized.x;

        return steerValue;
    }

    public void SpringSystem()
    {
        // Work out the stiffness and damper parameters based on the better spring model.
        for (int i = 0; i < FrontWheelHolder.Wheels.Count; i++)
        {
            FrontWheelHolder.Wheels[i].SpringSystem(Rigidbody);
        }

        for (int i = 0; i < RearWheelHolders.Count; i++)
        {
            for (int j = 0; j < RearWheelHolders[i].Wheels.Count; j++)
            {
                RearWheelHolders[i].Wheels[j].SpringSystem(Rigidbody);
            }
        }
    }


}
