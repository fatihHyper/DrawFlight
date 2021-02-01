using UnityEngine;

public class LeftAvoidDecision : Decision
{
    public override bool Decide(CarBrainBase brainBase)
    {
        var motor = brainBase.Motor;
        float frontStraightAvoidSensitivity = motor.CarSensorController.SensorResultDatas[Side.FRONT].avoidSensitivity;
        float frontLeftDiagonalAvoidSensitivity = motor.CarSensorController.SensorResultDatas[Side.FRONTLEFTDIAGONAL].avoidSensitivity;
        float velMagnitude = motor.Rigidbody.velocity.magnitude;
        return velMagnitude > 5.0f && (frontStraightAvoidSensitivity < 0f || frontLeftDiagonalAvoidSensitivity > 0f);
    }
}
