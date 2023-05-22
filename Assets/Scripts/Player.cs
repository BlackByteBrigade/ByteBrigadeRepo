using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Player : Cell
{
    public static Player instance;

    public PlayerMovement Movement { get; set; }
    public PlayerState State { get; set; }

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
    }
}

public enum PlayerState
{
    Idling,
    Moving,
    Dashing
}
