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
}

public enum PlayerState
{
    Idling,
    Moving,
    Dashing
}
