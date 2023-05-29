using System;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class Enemy : Cell
{
    public GameObject Player { get; set; }
    public Collider2D Weakspot;
    public SpriteHandler spriteHandler;
    
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

    [Header("DNA Upgrade")]
    [SerializeField] private DNACollectible upgradeCollectiblePrefab;
    [Tooltip("The prefab for the type of upgrade dropped. Make this null if this enemy doesn't drop an upgrade")]
    [SerializeField] private DNAUpgrade upgradeType;

    //movement
    public Rigidbody2D MyRigidbody { get; set; }

    private void Awake()
    {
        Player = GameObject.Find("Player");
    }

    // Start is called before the first frame update
    public void Start()
    {
        MyRigidbody = gameObject.GetComponent<Rigidbody2D>();
        _rand = new Random(DateTime.Now.Millisecond);
        _numberofAlertStates = Enum.GetNames(typeof(Alertness)).Length;
    }

    // Update is called once per frame
    public void Update()
    {
        if (Player != null)
            DistanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        HandleAlertness();
        if (DurationVulnerable > 0 && BecameVulnerable != default && (DateTime.Now - BecameVulnerable).TotalSeconds >= DurationVulnerable)
        {
            BecameVulnerable = default;
            IsInVulnerableState = false;
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
            if (AlertnessLevel == Alertness.Idle)
            {
                GameManager.Instance.RegisterEnemyNoticed();
            }
            AlertnessLevel = Alertness.Engaged;
            _lastAlertRise = DateTime.Now;
        }
        else if (DistanceToPlayer <= ReactsToPlayerDistance && Has3SecondsPassedSinceLastAlertRise &&
                 _rand.Next(0, 10) % 4 == 0)
        {
            CountdownTillRestAlertnessRemaining = CountdownTillRestAlertness;
            _lastAlertRise = DateTime.Now;
            if ((int)AlertnessLevel < _numberofAlertStates)
            {
                if (AlertnessLevel == Alertness.Idle)
                {
                    GameManager.Instance.RegisterEnemyNoticed();
                }
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



    #region COLLISIONS

    public void OnCollisionEnter2D(Collision2D collision)
    {
        var playerScript = Player.GetComponent<Player>();
        //Debug.Log($"Bump!");
        //we go though any because we don't want to dmg the player if the player is touching the enemy in a weakened state 
        if (collision.contacts.Any(contact => contact.collider == Weakspot || contact.otherCollider == Weakspot) && IsInVulnerableState)
        {
            if (collision.gameObject.CompareTag("Player") && playerScript.State == PlayerState.Dashing)
            {
                var dmg = 100;
                if (health <= dmg)
                {
                    GameManager.Instance.UnregisterNoticedEnemy();
                }
                playerScript.Attack(this);
            }
            else if (collision.gameObject.CompareTag("Bullet"))
            {
                PlayerProjectile bullet = collision.gameObject.GetComponent<PlayerProjectile>();
                TakeDamage(bullet.Damage);
            }
        }
        //else if (collision.contacts.Any(contact => contact.otherCollider == Player || contact.collider == Player)) // this didnt work since it was comparing a collider to a gameobject
        else if (collision.gameObject.CompareTag("Player")) // this will detect if it collides with any player colliders
        {
            DealDamage(playerScript);
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        var playerScript = Player.GetComponent<Player>();
        if (collision.gameObject.CompareTag("Player") && playerScript.State != PlayerState.Dashing)
        {
            DealDamage(playerScript);
        }
    }


    #endregion


    protected virtual void DealDamage(Player playerScript) {
        playerScript.TakeDamage(DmgFromTouching);
        OnDealDamage();
    }

    // virtual method for child classes to access damage effects (sound, vfx etc.)
    protected virtual void OnDealDamage() {}

    public override void Die()
    {
        if (upgradeType != null && PlayerManager.Instance.currentDNAUpgrade != upgradeType)
        {
            DNACollectible collectible = Instantiate(upgradeCollectiblePrefab, transform.position, Quaternion.identity);
            collectible.dnaUpgrade = upgradeType;
        }
        base.Die();
    }

    public enum Alertness
    {
        Idle = 0,
        Noticed,
        Engaged
    }
}