using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterController : IComponent
{
    MonoBehaviour MonoBehaviour { get; }

    void Initialize();
    float CurrentSpeed();
    void Move(Vector3 Direction);
    void Jump();
    bool IsGrounded();
    void Dispose();
}
