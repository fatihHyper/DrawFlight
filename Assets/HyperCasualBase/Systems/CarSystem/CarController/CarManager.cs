using UnityEngine;

public class CarManager : Singleton<CarManager>
{
    [SerializeField]
    private State carAIStartState;

    public State CarAIStartState { get => carAIStartState;  }

    public CarController CreateCar(CarData carData, Vector3 position,Quaternion rotation)
    {
        CarController carController = PoolingSystem.Instance.InstantiateAPS("Car", position, rotation).GetComponent<CarController>();
        carController.InitializeCar(carData);
        return carController;
    }
}
