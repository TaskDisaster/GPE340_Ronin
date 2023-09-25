using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    public Pawn pawn;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // If we were given a pawn at the start, possess it
        if (pawn != null)
        {
            PossessPawn(pawn);
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // Make decisions
        MakeDecisions();
    }

    protected abstract void MakeDecisions();

    protected virtual void PossessPawn(Pawn pawnToPosses)
    {
        // Set our pawn variable to the pawn we want to poassess
        pawn = pawnToPosses;

        // Set the pawn's controller to this controller
        pawn.controller = this;
    }

    protected virtual void UnpossessPawn()
    {
        // Set our pawn's controller to null
        pawn.controller = null;

        // Set our pawn variable to null
        pawn = null;
    }
}
