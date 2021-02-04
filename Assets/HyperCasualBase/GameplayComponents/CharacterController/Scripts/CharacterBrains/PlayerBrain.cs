using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrain : CharacterBrainBase
{
    private float speed;
    private float rotationSpeed;

    private Rigidbody _planeRigid;
    private void Start()
    {
        rotationSpeed = 10f;
        speed = 1000f;
        gameObject.transform.tag = "Player";
        _planeRigid =  gameObject.AddComponent<Rigidbody>();
    }
   
    public override void Logic()
    {
        transform.Rotate(-0.5f * rotationSpeed * Time.deltaTime, 0, 0);

    }
    private void FixedUpdate()
    {
        _planeRigid.AddForce(transform.forward * speed * Time.deltaTime);

    }
#if UNITY_EDITOR
    private void Update()
    {

        if (Input.GetButtonDown("Jump"))
            CharacterController.Jump();
    }
#endif
}
