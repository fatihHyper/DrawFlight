using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GasParticleHolder : MonoBehaviour
{
    private List<ParticleSystem> gasParticles;
    public List<ParticleSystem> GasParticles { get { return (gasParticles == null) ? gasParticles = GetComponentsInChildren<ParticleSystem>().ToList() : gasParticles; } }
}
