using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathParticles : MonoBehaviour
{
    private Pawn pawn;

    public void Start()
    {
        pawn = gameObject.GetComponent<Pawn>();    
    }

    public void DeathParticle()
    {
        Debug.Log(pawn + " has died.");
        Debug.Log("THE PARTICLE EFFECTS ARE SUPPOSED TO PLAY RIGHT NOW");
    }
}
