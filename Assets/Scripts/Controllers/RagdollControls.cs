using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RagdollControls : MonoBehaviour
{
    public bool isRagdoll;
    private Rigidbody mainRigidbody;
    private Collider mainCollider;
    private Rigidbody[] rigidbodies;
    private Collider[] colliders;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        mainCollider = GetComponent<Collider>();
        mainRigidbody = GetComponent<Rigidbody>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        anim = GetComponent<Animator>();

        if (isRagdoll)
        {
            EnableRagdoll();
        }
        else
        {
            DisableRagdoll();
        }
    }

    public void EnableRagdoll()
    {
        // Go through all the colliders, activate them
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
        // Go through all the rigidbodies, activate them
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;
        }

        // Disable main rigidbody
        mainRigidbody.isKinematic = true;
        // Disable main collider
        mainCollider.enabled = false;

        // Disable the animator
        anim.enabled = false;

        isRagdoll = true;
    }

    public void DisableRagdoll()
    {
        // Go through all the colliders, deactivate them
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
        // Go through all the rigidbodies, deactivate them
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = true;
        }

        // Enable main rigidbody
        mainRigidbody.isKinematic = false;
        // Enable main collider
        mainCollider.enabled = true;

        // Enable animator
        anim.enabled = true;

        isRagdoll = false;
    }
}
