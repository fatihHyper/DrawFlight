using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarController : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent OnCarSet = new UnityEvent();

    [HideInInspector]
    public CarInitializeEvent OnCarInitialize = new CarInitializeEvent();

    [HideInInspector]
    public CarGraphicSetEvent OnCarGraphicSet = new CarGraphicSetEvent();

    public CarData CarData { get; set; }

    private CarControlType carControlType;
    private CarAIType carAiType;

    public CarControlType CarControlType
    {
        get
        {
            return carControlType;
        }

        set
        {
            carControlType = value;
        }
    }

    [ShowInInspector]
    [ShowIf("isAI")]
    public CarAIType CarAiType
    {
        get
        {
            return carAiType;
        }

        set
        {
            carAiType = value;
        }
    }

    private bool isAI { get { return CarControlType == CarControlType.AI; } }



    private Collider bodyCollider;

    public Collider BodyCollider { get { return (bodyCollider == null) ? bodyCollider = GetComponent<Collider>() : bodyCollider; } }

    private void OnEnable()
    {
        if (Managers.Instance == null) return;
    }

    private void OnDisable()
    {
        if (Managers.Instance == null) return;
    }

    private void SetCar()
    {
        if (CarControlType == CarControlType.None)
        {
            OnCarSet.Invoke();
            return;
        }

        CameraTarget cameraTarget = GetComponent<CameraTarget>();
        List<ICarBrain> carBrains = new List<ICarBrain>(GetComponentsInChildren<ICarBrain>());

        foreach (var item in carBrains)
        {
            Utilities.DestroyExtended(item.MonoBehaviour);
        }

        switch (CarControlType)
        {
            case CarControlType.Player:
                gameObject.AddComponent<PlayerCarBrain>();
                if (!cameraTarget)
                    gameObject.AddComponent<CameraTarget>();
                break;

            case CarControlType.AI:
                if (cameraTarget)
                    Utilities.DestroyExtended(cameraTarget);

                var aiType = CarAiType.GetBehaviour();
                gameObject.AddComponent(aiType);
                break;

        }
        OnCarSet.Invoke();

    }

    public void InitializeCar(CarData carData)
    {
        CarData = carData;
        CarControlType = carData.CarControlData.CarControlType;
        CarAiType = carData.CarControlData.CarAIType;

        OnCarInitialize.Invoke(carData);
        SetCar();
    }

}

public class InitializeEvent<T> : UnityEvent<T> { }
public class CarInitializeEvent : InitializeEvent<CarData> { }
public class CarGraphicSetEvent : UnityEvent<GasParticleHolder,WheelHolder, List<WheelHolder>> { }

