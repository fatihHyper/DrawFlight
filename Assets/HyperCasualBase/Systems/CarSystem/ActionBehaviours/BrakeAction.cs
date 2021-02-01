using UnityEngine;

public class BrakeAction : ActionBehaviour
{
    private float forceAmount = 100.0f;
    public override void Act(CarBrainBase carBrainBase)
    {
        var motor = carBrainBase.Motor;
        float steerValue = motor.GetSteerValue();
        float absSteerValue = Mathf.Abs(steerValue);
        motor.SetMotorTorque(0);
        motor.SetBrakeTorque(absSteerValue);
        motor.Rigidbody.AddForce(-carBrainBase.transform.forward * Time.deltaTime * 100);
    }
}
