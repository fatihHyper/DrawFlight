using UnityEngine;

public class HitNormalAvoidAction : ActionBehaviour
{
    public override void Act(CarBrainBase carBrainBase)
    {
        var motor = carBrainBase.Motor;
        //Vector3 averageNormal = carBrainBase.transform.TransformDirection(motor.CarSensorController.SensorResultDatas[Side.FRONT].totalAverageNormal);
        //Vector3 averageNormal = motor.CarSensorController.SensorResultDatas[Side.FRONT].totalAverageNormal;
        Vector3 averageNormalInv = carBrainBase.transform.InverseTransformDirection(motor.CarSensorController.SensorResultDatas[Side.FRONT].totalAverageNormal);

        if (averageNormalInv.x > 0)
            motor.Steer(averageNormalInv.x);
        else if(averageNormalInv.x < 0)
            motor.Steer(averageNormalInv.x);

        motor.SetBrakeTorque(0);
        //motor.SetMotorTorque(1);
    }
}
