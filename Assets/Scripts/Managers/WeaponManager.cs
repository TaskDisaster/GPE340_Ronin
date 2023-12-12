using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class WeaponManager : MonoBehaviour
{
    [Header("Variables")]
    public Weapon defaultWeapon;
    public bool randomStart;
    public Weapon[] randomWeapon;
    public Transform attachmentPoint;
    public Weapon equippedWeapon;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Pawn pawn;

    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        animator = gameObject.GetComponent<Animator>();
        pawn = GetComponent<Pawn>();

        // If randomStart is true
        if (randomStart)
        {
            // Equip a random weapon
            EquipWeapon(GetRandomWeapon());
        }
        else
        {
            // Equip the Default weapon
            EquipWeapon(defaultWeapon);
        }
    }

    /// <summary>
    /// Gets a random weapon from a list of weapons
    /// </summary>
    /// <returns></returns>
    public Weapon GetRandomWeapon()
    {
        int random = Random.Range(0, randomWeapon.Length);
        return randomWeapon[random];
    }

    /// <summary>
    /// Equips a weapon
    /// </summary>
    /// <param name="weapon"></param>
    public void EquipWeapon(Weapon weapon)
    {
        // Create the weapon
        equippedWeapon = Instantiate(weapon) as Weapon;
        // Set it to the player's layer
        equippedWeapon.gameObject.layer = gameObject.layer;
        // Make it a child of player
        equippedWeapon.transform.SetParent(attachmentPoint);
        // Set weapon position
        equippedWeapon.transform.position = attachmentPoint.transform.position;
        // Set weapon rotation
        equippedWeapon.transform.rotation = attachmentPoint.transform.rotation;
        // Set it to the player's shooter
        pawn.shooter = equippedWeapon;
    }

    /// <summary>
    /// Unequips a weapon, if there is one
    /// </summary>
    public void Unequip()
    {
        // If they have a weapon
        if (equippedWeapon)
        {
            // Destroy it
            Destroy(equippedWeapon.gameObject);
            // Set weapon to null
            equippedWeapon = null;
        }
    }

    public void OnAnimatorIK(int layerIndex)
    {
        if (pawn.shooter != null)
        {
            if (pawn.shooter.RHPoint != null)
            {
                animator.SetIKPosition(AvatarIKGoal.RightHand, pawn.shooter.RHPoint.position);
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
                animator.SetIKRotation(AvatarIKGoal.RightHand, pawn.shooter.RHPoint.rotation);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
            }

            if (pawn.shooter.LHPoint != null)
            {
                animator.SetIKPosition(AvatarIKGoal.LeftHand, pawn.shooter.LHPoint.position);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, pawn.shooter.LHPoint.rotation);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
            }
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.0f);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.0f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0.0f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0.0f);
        }
    }
}
