
public class GoForwardAction : ActionBehaviour
{
    public override void Act(CarBrainBase carBrainBase)
    {
        IMotor motor = carBrainBase.Motor;
        motor.SetBrakeTorque(0);
        motor.SetMotorTorque(1);
    }
}
