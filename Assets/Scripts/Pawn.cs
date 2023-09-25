using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    public Controller controller;
    public float maxMoveSpeed;
    public float maxRotationSpeed;
    public float jumpForce;
    public bool isSprinting;

    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    // Movement
    public abstract void Move(Vector3 direction);

    // Rotation
    public abstract void Rotate(float speed);

    public abstract void RotateToLookAt(Vector3 targetPoint);
}
