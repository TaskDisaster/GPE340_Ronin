using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
    [Header("Unity Events")]
    public UnityEvent OnPickup;

    [Header("Variables")]
    public float timeTillDestroyed;
    public AudioClip pickupSound;

    public virtual void Start()
    {
        // Only destroy if it has a timer
        if (timeTillDestroyed > 0)
        {
            Destroy(gameObject, timeTillDestroyed);
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        PerfomPickup(other);
    }

    public virtual void PerfomPickup(Collider other)
    {
        // Check if the thing colliding with it is a Pawn
        if (other.gameObject.GetComponent<Pawn>() != null)
        {
            OnPickup.Invoke();
        }

        if (pickupSound != null)
        {
            AudioManager.Instance.Play(pickupSound);
        }
    }
}
