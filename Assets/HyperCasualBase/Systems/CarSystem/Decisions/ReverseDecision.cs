
using UnityEngine;

public class ReverseDecision : Decision
{
    public override bool Decide(CarBrainBase brainBase)
    {
        var motor = brainBase.Motor;
        int backHitCount = motor.CarSensorController.SensorResultDatas[Side.BACK].hittedRayCount;
        int frontHitCount = motor.CarSensorController.SensorResultDatas[Side.FRONT].hittedRayCount;
        float frontStraightTotalAverageDist = motor.CarSensorController.SensorResultDatas[Side.FRONT].totalAverageDistance;

        float velMagnitude = motor.Rigidbody.velocity.magnitude;
        return (backHitCount == 0 && frontHitCount > 0 && velMagnitude < 2.0f);


    }
}
