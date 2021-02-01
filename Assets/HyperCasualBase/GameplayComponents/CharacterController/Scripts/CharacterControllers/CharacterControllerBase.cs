using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public abstract class CharacterControllerBase : InterfaceBase, ICharacterController
{
    public MonoBehaviour MonoBehaviour { get { return this; } }

    private Character character;
    public Character Character { get { return (character == null) ? character = GetComponent<Character>() : character; } }


    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;
    }

    public virtual void Initialize()
    {
        Debug.Log("Character Controller Type Intialized " + this.GetType());
    }
    public abstract void Move(Vector3 Direction);
    public abstract void Jump();
    public abstract bool IsGrounded();

    protected virtual void RotateCharacter(Vector3 targetDirection)
    {
        if (targetDirection == Vector3.zero)
        {
            return;
        }
        targetDirection.Normalize();

        float distanceToTargetDir = Vector3.Distance(transform.TransformPoint(Vector3.forward), targetDirection);
        if (Character.CharacterControlType == CharacterControlType.Player)
            targetDirection = Camera.main.transform.TransformDirection(targetDirection);

        targetDirection.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), (Character.TurnSpeed * Mathf.Clamp(distanceToTargetDir, 0f, 2f)) * Time.deltaTime);

    }

    public abstract float CurrentSpeed();

    public virtual void Dispose()
    {
        //Debug.Log("Character Controller Type Disposed " + this.GetType());
        Utilities.DestroyExtended(this);
    }
}
