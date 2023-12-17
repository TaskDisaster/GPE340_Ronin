using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{
    public float healAmount;
    private FactionManager factionManager;
    public Faction factionUser;

    public override void Start()
    {
        factionManager = FindAnyObjectByType<FactionManager>();

        base.Start();
    }

    public override void PerfomPickup(Collider other)
    {
        Pawn pawn = other.gameObject.GetComponent<Pawn>();

        // Check if the pawn isn't null
        if (pawn == null)
        {
            return;
        }

        // Get the pawn's faction
        Faction otherFaction = factionManager.GetFaction(pawn);

        // Check if the factions are the same
        if (otherFaction != factionUser)
        {
            return;
        }

        // If the pawn's health component is there, heal
        if (pawn.healthComp != null)
        {
            pawn.healthComp.Heal(healAmount);
        }

        base.PerfomPickup(other);
    }
}
