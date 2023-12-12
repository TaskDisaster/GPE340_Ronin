using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Faction { Friendly, Hostile }

public class FactionManager : MonoBehaviour
{
    public Faction GetFaction(GameObject gameObject)
    {
        FactionComp factionComp = gameObject.GetComponent<FactionComp>();

        if (factionComp != null)
        {
            return factionComp.faction;
        }

        return Faction.Hostile; // Default to hostile if no faction component is found.
    }

    public Faction GetFaction(Pawn pawn)
    {
        FactionComp factionComp = pawn.GetComponent<FactionComp>();

        if (factionComp != null)
        {
            return factionComp.faction;
        }

        return Faction.Hostile;
    }
}
