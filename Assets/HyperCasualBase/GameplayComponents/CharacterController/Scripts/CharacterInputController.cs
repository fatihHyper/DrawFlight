using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputController : MonoBehaviour
{

    System.Type interfaceType;


    private ICharacterBrain characterBrain;
    public ICharacterBrain CharacterBrain { get { return Utilities.IsNullOrDestroyed(characterBrain, out interfaceType) ? characterBrain = GetComponent<ICharacterBrain>() : characterBrain; } }

    void FixedUpdate()
    {
        if (CharacterBrain == null)
            return;

        CharacterBrain.Logic();
    }

}
