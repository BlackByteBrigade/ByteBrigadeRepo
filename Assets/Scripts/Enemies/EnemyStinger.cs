using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyStinger : Enemy
{
    [Header("Dash")]
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDistance = 20f;
    [SerializeField] float durationBetweenDashes = 5f;
    [Tooltip("Time between end of turning and start of dash")] 
    [SerializeField] float dashDelay = 0.5f;

    // TEMP
    [SerializeField] AudioSource dashSound;
    
    float dashDuration = 1f; // deperecated


    Transform playerTransform;
    Vector3 anchorPosition;
    _State state = _State.Idle;
    enum _State {
        Idle = 0,
        Turning = 1,
        Dashing = 2,
        Recovering = 3,
    }
    

    new void Start() {

        anchorPosition = transform.position;
        base.Start();
        playerTransform = Player.transform;
    }

    new void Update() {
        base.Update();
        if (AlertnessLevel >= Alertness.Engaged && state == _State.Idle)
            // System.Action callback = ()=> DashTowardsPlayer();
            RotateTowardsPlayer();
    }


    void RotateTowardsPlayer(System.Action callback = null) {
        state = _State.Turning;
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
        state = _State.Dashing;
        Vector3 targetPos = transform.position + targetDirection.normalized * dashDistance;
        float _time = dashDistance / dashSpeed;
        Invoke(nameof(PlayDashSound), dashDelay-0.1f);
        transform.DOMove(targetPos, _time)
            .SetEase(Ease.InQuad)
            .SetDelay(dashDelay)
            .OnComplete(
                () => Recover()
            );
    }

    void PlayDashSound() => dashSound.Play();


    void Recover() {
        state = _State.Recovering;
        Invoke(nameof(SetIdle), durationBetweenDashes);
    }

    void SetIdle() {
        state = _State.Idle;
    }


    // IEnumerator IE_Dash(Vector3 direction) {

    //     state = _State.Dashing;

    //     // Vector3 direction = (playerTransform.position - transform.position).normalized;
    //     float dashTimer = 0f;

    //     while (dashTimer < dashDuration) {

    //         dashTimer += Time.deltaTime;
    //         transform.position += direction * dashSpeed * Time.deltaTime;
    //         yield return null;
    //     }

    //     state = _State.Recovering;

    //     yield return new WaitForSeconds(durationBetweenDashes);
    //     state = _State.Idle;
    // }


    void OnDrawGizmos() {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ReactsToPlayerDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, AlarmedByPlayerDistance);
    }
}
