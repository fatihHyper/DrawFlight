using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccesfulEnterControl : MonoBehaviour
{

    private ObstacleController obstacleControllers;

    private bool isTriggered;


    private void Start()
    {
        obstacleControllers = transform.parent.GetComponentInChildren<ObstacleController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered)
            return;
        isTriggered = true;

        obstacleControllers.PassObstacle();
        HapticManager.Haptic(HapticTypes.HeavyImpact);
        
    }
}
