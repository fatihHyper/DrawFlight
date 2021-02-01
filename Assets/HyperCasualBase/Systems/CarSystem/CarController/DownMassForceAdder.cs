using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownMassForceAdder : MonoBehaviour
{
    private Motor motor;
    public Motor Motor { get { return (motor == null) ? motor = GetComponent<Motor>() : motor; } }

    public Vector3 centerOfMass;

    public float downForce = 100.0f;

    private void Start()
    {
        Motor.Rigidbody.centerOfMass = centerOfMass;
    }
    private void FixedUpdate()
    {
        Motor.Rigidbody.AddForce(-transform.up * downForce * Motor.Rigidbody.velocity.magnitude);
    }
    private void OnDrawGizmos()
    {
        if (Motor == null) return;


        Gizmos.color = Color.red;
        Gizmos.DrawSphere(centerOfMass + Motor.Rigidbody.worldCenterOfMass, 0.04f);
    }

}
