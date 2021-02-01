using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterBrainDataHolder
{
    private static Dictionary<CharacterAIType, System.Type> characterAIDictionary;
    public static Dictionary<CharacterAIType, System.Type> CharacterAIDictionary
    {
        get
        {
            if(characterAIDictionary == null)
            {
                characterAIDictionary = new Dictionary<CharacterAIType, System.Type>();
                characterAIDictionary[CharacterAIType.Petrol] = typeof(AIPetrolBrain);
                characterAIDictionary[CharacterAIType.Runner] = typeof(AIRunnerBrain);
            }

            return characterAIDictionary;
        }
    }


    public static System.Type GetBehevior(this CharacterAIType characterAIType)
    {
        return CharacterAIDictionary[characterAIType];
    }

}
