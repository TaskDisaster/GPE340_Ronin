using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage { get; set; }
    public Rigidbody rb;
    public Pawn owner;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Attack
    public void OnTriggerEnter(Collider other)
    {
        // Get health of the collided object
        Health otherHealth = other.gameObject.GetComponent<Health>();
        Vector3 otherPosition = other.gameObject.transform.position;

        // If it has health, then damage it
        if (otherHealth != null)
        {
            otherHealth.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
