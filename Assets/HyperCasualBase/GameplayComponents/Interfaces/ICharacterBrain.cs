using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterBrain : IComponent
{
    MonoBehaviour MonoBehaviour { get; }

    ICharacterController CharacterController { get; }

    void Initialize();
    void Logic();
    void Dispose();

}
