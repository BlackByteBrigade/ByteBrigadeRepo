using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Player : Cell
{
    public static Player instance = null;

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
            dnaUpgrade.RemoveUpgrade(this);
            Destroy(dnaUpgrade);
        }

        dnaUpgrade = Instantiate(newDNA, transform);
        dnaUpgrade.ApplyUpgrade(this);
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
        RaiseOnDeathEvent();

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
