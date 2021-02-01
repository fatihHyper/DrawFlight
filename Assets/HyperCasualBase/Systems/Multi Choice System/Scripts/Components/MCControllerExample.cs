using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCControllerExample : MCControllerBase
{
    public float ActionTime = 1.5f;

    public override void Begin()
    {
        CharacterManager.Instance.Player.IsControlable = false;
    }

    public override void Do()
    {
        Choice.Job();
        if (Choice.IsCorrect)
        {
            //Success
            Complete();
        }
        else
        {
            //Fail
        }
        CharacterManager.Instance.Player.IsControlable = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Character>() != null)
        {
            Trigger();
        }
    }
}
