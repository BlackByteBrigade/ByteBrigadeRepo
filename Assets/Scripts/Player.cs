using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(PlayerMovement))]
public class Player : Cell
{

    public Action<bool> OnDashReady; // bool: isDashReady?
    public static Player instance = null;

    [Tooltip("Amount of damage from dash")]
    public int dashDamage;

    public int HealPerPlasmaCoin = 20;
    private bool isHealing;
    public PlayerMovement Movement { get; set; }
    public PlayerState State { get; set; }
    public Softbody Softbody { get; set; }
    public DNAUpgrade dnaUpgrade { get; set; }

    public CircleCollider2D mainCollider;

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
        Movement = GetComponent<PlayerMovement>();
        Softbody = GetComponent<Softbody>();
    }

    private void Start()
    {
        health = PlayerManager.Instance.currentHealth;
        PlayerHUD.instance.UpdateHealthbar(health / (float)PlayerManager.Instance.maxHealth);

        if (PlayerManager.Instance.currentDNAUpgrade != null)
        {
            ChangeDNA(PlayerManager.Instance.currentDNAUpgrade);
        }
        else
        {
            dnaUpgrade = null;
        }
    }

    public void ChangeDNA(DNAUpgrade newDNA)
    {
        if (dnaUpgrade != null)
        {
            if (newDNA.GetType() == dnaUpgrade.GetType()) return;
            dnaUpgrade.RemoveUpgrade(this);
            Destroy(dnaUpgrade.gameObject);
        }

        PlayerManager.Instance.currentDNAUpgrade = newDNA;
        dnaUpgrade = Instantiate(newDNA, transform);
        dnaUpgrade.ApplyUpgrade(this);
    }

    public void Attack(Enemy enemy)
    {
        enemy.TakeDamage(dashDamage);
        Movement.CancelDash(enemy);
        base.TakeDamage(0);
    }

    // On Damage Effects
    public override bool TakeDamage(int damage) {
        
        bool wasInvulnerable = IsInvulnerable;
        bool isDead = base.TakeDamage(damage);
        PlayerManager.Instance.currentHealth = health;
        if (!isDead && !wasInvulnerable) {
            CameraShake.Shake();
            AudioManager.instance.PlaySfX(SoundEffects.PlayerTakingDamage);
            PlayerHUD.instance.UpdateHealthbar(health/ (float)PlayerManager.Instance.maxHealth);
        } else if (isDead)
            AudioManager.instance.PlaySfX(SoundEffects.PlayerDeath);

        return isDead;
    }

    public override void Die()
    {
        Teleporter.currentDestinationId = -1;
        PlayerManager.Instance.currentDNAUpgrade = null;
        gameObject.SetActive(false);

        // TODO add partcies

        PlayerManager.Instance.RespawnPlayer();
    }

    public IEnumerator StartHeal()
    {
        isHealing = true;
        while (isHealing)
        {
            if (GameManager.Instance.storedplasmaCoins > 0 && health < PlayerManager.Instance.maxHealth)
            {
                GameManager.Instance.storedplasmaCoins--;
                health = Math.Min(health+HealPerPlasmaCoin, PlayerManager.Instance.maxHealth);
                PlayerHUD.instance.UpdateHealthbar(health / (float)PlayerManager.Instance.maxHealth);
                yield return new WaitForSeconds(1);
            }
            else
            {
                if(GameManager.Instance.storedplasmaCoins <= 0 && health!= 100)
                {
                    PlayerHUD.instance.isShaking = true;
                }
                isHealing = false;
            }
        }
    }

    public void EndHeal()
    {
        isHealing = false;
    }
}

public enum PlayerState
{
    Idling,
    Moving,
    Dashing
}
