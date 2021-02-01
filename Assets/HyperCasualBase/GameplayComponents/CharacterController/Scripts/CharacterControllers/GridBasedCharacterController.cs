using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBasedCharacterController : CharacterControllerBase
{
    private Vector3 playerVelocity;

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

    public override void Initialize()
    {
        base.Initialize();
        Controller.center = Vector3.up;
        Controller.height = 2f;
    }

    public override void Move(Vector3 direction)
    {
        Vector3 offset = direction - transform.position;
        offset = offset.normalized;
        offset.y = 0f;
        RotateCharacter(offset);
        Controller.Move(offset * Character.MoveSpeed * Time.deltaTime);


        //if (!Character.IsControlable)
        //{
        //    Gravity();
        //    Controller.Move(Vector3.down);
        //    return;
        //}

        //RotateCharacter(direction);
        //direction.y = 0;

        /* Controller.*/
        //    transform.position = Vector3.MoveTowards(transform.position, direction, Character.MoveSpeed * Time.deltaTime);
        Gravity();
    }

    protected override void RotateCharacter(Vector3 targetDirection)
    {
        if (targetDirection == Vector3.zero)
        {
            return;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), Character.TurnSpeed * Time.deltaTime);
    }

    private void Gravity()
    {
        //var groundNormal = Character.CharacterController.;
        //Debug.Log(groundNormal);

        if (!IsGrounded())
        {
            Controller.Move((Controller.velocity + Physics.gravity) * Time.deltaTime);
        }
        //else playerVelocity.y = 0;
    }



    public override void Jump()
    {
        playerVelocity.y = Mathf.Sqrt(Character.CharacterData.CharacterMovementData.JumpHeight * -2f * Physics.gravity.y * Time.deltaTime * 10);
        Controller.Move(playerVelocity);
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


    //---------------------------

    //Rigidbody rigidbody;
    //Rigidbody Rigidbody
    //{
    //    get
    //    {
    //        if (rigidbody == null)
    //        {
    //            rigidbody = GetComponent<Rigidbody>();
    //            if (rigidbody == null)
    //                rigidbody = gameObject.AddComponent<Rigidbody>();
    //        }

    //        return rigidbody;
    //    }
    //}
    //CapsuleCollider capsuleCollider;
    //CapsuleCollider CapsuleCollider
    //{
    //    get
    //    {
    //        if (capsuleCollider == null)
    //        {
    //            capsuleCollider = GetComponent<CapsuleCollider>();
    //            if (capsuleCollider == null)
    //                capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
    //        }

    //        return capsuleCollider;
    //    }
    //}

    //public override void Initialize()
    //{
    //    base.Initialize();
    //    Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    //    Rigidbody.angularDrag = 2f;
    //    CapsuleCollider.height = 2f;
    //    CapsuleCollider.center = Vector3.up;
    //}

    //public override void Move(Vector3 Direction)
    //{
    //    if (!Character.IsControlable)
    //        return;

    //    RotateCharacter(Direction);

    //    transform.position = Vector3.MoveTowards(transform.position, Direction, Character.MoveSpeed * Time.deltaTime);
    //}

    //protected override void RotateCharacter(Vector3 targetDirection)
    //{
    //    if (targetDirection == Vector3.zero)
    //    {
    //        return;
    //    }

    //    targetDirection.y = 0;
    //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection - transform.position), Character.TurnSpeed * Time.deltaTime);
    //}

    //public override bool IsGrounded()
    //{
    //    Ray ray = new Ray(transform.position + (Vector3.up * CapsuleCollider.height), Vector3.down);
    //    RaycastHit hit;
    //    Debug.DrawRay(transform.position + (Vector3.up * CapsuleCollider.height), Vector3.down, Color.red);
    //    return Physics.SphereCast(ray, CapsuleCollider.height / 2, out hit, CapsuleCollider.bounds.extents.y + 0.3f, Character.GroundLayer);
    //}

    //public override void Jump()
    //{
    //    if (!IsGrounded())
    //        return;

    //    Rigidbody.AddForce(Vector3.up * Character.JumpHeight);
    //}

    //public override void Dispose()
    //{
    //    base.Dispose();
    //    Utilities.DestroyExtended(rigidbody);
    //    Utilities.DestroyExtended(capsuleCollider);
    //    rigidbody = null;
    //    capsuleCollider = null;
    //}

    //public override float CurrentSpeed()
    //{
    //    return Rigidbody.velocity.magnitude;
    //}

}
