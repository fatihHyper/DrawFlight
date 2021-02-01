
using UnityEngine;

public class HighSpeedBrakeDecision : Decision
{
    public override bool Decide(CarBrainBase brainBase)
    {
        var motor = brainBase.Motor;
        return motor.CurrentSpeed >= motor.CarControlData.MaxSpeed /** Mathf.Pow(1 - Mathf.Abs(motor.GetSteerValue()),2)*/;
    }
}
