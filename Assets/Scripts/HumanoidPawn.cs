using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidPawn : Pawn
{
    private Animator animator;

    // Start is called before the first frame update
    protected override void Start()
    {
        // Get animator attached to this object
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {

    }

    public override void Move(Vector3 direction)
    {
        // Clamp the direction to 1 unit to make movement uniform
        direction = Vector3.ClampMagnitude(direction, 1);

        // Make a speed float for easy change
        float speed;

        if (isSprinting)
        {
            speed = maxMoveSpeed;
        }
        else
        {
            speed = maxMoveSpeed / 2;
        }

        // Take the direction we were told to move and multiply by speed
        direction = direction * speed;

        // Send the values from the direction in to the animator
        animator.SetFloat("Forward", direction.z);
        animator.SetFloat("Right", direction.x);
    }

    public override void Rotate(float speed)
    {
        // Use the Rotate function to rotate based on speed
        transform.Rotate(0, speed * maxRotationSpeed * Time.deltaTime, 0);
    }

    public override void RotateToLookAt(Vector3 targetPoint)
    {
        // Finde the vector from our position to the target point
        Vector3 lookVector = targetPoint - transform.position;

        // Find the rotation that will look down that vector with world up being the up direction
        Quaternion lookRotation = Quaternion.LookRotation(lookVector, Vector3.up);

        // Rotate slightly towards that target rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, maxRotationSpeed * Time.deltaTime);
    }

    public void OnAnimatorMove()
    {
        // After the animation runs

        // Use root motion to move the game object
        transform.position += animator.deltaPosition;
        transform.rotation *= animator.deltaRotation;

        // If we haave a NavMeshAgent on our controller
        AIController aiController = controller as AIController;
        if (aiController != null)
        {
            // Move to the position of the NavMeshAgent, too
            transform.position = aiController.agent.nextPosition;
        }
    }
}
