using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrain : CharacterBrainBase
{


    private void Start()
    {
        gameObject.transform.tag = "Player";
    }
    public override void Logic()
    {
        CharacterController.Move(new Vector3(InputManager.Instance.Joystick.Direction.x, 0f, InputManager.Instance.Joystick.Direction.y));
    }

#if UNITY_EDITOR
    private void Update()
    {

        if (Input.GetButtonDown("Jump"))
            CharacterController.Jump();
    }
#endif
}
