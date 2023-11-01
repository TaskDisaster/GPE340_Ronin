using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    public Light flashLight;

    // Start is called before the first frame update
    void Start()
    {
        // Make sure the light is initially turned off.
        flashLight.enabled = false;
    }

    // Call this method to trigger the muzzle flash.
    public void TriggerMuzzleFlash(float duration)
    {
        StartCoroutine(Flash(duration));
    }

    // Coroutine to control the flash duration.
    private IEnumerator Flash(float duration)
    {
        // Turn on the light.
        flashLight.enabled = true;

        // Wait for the specified duration.
        yield return new WaitForSeconds(duration);

        // Turn off the light.
        flashLight.enabled = false;
    }
}
