using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterController : CharacterControllerBase
{
    private UnityEngine.CharacterController controller;
    public UnityEngine.CharacterController Controller
    {
        get
        {
            if (controller == null)
            {
                controller = GetComponent<UnityEngine.CharacterController>();
                if (controller == null)
                    controller = gameObject.AddComponent<UnityEngine.CharacterController>();
            }
            return controller;
        }
    }
    private float gravityFactor = -0.5f;
    private Vector3 movementVector;
    public override void Initialize()
    {
        base.Initialize();
        Controller.center = Vector3.up;
        Controller.height = 2f;
        Controller.material = Character.CharacterData.CharacterMovementData.PhysicMaterial;
    }
    public override void Move(Vector3 direction)
    {
        Gravity();
        if (!Character.IsControlable || !IsGrounded())
        {
            RotateCharacter(direction);
            Controller.Move(movementVector);
            return;
        }

        RotateCharacter(direction);
        if (Character.CharacterControlType == CharacterControlType.Player)
            direction = Camera.main.transform.TransformDirection(direction);
        direction.y = 0;
        direction.Normalize();
        Vector3 movement = new Vector3(direction.x, 0, direction.z) * Character.MoveSpeed * Time.deltaTime;
        movementVector = new Vector3(movement.x, movementVector.y, movement.z);
        Controller.Move(movementVector);
    }
    private void Gravity()
    {
        if (IsGrounded() && movementVector.y < 0)
        {
            movementVector.y = 0f;
        }
        movementVector.y += gravityFactor * Time.deltaTime;
    }
    public override void Jump()
    {
        movementVector.y += Mathf.Sqrt(Character.CharacterData.CharacterMovementData.JumpHeight * -2f * gravityFactor * Time.deltaTime * 10);
    }
    public override bool IsGrounded()
    {
        return Controller.isGrounded;
    }
    public override void Dispose()
    {
        Utilities.DestroyExtended(controller);
        controller = null;
        base.Dispose();
    }
    public override float CurrentSpeed()
    {
        return Controller.velocity.magnitude;
    }
}














