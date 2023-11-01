using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    public Controller controller;
    public Mover mover;
    public ProjectileWeapon shooter;
    public Health healthComp;
    public float maxMoveSpeed;
    public float maxRotationSpeed;
    public bool isSprinting;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        mover = GetComponent<Mover>();
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
