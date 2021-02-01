
public class MoveDecision : Decision
{
    public override bool Decide(CarBrainBase brainBase)
    {
        var motor = brainBase.Motor;
        int totalHitCount = motor.CarSensorController.GetTotalHitCount();
        return (totalHitCount == 0);
    }
}
