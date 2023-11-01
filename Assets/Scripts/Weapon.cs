using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    public float damage;        // How much damage the weapon does

    public UnityEvent OnTriggerRelease;
    public UnityEvent OnTriggerPull;
    public UnityEvent OnFire;
}
