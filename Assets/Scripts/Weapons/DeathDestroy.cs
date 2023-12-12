using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathDestroy : MonoBehaviour
{
    public Health healthComp;
    public float timeBeforeDestroy;

    // Start is called before the first frame update
    void Start()
    {
        healthComp = GetComponent<Health>();

        if (healthComp != null)
        {
            healthComp.OnDeath.AddListener(DestroyDeath);
        }
    }

    // Destroy self upon death
    public void DestroyDeath()
    {
        Destroy(gameObject, timeBeforeDestroy);
    }
}
