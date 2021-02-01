using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLeftAvoidAction : ActionBehaviour
{
    public override void Act(CarBrainBase carBrainBase)
    {
        var motor = carBrainBase.Motor;
        float frontStraightAvoidSensitivity = motor.CarSensorController.SensorResultDatas[Side.FRONT].avoidSensitivity;
        float frontLeftDiagonalAvoidSensitivity = motor.CarSensorController.SensorResultDatas[Side.FRONTLEFTDIAGONAL].avoidSensitivity;
        carBrainBase.Motor.Steer(-Mathf.Max(-1, frontStraightAvoidSensitivity - frontLeftDiagonalAvoidSensitivity));
        motor.SetBrakeTorque(0);
        motor.SetMotorTorque(1);
    }
}
