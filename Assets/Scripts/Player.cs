using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(PlayerMovement))]
public class Player : Cell
{
    public static Player instance = null;

    [Tooltip("Amount of damage from dash")]
    public int dashDamage;

    public PlayerMovement Movement { get; set; }
    public PlayerState State { get; set; }
    public DNAUpgrade dnaUpgrade { get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            InitializeComponents();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeComponents()
    {
        dnaUpgrade = null;
        Movement = GetComponent<PlayerMovement>();
    }

    public void ChangeDNA(DNAUpgrade newDNA)
    {
        if (dnaUpgrade != null)
        {
            if (newDNA.GetType() == dnaUpgrade.GetType()) return;
            dnaUpgrade.RemoveUpgrade(this);
            Destroy(dnaUpgrade);
        }

        dnaUpgrade = Instantiate(newDNA, transform);
        dnaUpgrade.ApplyUpgrade(this);
    }

    public void Attack(Enemy enemy)
    {
        enemy.TakeDamage(dashDamage);
        Movement.CancelDash();
        base.TakeDamage(0);
    }

    // On Damage Effects
    public override bool TakeDamage(int damage) {
        bool isDead = base.TakeDamage(damage);
        if (!isDead)
            AudioManager.instance.PlaySfX(SoundEffects.PlayerTakingDamage);
        else
            AudioManager.instance.PlaySfX(SoundEffects.PlayerDeath);

        return isDead;
    }

    public override void Die()
    {
        gameObject.SetActive(false);

        // TODO add partcies + dropping held enemy parts

        GameManager.instance.RespawnPlayer();
    }
}

public enum PlayerState
{
    Idling,
    Moving,
    Dashing
}
