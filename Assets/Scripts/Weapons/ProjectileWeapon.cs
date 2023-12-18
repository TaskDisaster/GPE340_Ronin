using NUnit.Framework;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public bool triggerPulled;          // If we're shooting
    public Transform barrel;            // Where to shoot from
    public float fireCooldown;          // How fast can we shoot
    private float lastTimeShot;          // The last time shot
    public bool autoFire;
    public int bulletsPerShot;
    public Projectile bulletPrefab;     // Projectile prefab
    public float muzzleVelocity;        // Projectile speed

    // Start is called before the first frame update
    protected void Start()
    {
        owner = GetComponentInParent<Pawn>();
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

    protected void Shoot()
    {
        if (triggerPulled)
        {
            while (Time.time > lastTimeShot + fireCooldown)
            {
                for (int i = 0; i < bulletsPerShot; i++)
                {
                    // Calculate random spread
                    float randomRotation;
                    if (UnityEngine.Random.value <= 0.5f)
                    {
                        randomRotation = GetAccuracyRotation();
                    }
                    else
                    {
                        randomRotation = -GetAccuracyRotation();
                    }

                    // Add pawn's accuracy modifier
                    float pawnAccuracy;
                    pawnAccuracy = 1 - owner.weaponAccuracyPercent;
                    randomRotation *= pawnAccuracy;

                    // Random Spread
                    Quaternion randomSpread = Quaternion.Euler(0, randomRotation, 0);

                    // Create bullet
                    Projectile projectile = Instantiate(bulletPrefab, barrel.position, barrel.rotation * randomSpread) as Projectile;

                    // Set damage 
                    projectile.damage = damage;

                    // Set owner
                    projectile.owner = this.GetComponentInParent<Pawn>();

                    //Add a force to it
                    projectile.rb.AddRelativeForce(Vector3.forward * muzzleVelocity, ForceMode.VelocityChange);

                    // Set time shot
                    lastTimeShot = Time.time;

                    // Only do the effects once
                    if (i < 1)
                    {
                        // Do effects upon shooting
                        OnFire.Invoke();
                    }

                    if (!autoFire)
                    {
                        triggerPulled = false;
                    }
                }
            }
        }
    }

}
