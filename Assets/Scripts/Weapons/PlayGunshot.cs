using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class PlayGunshot : MonoBehaviour
{
    public AudioClip gunShotSound;

    public void PlayGunshotSound()
    {
        if (gunShotSound != null)
        {
            AudioManager.Instance.Play(gunShotSound);
        }
    }

}
