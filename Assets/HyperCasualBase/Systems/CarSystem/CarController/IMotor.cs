using System.Collections.Generic;
using UnityEngine;

public interface IMotor
{
    float CurrentSpeed { get;  }
    Rigidbody Rigidbody { get;  }
    Vector3 TargetPoint { get; set; }

    CarControlData CarControlData { get; }
    CarSensorController CarSensorController { get; }
    WheelHolder FrontWheelHolder { get; set; }
    GasParticleHolder GasParticleHolder { get; set; }
    List<WheelHolder> RearWheelHolders { get; set; }


    float GetSteerValue();
    void AnimateWheels();
    void SpringSystem();
    void SetMotorTorque(float torqueValue);
    void SetBrakeTorque(float torqueValue);
    void Steer(float inputSteerValue);
}
