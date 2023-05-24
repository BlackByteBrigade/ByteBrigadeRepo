using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int health;
    [Tooltip("Time just after a hit where you cant get hit again")]
    public float invulnerableTime = 0.1f;
    public event Action<Cell> OnDeath;

    public bool IsInvulnerable { get; private set; }
    public bool IsDead { get; private set; }

    /// <summary>
    /// Cell takes specified damage
    /// </summary>
    /// <param name="damage"></param>
    /// <returns>Whether or not the enemy died</returns>
    public virtual bool TakeDamage(int damage)
    {
        if (IsInvulnerable || IsDead) return IsDead; // dont want to go forward if we have already died/are currently invulnerable

        health = Mathf.Max(health - damage, 0); // so that health never goes below zero (for player healthbar to not go below zero)
        IsInvulnerable = true;
        Invoke(nameof(MakeVulnerable), invulnerableTime);

        IsDead = health <= 0;
        if (IsDead)
        {
            OnDeath?.Invoke(this);
            Die();
        }

        return IsDead;
    }

    private void MakeVulnerable()
    {
        IsInvulnerable = false;
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
