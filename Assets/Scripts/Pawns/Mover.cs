using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : MonoBehaviour
{
    public Pawn pawn;
    // Start is called before the first frame update
    public virtual void Start()
    {
        pawn = GetComponent<Pawn>();
    }

    // Update is called once per frame
    public abstract void Update();
    public abstract void Move(Vector3 direction, float speed);
    public abstract void Rotate(float speed);
    public abstract void RotateToLookAt(Vector3 targetPoint, float speed);
}
