using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : Controller
{
    [HideInInspector] public NavMeshAgent agent;
    public float stoppingDistance;
    public Transform targetTransform;
    private Vector3 desiredVelocity = Vector3.zero;

    // Start is called before the first frame update
    protected override void Start()
    {
        pawn.isSprinting = true;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void PossessPawn(Pawn pawnToPosses)
    {
        base.PossessPawn(pawnToPosses);

        // Get the agent off the pawn
        agent = pawn.GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            agent = pawn.gameObject.AddComponent<NavMeshAgent>();
        }

        // set the stopping distance
        agent.stoppingDistance = stoppingDistance;

        // Set the max sped of the AI from the pawn data
        agent.speed = pawn.maxMoveSpeed;

        // Set the max rotation speed of the AI from pawn data
        agent.angularSpeed = pawn.maxRotationSpeed;

        // Disable movement and rotation from the NavMeshAgent
        agent.updatePosition = false;
        agent.updateRotation = false;

        // Make sure the rigidbody does not change our position, only use root motion and the agent
        pawn.GetComponent<Rigidbody>().isKinematic = true;
    }

    protected override void UnpossessPawn()
    {
        // Remove the NavMeshAgent
        Destroy(agent);

        // Set the rigidbody back to not Kinematic
        pawn.GetComponent<Rigidbody>().isKinematic = false;

        base.UnpossessPawn();
    }

    protected override void MakeDecisions()
    {
        // Set our NaveMeshAgent to seek our target
        agent.SetDestination(targetTransform.position);

        // Find the velocity that the agent wants to move in order to follow the path
        desiredVelocity = agent.desiredVelocity;

        // Send this in to our Move function
        pawn.Move(desiredVelocity);

        // Look towards the player
        pawn.RotateToLookAt(targetTransform.position);
    }
}
