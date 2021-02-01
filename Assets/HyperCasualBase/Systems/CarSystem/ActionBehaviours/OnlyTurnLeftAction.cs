
using UnityEngine;

public class OnlyTurnLeftAction : ActionBehaviour
{
    public override void Act(CarBrainBase carBrainBase)
    {
        var motor = carBrainBase.Motor;
        float steerValue = motor.GetSteerValue();
        motor.Steer(steerValue);
    }
}
