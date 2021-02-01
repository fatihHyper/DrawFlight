using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarBrain : CarBrainBase, ICarBrain
{
    System.Type interfaceType;

    public override void Logic()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 dir = new Vector3(horizontal, 0, vertical);
        bool isBraking = Input.GetKey(KeyCode.Space);

        float brakeValue = isBraking ? 1.0f : 0.0f;
        float motorTorque = isBraking ? 0.0f : vertical;
        Motor.SetBrakeTorque(brakeValue);
        Motor.SetMotorTorque(motorTorque);
        Motor.Steer(dir.x);

        Motor.AnimateWheels();
    }

    public override void TransitionToState(State state)
    {
    }
}
