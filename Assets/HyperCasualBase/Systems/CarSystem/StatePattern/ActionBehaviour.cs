using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionBehaviour : ScriptableObject
{
    public abstract void Act(CarBrainBase carBrainBase); 
}
