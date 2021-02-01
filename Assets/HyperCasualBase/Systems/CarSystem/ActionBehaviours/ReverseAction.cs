using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseAction : ActionBehaviour
{
    public override void Act(CarBrainBase carBrainBase)
    {
        Vector3 frontMidRayNormal = carBrainBase.Motor.CarSensorController.SensorResultDatas[Side.FRONT].totalAverageNormal;

        if (frontMidRayNormal.x < 0)
            carBrainBase.Motor.Steer(0.5f);
        else if (frontMidRayNormal.x > 0)
            carBrainBase.Motor.Steer(-0.5f);

        carBrainBase.Motor.SetBrakeTorque(0);
        carBrainBase.Motor.SetMotorTorque(-1);
    }
}
