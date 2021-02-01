using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class CharacterData : ScriptableObject
{
    public CharacterControlType CharacterControlType = CharacterControlType.None;
    public BodyType BodyType = BodyType.CharacterController; 

    [ShowIf("isAI")]
    public CharacterAIType CharacterAIType = CharacterAIType.Petrol;


    public CharacterHeathData CharacterHealthData = new CharacterHeathData();
    public CharacterMovementData CharacterMovementData = new CharacterMovementData();


    private bool isAI { get { return CharacterControlType == CharacterControlType.AI; } }
}

[System.Serializable]
public class CharacterHeathData
{
    public CharacterHeathData()
    {
        MaxHealth = 10;
        InitialDamage = 1;
    }
    public int MaxHealth;
    public int InitialDamage;
}

[System.Serializable]
public class CharacterMovementData
{
    public CharacterMovementData()
    {
        MovementForce = 20;
        MaxTrunSpeed = 3;
        JumpHeight = 15;
    }
    public float MaxSpeed;
    public float MovementForce;
    public float MaxTrunSpeed;
    public float JumpHeight;

    public PhysicMaterial PhysicMaterial;
}
