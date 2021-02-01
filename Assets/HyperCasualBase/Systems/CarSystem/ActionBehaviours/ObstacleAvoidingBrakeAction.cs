
using UnityEngine;

public class ObstacleAvoidingBrakeAction : ActionBehaviour
{
    private float forceAmount = 200.0f;

    public override void Act(CarBrainBase carBrainBase)
    {
        var motor = carBrainBase.Motor;
        motor.SetMotorTorque(0);
        motor.SetBrakeTorque(10);
        motor.Rigidbody.AddForce(-carBrainBase.transform.forward * Time.fixedDeltaTime * forceAmount);
    }
}
