using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{

    // signals
    public Action<int> OnDamageTaken;    // int: damageTaken
    public Action<bool> OnInvulnerableChange; // bool: isImmune?
    public event Action<Cell> OnDeath;

    public int health;
    [Tooltip("Time just after a hit where you cant get hit again")]
    public float invulnerableTime = 0.1f;

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
        MakeInvulnerable();

        Invoke(nameof(MakeVulnerable), invulnerableTime);
        OnDamageTaken?.Invoke(damage); // signal for all subscribers

        IsDead = health <= 0;
        if (IsDead)
        {
            OnDeath?.Invoke(this);
            Die();
        }

        return IsDead;
    }

    protected virtual void MakeInvulnerable() {
        IsInvulnerable = true;
        OnInvulnerableChange?.Invoke(true);
    }

    protected virtual void MakeVulnerable()
    {
        IsInvulnerable = false;
        OnInvulnerableChange?.Invoke(false);
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    
}
