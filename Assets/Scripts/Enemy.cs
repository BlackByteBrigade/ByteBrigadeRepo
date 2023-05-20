using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Enemy : Cell
{
    public GameObject Player { get; set; }
    public Collider Weakspot { get; set; }
    public float DistanceToPlayer { get; set; }
    public float ReactsToPlayerDistance { get; set; }
    public float AlarmedByPlayerDistance { get; set; }
    public Alertness AlertnessLevel { get; set; }
    public float CountdownTillRestAlertness { get; set; }
    public float CountdownTillRestAlertnessRemaining { get; set; }

    private Random _rand;
    private int _numberofAlertStates;
    private DateTime _lastAlertRise { get; set; }

    private bool Has3SecondsPassedSinceLastAlertRise => (_lastAlertRise != default(DateTime) && (DateTime.Now - _lastAlertRise).TotalSeconds > 3);

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("player");
        _rand = new Random(DateTime.Now.Millisecond);
        _numberofAlertStates = Enum.GetNames(typeof(Alertness)).Length;
    }

    // Update is called once per frame
    void Update()
    {
        DistanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        HandleAlertness();
    }

    public void HandleAlertness()
    {
        if (AlarmedByPlayerDistance <= DistanceToPlayer)
        {
            AlertnessLevel = Alertness.Engaged;
            _lastAlertRise = DateTime.Now;
        }
        else if (ReactsToPlayerDistance <= DistanceToPlayer && Has3SecondsPassedSinceLastAlertRise && _rand.Next(0, 10) % 4 == 0)
        {
            CountdownTillRestAlertnessRemaining = CountdownTillRestAlertness;
            _lastAlertRise = DateTime.Now;
            if ((int)AlertnessLevel < _numberofAlertStates)
            {
                AlertnessLevel++;
            }
        }
        else if (AlertnessLevel > 0 && CountdownTillRestAlertnessRemaining <= 0)
        {
            AlertnessLevel--;
        }

        if (CountdownTillRestAlertnessRemaining > 0)
        {
            CountdownTillRestAlertnessRemaining -= Time.deltaTime;
        }
    }

    public enum Alertness
    {
        Idle = 0,
        Noticed,
        Engaged
    }
}