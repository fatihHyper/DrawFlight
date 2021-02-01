
public class ObstacleAvoidingBrakeDecision : Decision
{
    public override bool Decide(CarBrainBase brainBase)
    {
        var motor = brainBase.Motor;
        int frontHitCount = motor.CarSensorController.SensorResultDatas[Side.FRONT].hittedRayCount;
        float frontStraightAvoidSensitivity = motor.CarSensorController.SensorResultDatas[Side.FRONT].avoidSensitivity;
        float velMagnitude = motor.Rigidbody.velocity.magnitude;

        return (frontHitCount > 0 && frontStraightAvoidSensitivity == 0 && velMagnitude > 2.0f );
    }
}
