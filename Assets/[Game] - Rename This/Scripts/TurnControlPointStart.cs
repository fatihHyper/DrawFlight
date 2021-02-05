using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnControlPointStart : MonoBehaviour
{

    private static FlightController flightController;
    private static FlightController FlightController { get { return (flightController == null) ? flightController = FindObjectOfType<FlightController>() : flightController; } set { flightController = value; } }


    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<SplineFollower>().follow = true;
        
    }
}
