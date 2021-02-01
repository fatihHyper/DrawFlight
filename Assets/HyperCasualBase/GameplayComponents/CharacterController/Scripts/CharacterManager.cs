using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;

public class CharacterManager : Singleton<CharacterManager>
{
    [HideInInspector]
    public UnityEvent OnCharactersLoaded = new UnityEvent();

    [HideInInspector]
    public CharacterEvent OnCharacterAdded = new CharacterEvent();

    [HideInInspector]
    public CharacterEvent OnCharacterRemoved = new CharacterEvent();

    [HideInInspector]
    public PlacementEvent OnPlacementChange = new PlacementEvent();
    [ReadOnly]
    public List<Character> Characters = new List<Character>();

    private Character player;
    public Character Player
    {
        get
        {
            if (player == null)
            {
                for (int i = 0; i < Characters.Count; i++)
                {
                    if (Characters[i].CharacterControlType == CharacterControlType.Player)
                    {
                        player = Characters[i];
                        return Characters[i];
                    }
                }
                Debug.Log("There isn't a player character");
                return null;
            }
            else return player;
        }
        set { player = value; }
    }

    public Character GetRandomAI
    {
        get
        {
            return Characters[Random.Range(0, Characters.Count)];
        }
    }


    public void AddCharacter(Character character)
    {
        if (!Characters.Contains(character))
        {
            Characters.Add(character);
            OnCharacterAdded.Invoke(character);
        }
    }

    public void RemoveCharacter(Character character)
    {
        if (Characters.Contains(character))
        {
            Characters.Remove(character);
            OnCharacterRemoved.Invoke(character);
        }
        else
        {
            Debug.LogError("This Character is not tracked by the manager returning in to the pool to prevent possible bugs.");
        }

        //CheckGameCharacterState(character);
    }

    private void CheckGameCharacterState(Character character)
    {
        if (character.CharacterControlType == CharacterControlType.None)
            return;

        if (character.CharacterControlType == CharacterControlType.Player)
        {
            if (Characters.Count >= 1)
            {
                OnPlacementChange.Invoke(Characters.Count + 1);
                GameManager.Instance.CompilateStage(false);
                AnalitycsManager.Instance.LogEvent("Level_Event", "Level_Fail", Characters.Count.ToString());
                Debug.Log("Player lose the geme");
            }
        }
        else
        {
            if (Characters.Count <= 1)
            {
                GameManager.Instance.CompilateStage(true);
                Debug.Log("Player won the geme");
            }
        }
    }

    public Character CreateCharacter(CharacterData characterData, Vector3 position)
    {
        Character character = PoolingSystem.Instance.InstantiateAPS("Character", position, Quaternion.identity).GetComponent<Character>();
        character.InitilizeCharacter(characterData);
        return character;
    }

    [Button]
    private void KillAllAI()
    {
        List<Character> characters = new List<Character>(Characters);
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].CharacterControlType == CharacterControlType.Player)
                continue;

            characters[i].KillCharacter();
        }
    }

    [Button]
    private void KillPlayer()
    {
        for (int i = 0; i < Characters.Count; i++)
        {
            if (Characters[i].CharacterControlType == CharacterControlType.Player)
                Characters[i].KillCharacter();

        }
    }
}


public class CharacterEvent : UnityEvent<Character> { }
public class PlacementEvent : UnityEvent<int> { }
