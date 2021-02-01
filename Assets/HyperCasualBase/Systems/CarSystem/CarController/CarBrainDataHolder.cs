using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CarBrainDataHolder 
{
    private static Dictionary<CarAIType, System.Type> carAIDictionary;
    public static Dictionary<CarAIType, System.Type> CarAIDictionary
    {
        get
        {
            if (carAIDictionary == null)
            {
                carAIDictionary = new Dictionary<CarAIType, System.Type>();
                carAIDictionary[CarAIType.FollowWaypoint] = typeof(FollowAICarBrain);
            }

            return carAIDictionary;
        }
    }


    public static System.Type GetBehaviour(this CarAIType carAIType)
    {
        return CarAIDictionary[carAIType];
    }

}
