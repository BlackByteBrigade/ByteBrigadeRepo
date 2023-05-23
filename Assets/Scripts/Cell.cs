using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int health;
    public event Action<Cell> OnDeath;

    /// <summary>
    /// Cell takes specified damage
    /// </summary>
    /// <param name="damage"></param>
    /// <returns>Whether or not the enemy died</returns>
    public virtual bool TakeDamage(int damage)
    {
        health = Mathf.Max(health - damage, 0); // so that health never goes below zero (for player healthbar to not go below zero)

        bool isDead = health <= 0;
        if (isDead)
        {
            Die();
        }

        return isDead;
    }

    public virtual void Die()
    {
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }
}
