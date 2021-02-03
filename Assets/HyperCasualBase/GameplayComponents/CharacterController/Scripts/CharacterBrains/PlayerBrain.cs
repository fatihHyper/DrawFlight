using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrain : CharacterBrainBase
{
    private Vector3 direction;

    private void Start()
    {
        gameObject.transform.tag = "Player";
        direction = Vector3.forward;
        gameObject.AddComponent<Rigidbody>();
    }
    public override void Logic()
    {

        gameObject.GetComponent<Rigidbody>().velocity = (Vector3.forward  *10f);
            
    }

#if UNITY_EDITOR
    private void Update()
    {

        if (Input.GetButtonDown("Jump"))
            CharacterController.Jump();
    }
#endif
}
