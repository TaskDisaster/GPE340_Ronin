using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : Mover
{

    Animator animator;
    private float maxMoveSpeed;
    private float maxRotationSpeed;
    private bool isSprinting;

    // Start is called before the first frame update
    public override void Start()
    {
        // Get animator attached to this object
        animator = GetComponent<Animator>();

        // Make the animator apply root motion
        animator.applyRootMotion = true;
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {

    }

    public override void Move(Vector3 direction, float speed)
    {
        // Clamp the direction to 1 unit to make movement uniform
        direction = Vector3.ClampMagnitude(direction, 1);

        // Take the direction we were told to move and multiply by speed
        direction = direction * speed;

        // Send the values from the direction in to the animator
        animator.SetFloat("Forward", direction.z);
        animator.SetFloat("Right", direction.x);
    }

    public override void Rotate(float speed)
    {
        // Use the Rotate function to rotate based on speed
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }

    public override void RotateToLookAt(Vector3 targetPoint, float speed)
    {
        // Finde the vector from our position to the target point
        Vector3 lookVector = targetPoint - transform.position;

        // Find the rotation that will look down that vector with world up being the up direction
        Quaternion lookRotation = Quaternion.LookRotation(lookVector, Vector3.up);

        // Rotate slightly towards that target rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, speed * Time.deltaTime);
    }

    public void OnAnimatorMove()
    {
        // After the animation runs
        transform.position += animator.deltaPosition;
        transform.rotation *= animator.deltaRotation;

    }
}
