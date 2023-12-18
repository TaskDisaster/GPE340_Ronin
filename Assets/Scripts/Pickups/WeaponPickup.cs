using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Pickup
{
    public Weapon weapon;
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


        if (pawn != null)
        {
            if (pawn.weaponManager != null)
            {
                pawn.weaponManager.Unequip();
                pawn.weaponManager.EquipWeapon(weapon);
            }
        }

        if (pickupSound != null)
        {
            AudioManager.Instance.Play(pickupSound);
        }

        OnPickup.Invoke();
    }
}
