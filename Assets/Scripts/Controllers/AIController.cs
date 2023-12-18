using UnityEngine;
using UnityEngine.AI;

public class AIController : Controller
{
    #region AI Data
    [Header("AI Data")]
    /// <summary>
    /// The distance of how close the AI gets to the player
    /// </summary>
    [Tooltip("The distance the AI must stop at")]
    public float stoppingDistance;

    /// <summary>
    /// The transform of the target
    /// </summary>
    [Tooltip("The transform of the target")]
    public Transform targetTransform;

    /// <summary>
    /// Reference to the NaveMesh agent
    /// </summary>
    [HideInInspector] 
    public NavMeshAgent agent;

    /// <summary>
    /// The desired velocity that the pawn moves at
    /// </summary>
    [Tooltip("The desired velocity that the pawn moves at")]
    private Vector3 desiredVelocity = Vector3.zero;
    #endregion

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
        // Stop everything if pawn is killed
        if (pawn.healthComp.currentHealth <= 0)
        {
            pawn.shooter.OnTriggerRelease.Invoke();
            GameManager.Instance.currentEnemyPawns.Remove(pawn);
            pawn.weaponManager.Unequip();
            UnpossessPawn();
            Destroy(gameObject);
            return;
        }

        if (targetTransform == null)
        {
            targetTransform = gameObject.transform;
            return;
        }

        // Set our NavMeshAgent to seek our target
        agent.SetDestination(targetTransform.position);

        // Find the velocity that the agent wants to move in order to follow the path
        desiredVelocity = agent.desiredVelocity;

        // Send this in to our Move function
        pawn.Move(desiredVelocity);

        // Look towards the player
        pawn.RotateToLookAt(targetTransform.position);

        // Get the distance from pawn to target
        float distance = CalculateDistance(pawn.transform, targetTransform);

        // If less than or equal to stoppingDistance, reign fire
        if (distance <= stoppingDistance)
        {
            pawn.shooter.OnTriggerPull.Invoke();
        }
        else
        {
            pawn.shooter.OnTriggerRelease.Invoke();
        }
    }

    protected float CalculateDistance(Transform self, Transform target)
    {
        float distance = Vector3.Distance(self.position, target.position);
        return distance;
    }
}
