using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;
using UnityEngine.AI;

public class AIMover : Mover
{
    //This will move via root motion, so it needs a reference to the animator.
    private Animator animator;
    //It uses NavMesh to move around, so it needs a reference to the agent.
    private NavMeshAgent agent;

    [SerializeField]
    [Tooltip("The distance away from the target this pawn will start slowing down.")]
    private float slowingDistance = 3;

    [SerializeField]
    [Tooltip("The distance away fromt the target this pawn will stop.")]
    private float stoppingDistance = 1.5f;

    [SerializeField]
    [Tooltip("How many times per second the NavMeshAgent will recalculate.")]
    private float navMeshFrames = 15;

    private float updateClock = 0;
    private Vector3 smoothMovement = Vector3.zero;

    public override void Start()
    {
        base.Start();

        //Get or create the agent.
        agent = pawn.GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            agent = pawn.gameObject.AddComponent<NavMeshAgent>();
        }

        //Set defaults on the agent
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.stoppingDistance = 0.5f; //We will not use the Agent's stopping distance.
        agent.acceleration = 15; //Apply a higher acceleration for better pathfinding. The root motion handles speed and acceleration itself.

        //Get the animator component
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Pawn must have an animator component attached.");
        }
    }

    public override void Update()
    {

    }

    private void OnDestroy()
    {
        //When this movement script is removed, get rid of the NavMeshAgent.
        if (agent != null)
        {
            Destroy(agent);
        }
    }

    public override void Move(Vector3 target, float speed)
    {
        //Only redo the path sometimes (every 1/15 of a second default)
        if (updateClock <= 0)
        {
            agent.SetDestination(target);
            updateClock = 1 / navMeshFrames;
        }
        else
        {
            updateClock -= Time.deltaTime;
        }

        //Get the delta vector from where the agent wants to be from pawn.
        Vector3 worldPosition = agent.nextPosition - transform.position;
        //Set the y to zero since the animator is 2D.
        worldPosition.y = 0;
        //Make the worldPosition relative to the pawn.
        Vector3 localMovement = transform.InverseTransformDirection(worldPosition);

        //Apply speed to the root motion.
        localMovement.Normalize();
        localMovement *= speed;

        //Slow the movement down when approching the destination.
        if (agent.remainingDistance <= slowingDistance)
        {
            localMovement *= agent.remainingDistance / slowingDistance;
        }

        //Check to see if the pawn should stop
        if (agent.remainingDistance <= stoppingDistance)
        {
            localMovement = Vector3.zero;
        }

        //Smooth the movement between frames.
        float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
        smoothMovement = Vector3.Lerp(smoothMovement, localMovement, smooth);

        //Make sure it won't get infinitely small.
        if (smoothMovement.magnitude < 0.1)
        {
            smoothMovement = Vector3.zero;
        }

        //Apply the movement to the animator
        animator.SetFloat("Right", smoothMovement.x);
        animator.SetFloat("Forward", smoothMovement.z);
    }

    public override void Rotate(float speed)
    {
        // Use the Rotate function to rotate based on speed
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }


    public override void RotateToLookAt(Vector3 target, float speed)
    {
        //Find the vector amd rotation from the pawn to the target.
        Vector3 lookVector = target - pawn.transform.position;

        //Turn the vector into a quaternion
        Quaternion lookRotation = Quaternion.LookRotation(lookVector, Vector3.up);

        //Smooth the rotation
        pawn.transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, speed * Time.deltaTime);
    }

    private void OnAnimatorMove()
    {
        //Get the root motion and manually apply it.
        Vector3 rootPosition = animator.rootPosition;
        rootPosition.y = agent.nextPosition.y; //We will be using the agent's verticality since all of the movement animations are 2D.
        transform.position = rootPosition;

        //Syncronizes the NavMesh with the actual position of the animator.
        //This needs to happen due to a race condition between the script update thread and the NavMesh thread.
        //OnAnimatorMove is always called after both, so it resolves that issue.
        agent.nextPosition = rootPosition;
    }
}
