using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pickup))]
public class DestroySelf : MonoBehaviour
{
    private Pickup pickup;
    public float timeTillDestroy = 0;

    // Start is called before the first frame update
    void Start()
    {
        pickup = GetComponent<Pickup>();

        if (pickup != null)
        {
            pickup.OnPickup.AddListener(DestroyDeath);
        }
    }

    public void DestroyDeath()
    {
        Destroy(gameObject);
    }
}
