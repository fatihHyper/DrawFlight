using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputController : MonoBehaviour
{

    System.Type interfaceType;

    private ICarBrain carBrain;
    public ICarBrain CarBrain { get { return Utilities.IsNullOrDestroyed(carBrain, out interfaceType) ? carBrain = GetComponent<ICarBrain>() : carBrain; } }

    void FixedUpdate()
    {
        if (CarBrain == null)
            return;

        CarBrain.Logic();
    }
}
