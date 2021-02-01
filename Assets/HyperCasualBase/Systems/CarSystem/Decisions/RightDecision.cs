
public class RightDecision : Decision
{
    public override bool Decide(CarBrainBase brainBase)
    {
        var motor = brainBase.Motor;
        int totalHitCount = motor.CarSensorController.GetTotalHitCount();

        float steerValue = motor.GetSteerValue();
        return (totalHitCount == 0 && steerValue > 0);
    }
}
