using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pickup))]
public class OnBegin : MonoBehaviour
{
    private Pickup pickup;

    private void Start()
    {
        pickup = GetComponent<Pickup>();

        if (pickup != null )
        {
            pickup.OnPickup.AddListener(BeginGame);
        }
    }

    public void BeginGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.beginWaves = true;
        }
    }
}
