using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class RigidbodyCharacterController : CharacterControllerBase
{
    Rigidbody rigidbody;
    Rigidbody Rigidbody
    {
        get
        {
            if (rigidbody == null)
            {
                rigidbody = GetComponent<Rigidbody>();
                if (rigidbody == null)
                    rigidbody = gameObject.AddComponent<Rigidbody>();
            }

            return rigidbody;
        }
    }
    CapsuleCollider capsuleCollider;
    CapsuleCollider CapsuleCollider
    {
        get
        {
            if(capsuleCollider == null)
            {
                capsuleCollider = GetComponent<CapsuleCollider>();
                if (capsuleCollider == null)
                    capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            }

            return capsuleCollider;
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        Rigidbody.angularDrag = 2f;
        CapsuleCollider.height = 2f;
        CapsuleCollider.center = Vector3.up;
        CapsuleCollider.material = Character.CharacterData.CharacterMovementData.PhysicMaterial;
    }

    public override void Move(Vector3 Direction)
    {
        if (!Character.IsControlable)
            return;

        RotateCharacter(Direction);

        if (Character.CharacterControlType == CharacterControlType.Player)
            Direction = Camera.main.transform.TransformDirection(Direction);

        Direction.Normalize();
        Vector3 movement = new Vector3(Direction.x, 0, Direction.z) * Character.MoveSpeed * 100 * Time.deltaTime;
        Rigidbody.velocity = new Vector3(movement.x, Rigidbody.velocity.y, movement.z);
    }


    public override bool IsGrounded()
    {
        Ray ray = new Ray(transform.position + (Vector3.up * CapsuleCollider.height), Vector3.down);
        RaycastHit hit;
        Debug.DrawRay(transform.position + (Vector3.up * CapsuleCollider.height), Vector3.down, Color.red);
        return Physics.SphereCast(ray, CapsuleCollider.height / 2, out hit, CapsuleCollider.bounds.extents.y + 0.3f, Character.GroundLayer);
    }

    public override void Jump()
    {
        if (!IsGrounded())
            return;

        Rigidbody.AddForce(Vector3.up * Character.JumpHeight);
    }

    public override void Dispose()
    {
        base.Dispose();
        Utilities.DestroyExtended(rigidbody);
        Utilities.DestroyExtended(capsuleCollider);
        rigidbody = null;
        capsuleCollider = null;
    }

    public override float CurrentSpeed()
    {
        return Rigidbody.velocity.magnitude;
    }


}
