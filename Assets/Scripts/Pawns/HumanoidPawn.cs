using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HumanoidPawn : Pawn
{

    // Start is called before the first frame update
    protected override void Start()
    {

    }

    protected override void Update()
    {

    }

    public override void Move(Vector3 direction)
    {
        if(mover != null)
        {
            // Convert our world space to local space
            direction = transform.InverseTransformDirection(direction);

            mover.Move(direction, maxMoveSpeed);
        }
    }

    public override void Rotate(float speed)
    {
        float maxSpeed = speed * maxRotationSpeed;
        mover.Rotate(maxSpeed);
    }

    public override void RotateToLookAt(Vector3 targetPoint)
    {
        mover.RotateToLookAt(targetPoint, maxRotationSpeed);
    }
}
