using System;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class Enemy : Cell
{
    public GameObject Player { get; set; }
    public Collider2D Weakspot;
    
    public bool IsInVulnerableState;
    public int DmgFromTouching;
    
    public float DistanceToPlayer { get; set; }
    public float ReactsToPlayerDistance;
    public float AlarmedByPlayerDistance;
    public Alertness AlertnessLevel { get; set; }
    public float CountdownTillRestAlertness;
    public float CountdownTillRestAlertnessRemaining { get; set; }

    private Random _rand;
    private int _numberofAlertStates;
    private DateTime _lastAlertRise { get; set; }

    private bool Has3SecondsPassedSinceLastAlertRise =>
        (_lastAlertRise == default(DateTime) || (DateTime.Now - _lastAlertRise).TotalSeconds > 3);


    private DateTime BecameVulnerable;
    public int DurationVulnerable;

    //movement
    public Rigidbody2D MyRigidbody { get; set; }

    // Start is called before the first frame update
    public void Start()
    {
        MyRigidbody = gameObject.GetComponent<Rigidbody2D>();
        Player = GameObject.Find("Player");
        _rand = new Random(DateTime.Now.Millisecond);
        _numberofAlertStates = Enum.GetNames(typeof(Alertness)).Length;
    }

    // Update is called once per frame
    public void Update()
    {
        if (Player != null)
            DistanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        HandleAlertness();
        if (BecameVulnerable != default && (DateTime.Now - BecameVulnerable).TotalSeconds >= DurationVulnerable)
        {
            BecameVulnerable = default;
            IsInVulnerableState = false;
        }

        if (AlertnessLevel >= Alertness.Noticed)
        {
            AudioManager.instance.PlayCombatMusic();
        }
    }


    public void BecameVulnerableNow()
    {
        BecameVulnerable = DateTime.Now;
        IsInVulnerableState = true;
    }

    public void HandleAlertness()
    {
        if (DistanceToPlayer <= AlarmedByPlayerDistance)
        {
            AlertnessLevel = Alertness.Engaged;
            _lastAlertRise = DateTime.Now;
            //Debug.Log($"AlertnessLevel increased to {AlertnessLevel:G}");
        }
        else if (DistanceToPlayer <= ReactsToPlayerDistance && Has3SecondsPassedSinceLastAlertRise &&
                 _rand.Next(0, 10) % 4 == 0)
        {
            CountdownTillRestAlertnessRemaining = CountdownTillRestAlertness;
            _lastAlertRise = DateTime.Now;
            if ((int)AlertnessLevel < _numberofAlertStates)
            {
                AlertnessLevel++;
                //Debug.Log($"AlertnessLevel increased to {AlertnessLevel:G}");
            }
        }
        else if (AlertnessLevel > 0 && CountdownTillRestAlertnessRemaining <= 0)
        {
            AlertnessLevel--;
            //Debug.Log($"AlertnessLevel decreased to {AlertnessLevel:G}");
        }

        if (CountdownTillRestAlertnessRemaining > 0)
        {
            CountdownTillRestAlertnessRemaining -= Time.deltaTime;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        var playerScript = Player.GetComponent<Player>();
        //Debug.Log($"Bump!");
        //we go though any because we don't want to dmg the player if the player is touching the enemy in a weakened state 
        if (collision.contacts.Any(contact => contact.collider == Weakspot || contact.otherCollider == Weakspot) &&
            IsInVulnerableState && playerScript.State == PlayerState.Dashing)
        {
            var dmg = 100;
            if (health <= dmg)
            {
                AudioManager.instance.PlayMusic();
            }
            TakeDamage(dmg); //todo get amount of dmg from player object 
        }
        else if (collision.contacts.Any(contact => contact.otherCollider == Player || contact.collider == Player))
        {
            playerScript.TakeDamage(DmgFromTouching);
        }
    }

    public enum Alertness
    {
        Idle = 0,
        Noticed,
        Engaged
    }
}