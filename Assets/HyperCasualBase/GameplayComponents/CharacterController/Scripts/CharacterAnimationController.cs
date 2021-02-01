using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    Character character;
    Character Character { get { return (character == null) ? character = GetComponentInParent<Character>() : character; } }

    List<Animator> animators;
    List<Animator> Animators { get { return (animators == null || animators.Count == 0) ? animators = new List<Animator>(GetComponentsInChildren<Animator>()) : animators; } }


    private void OnEnable()
    {
        Character.OnCharacterJump.AddListener(OnJump);
        Character.OnCharacterDie.AddListener(OnDeath);
        Character.OnCharacterHit.AddListener(OnHit);
        Character.OnCharacterRevive.AddListener(() => {
            foreach (var Animator in Animators)
            {
                Animator.applyRootMotion = false;
            }
            transform.position = Character.transform.position;
            transform.rotation = Character.transform.rotation;
        });
    }

    private void OnDisable()
    {
        Character.OnCharacterJump.RemoveListener(OnJump);
        Character.OnCharacterDie.RemoveListener(OnDeath);
        Character.OnCharacterHit.RemoveListener(OnHit);
        Character.OnCharacterRevive.RemoveListener(() => {
            foreach (var Animator in Animators)
            {
                Animator.applyRootMotion = false;
            }
            transform.position = Character.transform.position;
            transform.rotation = Character.transform.rotation;
        });
    }

    private void Update()
    {
        UpdateAnimations();
    }

    private void UpdateAnimations()
    {
        foreach (var Animator in Animators)
        {
            Animator.SetFloat("Speed", Character.CurrentSpeed);
            Animator.SetBool("isGrounded", Character.IsGrounded);
            Animator.SetBool("isDead", Character.isDead);
        }
    }

    private void OnJump()
    {
        foreach (var Animator in Animators)
        {
            Animator.SetTrigger("Jump");
        }
    }

    private void OnDeath()
    {
        foreach (var Animator in Animators)
        {
            Animator.applyRootMotion = true;
            Animator.SetTrigger("Die");
        }
    }

    private void OnHit()
    {
        foreach (var Animator in Animators)
        {
            Animator.SetTrigger("Hit");
        }
    }
}
