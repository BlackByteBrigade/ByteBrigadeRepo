using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[SelectionBase]
public class TCellAnimations : MonoBehaviour
{

    [Header("Eye")]
    [SerializeField] Transform eye;
    [SerializeField] float eyeRadius = 1f;
    [SerializeField] float eyeSpeed = 1f;
    [SerializeField] Ease easeEye;
    [SerializeField] float eyePause = 1f;

    [Header("Arms")]
    [SerializeField] Transform arms;
    [SerializeField] float rotationSpeed = 10f;  
    [SerializeField] float rotationAngle = 30f;
    [SerializeField] Ease easeArms;


    Quaternion startRotation;
    Quaternion endRotation;
    bool isForward = true;
    Vector3 eyePosition;

    Tweener tweenEye;
    Tweener tweenArms;



    void Start() {
        startRotation = transform.rotation;
        endRotation = startRotation * Quaternion.Euler(0f, 0f, rotationAngle);

        eyePosition = eye.position;
        isForward = (Random.Range(0,1f) > 0.5f);
        Invoke(nameof(RotateArms), Random.Range(0, 2f));
        MoveEye();
    }

    void RotateArms() {

        float duration = Mathf.Abs(rotationAngle) / rotationSpeed;

        if (isForward)
        {
            tweenArms = arms.DORotate(endRotation.eulerAngles, duration)
                .SetEase(easeArms)
                .OnComplete(() =>
                {
                    isForward = false;
                    RotateArms();
                });
        }
        else
        {
            tweenArms = arms.DORotate(startRotation.eulerAngles, duration)
                .SetEase(easeArms)
                .OnComplete(() =>
                {
                    isForward = true;
                    RotateArms();
                });
        }
    }

   

    void MoveEye() {
        Vector2 randomOffset = Random.insideUnitCircle * eyeRadius;


        Vector2 direction = Player.instance.transform.position - eyePosition;
        float angle = Mathf.Atan2(direction.y, direction.x);
        Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * eyeRadius;

        Vector2 targetPosition = eyePosition + offset;
        tweenEye = eye.DOMove(new Vector3(targetPosition.x, targetPosition.y, 0), eyeSpeed)
            .SetSpeedBased(true)
            .SetEase(easeEye)
            // .SetDelay(eyePause)
            .OnComplete(()=> MoveEye());

    }


    void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(eye.position, eyeRadius);
    }

    void OnDestroy() {
        tweenEye.Kill();
        tweenArms.Kill();
    }
}
