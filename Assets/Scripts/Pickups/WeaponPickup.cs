using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Pickup
{
    public Weapon weapon;

    public override void PerfomPickup(Collider other)
    {
        Pawn pawn = other.gameObject.GetComponent<Pawn>();

        if (pawn != null)
        {
            if (pawn.weaponManager != null)
            {
                pawn.weaponManager.Unequip();
                pawn.weaponManager.EquipWeapon(weapon);
            }
        }
        base.PerfomPickup(other);
    }
}
