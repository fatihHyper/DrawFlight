using UnityEngine;

public class RightAvoidDecision : Decision
{
    public override bool Decide(CarBrainBase brainBase)
    {
        var motor = brainBase.Motor;
        float frontStraightAvoidSensitivity = motor.CarSensorController.SensorResultDatas[Side.FRONT].avoidSensitivity;
        float frontRightDiagonalAvoidSensitivity = motor.CarSensorController.SensorResultDatas[Side.FRONTRIGHTDIAGONAL].avoidSensitivity;
        float velMagnitude = motor.Rigidbody.velocity.magnitude;

        return velMagnitude > 5.0f && (frontStraightAvoidSensitivity > 0f || frontRightDiagonalAvoidSensitivity > 0.0f);
    }
}
