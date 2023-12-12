using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage { get; set; }
    public float bulletLifetime;
    public Rigidbody rb;
    public Pawn owner;
    private FactionManager factionManager;
    private Faction ownerFaction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        factionManager = FindFirstObjectByType<FactionManager>();
        ownerFaction = factionManager.GetFaction(owner);

        Destroy(gameObject, bulletLifetime);
    }

    // Attack
    public void OnTriggerEnter(Collider other)
    {
        // Check if factionManager is null
        if (factionManager == null)
        {
            Debug.LogError("FactionManager not found in scene.");
            return;
        }

        // Check the other faction
        Faction otherFaction = factionManager.GetFaction(other.gameObject);

        // Ignore faction
        if (otherFaction == ownerFaction)
        {
            return;
        }

        // Get health of the collided object
        Health otherHealth = other.gameObject.GetComponent<Health>();
        
        // If it has health, then damage it
        if (otherHealth != null)
        {
            otherHealth.TakeDamage(damage);
        }

        // If it's not another projectile, destroy self
        if (other.gameObject.GetComponent<Projectile>() == null)
        {
            Destroy(gameObject);
        }

    }
}
