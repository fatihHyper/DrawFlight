using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class CompleteStageTrigger : MonoBehaviour
{

    public bool isSuccess;

    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if(character)
        {
            if(character.CharacterControlType == CharacterControlType.Player)
                GameManager.Instance.CompilateStage(isSuccess);
        }
    }
}
