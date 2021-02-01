using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitNormalAvoidDecision : Decision
{
    public override bool Decide(CarBrainBase brainBase)
    {
        var motor = brainBase.Motor;
        float frontStraightAvoidSensitivity = motor.CarSensorController.SensorResultDatas[Side.FRONT].avoidSensitivity;
        float frontStraightTotalAverageDist = motor.CarSensorController.SensorResultDatas[Side.FRONT].totalAverageDistance;
        float frontLeftDiagonalHitCount = motor.CarSensorController.SensorResultDatas[Side.FRONTLEFTDIAGONAL].hittedRayCount;
        float frontRightDiagonalHitCount = motor.CarSensorController.SensorResultDatas[Side.FRONTRIGHTDIAGONAL].hittedRayCount;
        int frontHitCount = motor.CarSensorController.SensorResultDatas[Side.FRONT].hittedRayCount;
        float velMagnitude = motor.Rigidbody.velocity.magnitude;

        return (frontStraightTotalAverageDist > 8.0f && frontHitCount > 0 && frontLeftDiagonalHitCount == 0 && frontRightDiagonalHitCount == 0 && frontStraightAvoidSensitivity == 0 && velMagnitude > 2.0f);
    }
}
