using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnRightAvoidAction : ActionBehaviour
{
    public override void Act(CarBrainBase carBrainBase)
    {
        var motor = carBrainBase.Motor;
        float frontStraightAvoidSensitivity = motor.CarSensorController.SensorResultDatas[Side.FRONT].avoidSensitivity;
        float frontRightDiagonalAvoidSensitivity = motor.CarSensorController.SensorResultDatas[Side.FRONTRIGHTDIAGONAL].avoidSensitivity;
        motor.Steer(-Mathf.Min(1, frontStraightAvoidSensitivity + frontRightDiagonalAvoidSensitivity ));
        motor.SetBrakeTorque(0);
        motor.SetMotorTorque(1);
    }
}
