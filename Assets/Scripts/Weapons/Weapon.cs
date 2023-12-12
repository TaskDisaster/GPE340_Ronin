using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    [Header("Inverse Kinematics")]
    public Transform RHPoint;
    public Transform LHPoint;

    [Header("Variables")]
    public float damage;        // How much damage the weapon does
    public Pawn owner;

    [Header("Unity Events")]
    public UnityEvent OnTriggerRelease;
    public UnityEvent OnTriggerPull;
    public UnityEvent OnFire;

    [Header("Accuracy Data")]
    [Range(0, 100)]
    public float weaponAccuracy;
    public float maxWeaponAccuracy = 100;
    [Tooltip("This is the max rotation of a projectile from on-target if the weapon has 0 accuracy")]
    public float maxWeaponAccuracyRotationOffset;

    public float GetAccuracyRotation()
    {
        // Find what percent of accuracy our weapon has
        float percentOfAccuracy = weaponAccuracy / maxWeaponAccuracy;
        // Flip it, so 100% accuracy means zero rotation
        percentOfAccuracy = 1 - percentOfAccuracy;
        // Find the max amount we would rotate based on this accuracy
        float maxRotationOffset = maxWeaponAccuracyRotationOffset * percentOfAccuracy;
        // Choose a random rotation less than the max
        float accuracyRotationOffset = Random.Range(-maxRotationOffset, maxRotationOffset);
        // Convert to the Quaternion we would adjust our rotation of our projectile by
        return accuracyRotationOffset;
    }
}
