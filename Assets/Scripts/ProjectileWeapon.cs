using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public bool triggerPulled;          // If we're shooting
    public Transform barrel;            // Where to shoot from
    public float fireCooldown;          // How fast can we shoot
    private float lastTimeShot;          // The last time shot
    public bool autoFire;
    public Projectile bulletPrefab;     // Projectile prefab
    public float muzzleVelocity;        // Projectile speed

    // Start is called before the first frame update
    protected void Start()
    {

    }

    // Update is called once per frame
    protected void Update()
    {
        Shoot();
    }

    public void PullTrigger()
    {
        triggerPulled = true;
    }

    public void ReleaseTrigger()
    {
        triggerPulled = false;
    }

    protected virtual void Shoot()
    {
        if (triggerPulled)
        {
            while (Time.time > lastTimeShot + fireCooldown)
            {
                // Create bullet
                Projectile projectile = Instantiate(bulletPrefab, barrel.position, barrel.rotation) as Projectile;

                // Set damage 
                projectile.damage = damage;

                // Set owner
                projectile.owner = this.GetComponentInParent<Pawn>();

                //Add a force to it
                projectile.rb.AddRelativeForce(Vector3.forward * muzzleVelocity, ForceMode.VelocityChange);

                // Set time shot
                lastTimeShot = Time.time;

                // Do effects upon shooting
                OnFire.Invoke();
                
                if (!autoFire)
                {
                    triggerPulled = false;
                }
            }
        }
    }

}
