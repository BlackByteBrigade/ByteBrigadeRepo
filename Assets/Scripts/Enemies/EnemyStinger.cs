using System;
using System.Collections;
using UnityEngine;

[SelectionBase]
public class EnemyStinger : Enemy
{
    [Header("Dash")] [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDistance = 20f;
    [SerializeField] float durationBetweenDashes = 5f;

    [Tooltip("Time between end of turning and start of dash")] [SerializeField]
    float dashDelay = 1.5f;

    [SerializeField] Sound dashSound;
    [SerializeField] int dashDamage;

    [SerializeField] StingerTail tail;

    private Rigidbody2D _body;
    public int TurnSpeed;

    private Patrol _patrolComponent { get; set; }

    AudioPlayer audioPlayer;
    Transform playerTransform;
    int idleDamage;
    Vector3 savedTargetPosition;
    State state = State.Idle;

    enum State
    {
        Idle = 0,
        WindUp = 1,
        Dashing = 2,
        Recovering = 3,
    }

    public void Start()
    {
        if (TryGetComponent(out audioPlayer))
        {
            audioPlayer.AddSound(dashSound);
        }

        idleDamage = DmgFromTouching;
        base.Start(); // enemy needs public virtual void Start
        playerTransform = Player.transform;
        tail.SetIdle();

        _patrolComponent = GetComponent<Patrol>();
        _body = GetComponent<Rigidbody2D>();

        if (TurnSpeed == 0) TurnSpeed = 500;
    }

    public void Update()
    {
        base.Update(); // enemy needs public virtual void Update
        if (AlertnessLevel >= Alertness.Engaged && state == State.Idle)
        {
            _patrolComponent.DisablePatrolling();

            StartCoroutine(TargetAndAttackPlayer());
        }
        else if (AlertnessLevel == Alertness.Idle && state == State.Idle)
        {
            _patrolComponent.EnablePatrolling();
        }
    }


    private IEnumerator TargetAndAttackPlayer()
    {
        if (IsDead) yield break;

        state = State.WindUp;
        tail.WindUp();
        savedTargetPosition = playerTransform.position - transform.position;
        var targetAngle = Mathf.Atan2(savedTargetPosition.y, savedTargetPosition.x) * Mathf.Rad2Deg;

        Debug.Log("Rotate to target");


        var targetAsQuaternion = Quaternion.Euler(0, 0, targetAngle - 90);
        Debug.Log($"Target rotation: {targetAsQuaternion}");
        do
        {
            //continuously check if the player has killed the enemy
            if (IsDead) yield break;

            float rotAmount = 225 * Time.fixedDeltaTime;
            if (Quaternion.Angle(targetAsQuaternion, transform.rotation) <= rotAmount)
            {
                transform.rotation = targetAsQuaternion;
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, targetAsQuaternion, Time.fixedDeltaTime * 5);
            }

            yield return new WaitForFixedUpdate();
        } while (Quaternion.Angle(targetAsQuaternion, transform.rotation) >= 0.05f); //we just wanna get close enough

        yield return new WaitForSeconds(dashDelay);

        StartCoroutine(DashToTarget());
    }

    private IEnumerator DashToTarget()
    {
        Debug.Log("Dash to target");
        if (IsDead) yield break;
        state = State.Dashing;
        tail.Dash();
        DmgFromTouching = dashDamage;
        _body.GetComponent<Collider2D>().isTrigger = true;
        var RaycastHit = Physics2D.Raycast(transform.position, transform.up, dashDistance, LayerMask.GetMask("Level"));
        var targetPos = transform.position + savedTargetPosition.normalized *
            MathF.Min(dashDistance, RaycastHit.collider != null ? RaycastHit.distance : 999);
        if (RaycastHit.collider != null)
            Debug.Log($"We hit: {RaycastHit.collider.gameObject.name}");
        Vector2 targetPosVector2 = targetPos; //use as vector 2 to use same logic as EnemyBasic

        PlayDashSound();
        do
        {
            //continuously check if the player has killed the enemy
            if (IsDead) yield break;


            Vector2 currentPos = transform.position; // easier to deal with the position as a vector2
            var targetDir = (targetPosVector2 - currentPos).normalized;

            transform.position += (Vector3)targetDir * 15.5f * Time.deltaTime;
            transform.up = targetDir;

            yield return null;
        } while (GetDistanceTo(targetPos) >= 0.2f);

        _body.GetComponent<Collider2D>().isTrigger = false;

        _body.velocity = Vector2.zero;
        Recover();
    }

    public float GetDistanceTo(Vector3 position)
    {
        return Vector3.Distance(transform.position, position);
    }

    void PlayDashSound() => audioPlayer?.Play(dashSound);


    void Recover()
    {
        Debug.Log("Recovering");
        tail.SetIdle();
        DmgFromTouching = idleDamage;
        state = State.Recovering;
        Invoke(nameof(SetIdle), durationBetweenDashes);
    }

    void SetIdle()
    {
        tail.SetIdle();
        state = State.Idle;
        DmgFromTouching = idleDamage;
    }
}