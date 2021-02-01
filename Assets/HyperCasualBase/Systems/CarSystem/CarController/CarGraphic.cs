using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGraphic : MonoBehaviour
{
    private CarController carController;
    public CarController CarController { get { return (carController == null)? carController = GetComponentInParent<CarController>() : carController; } }

    private void OnEnable()
    {
        if (Managers.Instance == null) return;
        CarController.OnCarInitialize.AddListener(CreateGraphics);
    }

    private void OnDisable()
    {
        if (Managers.Instance == null) return;
        CarController.OnCarInitialize.RemoveListener(CreateGraphics);
    }

    public void CreateGraphics(CarData carData)
    {
        var carGraphicData = carData.CarGraphicData;
        var carBodyController = Instantiate(carGraphicData.BodyPrefab, transform).GetComponent<CarBodyController>();

        var frontWheelTransforms = carBodyController.FrontWheelHolder.WheelTransforms;

        CarController.OnCarGraphicSet.Invoke(carBodyController.GasParticleHolder, carBodyController.FrontWheelHolder, carBodyController.RearWheelHolders);

        for (int i = 0; i < frontWheelTransforms.Count; i++)
        {
            var wheelTf = frontWheelTransforms[i];
            GenerateWheelGraphics(carGraphicData.FrontWheelPrefab, wheelTf);
            //TODO: SET WHEEL PROPS
        }

        for (int i = 0; i < carBodyController.RearWheelHolders.Count; i++)
        {
            var wheelTransforms = carBodyController.RearWheelHolders[i].WheelTransforms;

            for (int j = 0; j < wheelTransforms.Count; j++)
            {
                var wheelTf = wheelTransforms[j];
                GenerateWheelGraphics(carGraphicData.RearWheelPrefab, wheelTf);
                //TODO: SET WHEEL PROPS
            }
        }

    }

    public void GenerateWheelGraphics(GameObject wheelPrefab,Transform wheelTf)
    {
        //wheelTf.gameObject.AddComponent<WheelCollider>();
        var wheel = wheelTf.gameObject.AddComponent<Wheel>();
        wheel.SetWheelColliderProps(CarController.CarData.WheelColliderData);

        //TODO: SET WHEEL PROPS
        var wheelModel = Instantiate(wheelPrefab, wheelTf.position, wheelTf.rotation, wheelTf);
        wheel.Model = wheelModel.transform;
    }

}
