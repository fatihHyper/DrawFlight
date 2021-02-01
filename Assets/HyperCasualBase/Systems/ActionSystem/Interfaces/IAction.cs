using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public interface IAction : IComponent
{
    bool isComplate { get; set; }

    void Begin();
    void Do();
    void Complete();
}
