using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEffects : MonoBehaviour
{
    public ParticleSystem gunEffect;

    public void PlayParticleEffects()
    {
        if (gunEffect != null)
        {
            gunEffect.Play();
        }
    }
}
