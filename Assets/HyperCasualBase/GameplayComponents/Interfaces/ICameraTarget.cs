using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICameraTarget : IComponent
{
    Transform transform { get; }
    void SubToCamera();
    void UnSubToCamera();
}
