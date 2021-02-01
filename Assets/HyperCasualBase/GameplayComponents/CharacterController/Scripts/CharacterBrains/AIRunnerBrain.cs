using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class AIRunnerBrain : CharacterBrainBase
{

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

    public override void Logic()
    {
        //throw new System.NotImplementedException();
    }
}
