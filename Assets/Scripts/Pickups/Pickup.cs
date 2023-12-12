using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{

    [Header("Unity Events")]
    public UnityEvent OnPickup;

    public void OnTriggerEnter(Collider other)
    {
        // Check if the thing colliding with it is a Pawn
        if (other.gameObject.GetComponent<Pawn>() != null)
        {
            OnPickup.Invoke();
        }
    }
}
