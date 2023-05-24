using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


/*
            STINGER BEHAVIOUR:
            ------------------

    Alertness {Idle, Noticed, Engaged}

    Idle: 
        slowly move around
        no dashes
        random patrol within radius of anchorpoint


    Noticed:
        noticed animation + sound
    
    Engaged:

        Turning
            1. rotate towards player
                if angle > threshold swim in a tight radius
                else turn on spot

        Wind Up
            2. save player position as target position
            3. short delay with maybe an additional telegraph
                tail wind up

        Dashing
            4. fast dash towards saved target position
                tail unwinds
                Dash sound
        
        Recovering
            5. Stay in place for <durationBetweenDashes>
            6. if player out of range 
                => return to anchorpoint
            7. if player still within range
                => Turning state

*/


// Author: Pascal

public class EnemyStinger : Enemy
{


    [Header("Dash")]
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDistance = 20f;
    [SerializeField] float durationBetweenDashes = 5f;
    [Tooltip("Time between end of turning and start of dash")] 
    [SerializeField] float dashDelay = 0.5f;

    [SerializeField] StingerTail tail;

    // TEMP
    [SerializeField] AudioSource dashSound;


    

    Transform playerTransform;
    Vector3 anchorPosition;
    State state = State.Idle;
    enum State {
        Idle = 0,
        Turning = 1,
        Dashing = 2,
        Recovering = 3,
    }
    
    // should be "protected override"
    new void Start() {

        anchorPosition = transform.position;
        base.Start(); // enemy needs public virtual void Start
        playerTransform = Player.transform;
    }

    // should be "protected override"
    new void Update() {
        base.Update(); // enemy needs public virtual void Update
        if (AlertnessLevel >= Alertness.Engaged && state == State.Idle)
            // System.Action callback = ()=> DashTowardsPlayer();
            RotateTowardsPlayer();
    }






    void RotateTowardsPlayer(System.Action callback = null) {
        state = State.Turning;
        Vector3 targetDirection = playerTransform.position - transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        transform.DORotate(new Vector3(0f, 0f, targetAngle), 1f)
            .SetEase(Ease.OutQuad)
            .OnComplete( 
                // () => { if (callback != null) callback(); }
                () => DashTowardsPlayer(targetDirection)
            );
    }

    void DashTowardsPlayer(Vector3 targetDirection, System.Action callback = null) {
        state = State.Dashing;
        Vector3 targetPos = transform.position + targetDirection.normalized * dashDistance;
        float _time = dashDistance / dashSpeed;
        transform.DOMove(targetPos, _time)
            .SetEase(Ease.InQuad)
            .SetDelay(dashDelay)
            .OnComplete(
                () => Recover()
            );

        Invoke(nameof(PlayDashSound), dashDelay-0.1f);
    }

    void PlayDashSound() => dashSound.Play();


    void Recover() {
        state = State.Recovering;
        Invoke(nameof(SetIdle), durationBetweenDashes);
    }

    void SetIdle() {
        state = State.Idle;
    }








    // IEnumerator IE_Dash(Vector3 direction) {

    //     state = State.Dashing;

    //     // Vector3 direction = (playerTransform.position - transform.position).normalized;
    //     float dashTimer = 0f;
    //     float dashDuration = 1f;

    //     while (dashTimer < dashDuration) {

    //         dashTimer += Time.deltaTime;
    //         transform.position += direction * dashSpeed * Time.deltaTime;
    //         yield return null;
    //     }

    //     state = State.Recovering;

    //     yield return new WaitForSeconds(durationBetweenDashes);
    //     state = State.Idle;
    // }


    
}
