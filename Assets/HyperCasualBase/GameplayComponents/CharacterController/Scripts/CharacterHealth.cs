using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class CharacterHealth : InterfaceBase, IDamagable
{
    Character character;
    Character Character { get { return (character == null) ? character = GetComponentInParent<Character>() : character; } }

    [Header("UI")]
    public Slider Slider;

    public int MaxHealth { get { return Character.CharacterData.CharacterHealthData.MaxHealth; } }
    public int InitialDamage { get { return Character.CharacterData.CharacterHealthData.InitialDamage; } }

    private int currentHealth;
    public int CurrentHealt
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
        }
    }

    private void Start()
    {
        Initilize();
    }

    private void OnEnable()
    {
        Character.OnCharacterRevive.AddListener(Initilize);
        Character.OnCharacterReciveDamage.AddListener(Damage);
    }

    private void OnDisable()
    {
        Character.OnCharacterRevive.RemoveListener(Initilize);
        Character.OnCharacterReciveDamage.RemoveListener(Damage);
    }

    private void Initilize()
    {
        CurrentHealt = MaxHealth;
        Slider.maxValue = MaxHealth;
        UpdateUI();
    }

    public void Damage(int damageAmount)
    {

        CurrentHealt -= damageAmount;
        if (CurrentHealt <= 0)
            Dispose();
        else Character.OnCharacterHit.Invoke();
        
        UpdateUI();
    }

    public void Dispose()
    {
        Character.KillCharacter();
    }

    private void UpdateUI()
    {
        Slider.value = CurrentHealt;
    }
}
