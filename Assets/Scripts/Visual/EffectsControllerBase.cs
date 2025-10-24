using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectsControllerBase : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particleSystemEffect;

    public virtual void ClearAndStop()
    {
        foreach (var particle in particleSystemEffect)
        {
            particle.Stop();
            particle.Clear();
        }
    }
    public virtual void Stop()
    {
        foreach (var particle in particleSystemEffect)
        {
            particle.Stop();
        }
    }
    public virtual void Play()
    {
        foreach (var particle in particleSystemEffect)
        {
            particle.Play();
        }
    }
}
