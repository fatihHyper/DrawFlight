using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public enum CharacterControlType { None, Player, AI }
public enum CharacterAIType { Petrol, Runner }
public enum BodyType { CharacterController, Rigidbody}


public class Character : InterfaceBase, IPoolable
{
    [HideInInspector]
    public UnityEvent OnCharacterRevive = new UnityEvent();
    [HideInInspector]
    public UnityEvent OnCharacterDie = new UnityEvent();
    [HideInInspector]
    public UnityEvent OnCharacterHit = new UnityEvent();
    [HideInInspector]
    public HitEvent OnCharacterReciveDamage = new HitEvent();
    [HideInInspector]
    public UnityEvent OnCharacterJump = new UnityEvent();
    [HideInInspector]
    public UnityEvent OnCharacterSet = new UnityEvent();
    [HideInInspector]
    public SkinEvent OnSkinChange = new SkinEvent();
    [HideInInspector]
    public InitilizeEvent OnCharacterInitilize = new InitilizeEvent();

    private CharacterControlType characterControlType;
    [ShowInInspector]
    public CharacterControlType CharacterControlType
    {
        get
        {
            return characterControlType;
        }

        set
        {
            characterControlType = value;
        }
    }


    private CharacterAIType characterAIType;
    [ShowIf("isAI")]
    [ShowInInspector]
    public CharacterAIType CharacterAIType
    {
        get
        {
            return characterAIType;
        }

        set
        {
            characterAIType = value;
        }
    }


    private BodyType controllerPhysics;
    [ShowIf("isAI")]
    [ShowInInspector]
    public BodyType ControllerPhysics
    {
        get
        {
            return controllerPhysics;
        }

        set
        {
            controllerPhysics = value;
        }
    }

    private CharacterData characterData;
    public CharacterData CharacterData { get { return characterData; } set { characterData = value; } }

    System.Type interfaceType;
    private ICharacterController characterController;
    public ICharacterController CharacterController { get { return Utilities.IsNullOrDestroyed(characterController, out interfaceType) ? characterController = GetComponent<ICharacterController>() : characterController; } }

    public bool IsGrounded { get { return (CharacterController == null) ? false : CharacterController.IsGrounded(); } }
    public float CurrentSpeed { get { return (CharacterController == null) ? 0 : CharacterController.CurrentSpeed(); } }

    public LayerMask GroundLayer;

    private float moveSpeed;
    public float MoveSpeed { get { return (moveSpeed == 0)? CharacterData.CharacterMovementData.MovementForce: moveSpeed; } set { moveSpeed = value; } }
    private float turnSpeed;
    public float TurnSpeed { get { return (turnSpeed == 0)? CharacterData.CharacterMovementData.MaxTrunSpeed :turnSpeed; } set { turnSpeed = value; } }
    private float jumpHeight;
    public float JumpHeight { get { return (jumpHeight == 0)? CharacterData.CharacterMovementData.JumpHeight : jumpHeight; } set { jumpHeight = value; } }

    [ReadOnly]
    public bool isDead;

    private bool isAI { get { return CharacterControlType == CharacterControlType.AI; } }

    [ShowInInspector]
    [ReadOnly]
    private bool isControlable;
    public bool IsControlable { get { return isControlable; } set { isControlable = value; } }

    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;


        LevelManager.Instance.OnLevelStart.AddListener(() => {
            ReviveCharacter();
        });
        LevelManager.Instance.OnLevelFinish.AddListener(() => IsControlable = false);
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;


        LevelManager.Instance.OnLevelStart.RemoveListener(() => {
            ReviveCharacter();
        });
        LevelManager.Instance.OnLevelFinish.RemoveListener(() => IsControlable = false);
    }

    public void InitilizeCharacter(CharacterData characterData)
    {
        CharacterControlType = characterData.CharacterControlType;
        CharacterAIType = characterData.CharacterAIType;
        ControllerPhysics = characterData.BodyType;
        CharacterData = characterData;
        OnCharacterInitilize.Invoke(CharacterData);
        isDead = true;
        IsControlable = false;
        CharacterManager.Instance.AddCharacter(this);
        SetCharacter();
    }

    public void KillCharacter()
    {
        if (isDead)
            return;

        IsControlable = false;
        isDead = true;
        CharacterManager.Instance.RemoveCharacter(this);
        OnCharacterDie.Invoke();
    }

    [Button]
    public void ReviveCharacter()
    {
        if (!isDead)
            return;


        IsControlable = true;
        isDead = false;
        //Reset Character values here
        OnCharacterRevive.Invoke();
    }

    [Button]
    public void DamageCharacter(int damage)
    {
        OnCharacterReciveDamage.Invoke(damage);
    }

    private void SetCharacter()
    {
        StartCoroutine(SetCharacterCo());
    }

    IEnumerator SetCharacterCo()
    {
        if (CharacterControlType == CharacterControlType.None)
        {
            OnCharacterSet.Invoke();
            yield break;
        }

        CameraTarget cameraTarget = GetComponent<CameraTarget>();
        List<ICharacterBrain> characterBrains = new List<ICharacterBrain>(GetComponentsInChildren<ICharacterBrain>());

        //Clear Character Brains
        foreach (var item in characterBrains)
        {
            item.Dispose();
        }

        yield return new WaitForEndOfFrame();

        List<ICharacterController> characterControllers = new List<ICharacterController>(GetComponentsInChildren<ICharacterController>());

        //Clear Character Controllers
        foreach (var item in characterControllers)
        {
            item.Dispose();
        }

        yield return new WaitForEndOfFrame();

        //Add character controller type
        var controllerType = ControllerPhysics.GetBehevior();
        var type = gameObject.AddComponent(controllerType);
        type.GetComponent<ICharacterController>().Initialize();

        yield return new WaitForEndOfFrame();

        switch (CharacterControlType)
        {
            case CharacterControlType.Player:
                //Add player bain
                var brain = gameObject.AddComponent<PlayerBrain>();
                brain.GetComponent<ICharacterBrain>().Initialize();

                //Add camera target to make this object trackable by camera
                if (!cameraTarget)
                    gameObject.AddComponent<CameraTarget>();
                break;

            case CharacterControlType.AI:
                //Remove camera target
                if (cameraTarget)
                    Utilities.DestroyExtended(cameraTarget);

                //Add AI brain based on the type in character data
                var aiType = CharacterAIType.GetBehevior();
                var aiBrain = gameObject.AddComponent(aiType);
                aiBrain.GetComponent<ICharacterBrain>().Initialize();
                break;

        }
        OnCharacterSet.Invoke();
    }

    public void Initilize()
    {
        ReviveCharacter();
    }

    public void Dispose()
    {
        KillCharacter();
    }
}

public class SkinEvent : UnityEvent<Skin> { }
public class InitilizeEvent : UnityEvent<CharacterData> { }
public class HitEvent : UnityEvent<int> { }
