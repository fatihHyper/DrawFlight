using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public abstract class EffectControllerBase : MonoBehaviour
{
    private ParticleSystem particleSystem;
    public ParticleSystem ParticleSystem { get { return (particleSystem == null) ? particleSystem = GetComponent<ParticleSystem>() : particleSystem; } }



    protected virtual void PlayParticleEffect()
    {
        ParticleSystem.Play();
    }
}
