using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Variables")]
    #region Variables
    public Pawn pawn;
    public float currentHealth;
    public float maxHealth;
    #endregion

    [Header("Events")]
    #region Events
    [SerializeField, Tooltip("Raised every time the object is healed.")]
    public UnityEvent OnHeal;
    [SerializeField, Tooltip("Raised every time the object is damaged.")]
    public UnityEvent OnTakeDamage;
    [SerializeField, Tooltip("Raised once when the object's health reaches 0.")]
    public UnityEvent OnDeath;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        pawn = GetComponent<Pawn>();
        currentHealth = maxHealth;

        if (pawn.uiManager != null)
        {
            pawn.uiManager.healthBar.fillAmount = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float amount)
    {
        // Minus damage from health
        currentHealth = currentHealth - amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Change the health bar to match
        if (pawn.uiManager != null)
        {
            pawn.uiManager.healthBar.fillAmount = currentHealth / maxHealth;
        }
        
        // Tell all the subscribers that this object took damage
        OnTakeDamage.Invoke();

        // Set the currentHealth
        if (currentHealth <= 0)
        {
            SendMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
            // Tell all the subscribers that this object is dead
            OnDeath.Invoke();
        }
    }

    public void Heal(float amount)
    {
        // Add the healing to health
        currentHealth = currentHealth + amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Change the health bar to match
        if (pawn.uiManager != null)
        {
            pawn.uiManager.healthBar.fillAmount = currentHealth / maxHealth;
        }

        OnHeal.Invoke();
    }
}
