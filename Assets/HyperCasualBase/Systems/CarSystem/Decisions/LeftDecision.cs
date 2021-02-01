using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftDecision : Decision
{
    public override bool Decide(CarBrainBase brainBase)
    {
        var motor = brainBase.Motor;
        float steerValue = motor.GetSteerValue();
        int totalHitCount = motor.CarSensorController.GetTotalHitCount();
        return (totalHitCount == 0 && steerValue < 0);
    }
}
