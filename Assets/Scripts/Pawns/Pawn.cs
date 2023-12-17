using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    [Header("Pawn Variables")]
    #region Variables
    public Controller controller;
    public Mover mover;
    public Weapon shooter;
    public WeaponManager weaponManager;
    public Health healthComp;
    public PlayerUIManager uiManager;
    public float maxMoveSpeed;
    public float maxRotationSpeed;
    public bool isSprinting;
    [Range(0,1)]
    public float weaponAccuracyPercent;
    #endregion

    // Start is called before the first frame update
    protected virtual void Start()
    {
        mover = gameObject.GetComponent<Mover>();
        weaponManager = gameObject.GetComponent<WeaponManager>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    // Movement
    public abstract void Move(Vector3 direction);

    // Rotation
    public abstract void Rotate(float speed);

    public abstract void RotateToLookAt(Vector3 targetPoint);
}
