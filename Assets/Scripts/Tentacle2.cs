using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle2 : MonoBehaviour
{
    public int length;
    public LineRenderer lineRenderer;

    [Header("Tentacle Feel")]
    public float segmentLength;
    public float segmentLengthIdle;
    public float smoothSpeed;
    public float drag;

    [Header("Tentacle Wiggle")]
    public float wiggleMagnitude;
    public float wiggleSpeed;


    private Vector3[] segmentPoses;
    private Vector3[] segmentVels;
    private float startAngle;
    float segmentLengthMoving;

    public State state = State.Idle;
    public enum State { Idle = 0, Moving = 1 }

    [SerializeField] Rigidbody2D Body;
    [SerializeField] float scale = 0.4f;

    [SerializeField] Gradient gradientIdle;
    [SerializeField] Gradient gradientMoving;



    private void Start()
    {
        // set it to worldspace here instead of inspector so that we can have the tentacle look good in the unity scene
        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentVels = new Vector3[length];
        startAngle = Random.Range(0, 360);

        segmentLengthMoving = segmentLength / scale;
        segmentLengthIdle /= scale;

        EnterIdleState();
        ResetLine();
    }

    private void Update()
    {
        segmentPoses[0] = transform.position;
        UpdateState();

        for (int i = 1; i < length; i++)
        {

            float currentAngle = startAngle + Time.time * wiggleSpeed - (wiggleSpeed * i / length);
            float sinValue = Mathf.Sin(currentAngle);
            Vector3 segmentDirection = (segmentPoses[i] - segmentPoses[i - 1]).normalized;
            float velocityMagnitude = segmentVels[i].magnitude - wiggleMagnitude / 10;
            Vector3 normalizedDirection = (transform.right + segmentDirection * (drag * Mathf.Max(velocityMagnitude, 0))).normalized;

            Vector3 targetDir = Quaternion.Euler(0, 0, sinValue * wiggleMagnitude) * normalizedDirection;


            Vector3 currentPos = segmentPoses[i];
            Vector3 targetPos = segmentPoses[i - 1] + targetDir * segmentLength;
            segmentPoses[i] = Vector3.SmoothDamp(currentPos, targetPos, ref segmentVels[i], smoothSpeed);

            // Vector3 targetDir = Quaternion.Euler(0, 0, Mathf.Sin(startAngle + Time.time * wiggleSpeed - (wiggleSpeed * i / length)) * wiggleMagnitude) * (transform.right + (segmentPoses[i] - segmentPoses[i - 1]).normalized * (drag * Mathf.Max(segmentVels[i].magnitude - wiggleMagnitude / 10, 0))).normalized;
        }

        lineRenderer.SetPositions(segmentPoses);
    }


    void UpdateState() {
        float currentSpeed = Body.velocity.magnitude;

        if (currentSpeed > 0.1f && state == State.Idle) 
            EnterMovingState();
        

        else if (currentSpeed < 0.1f && state == State.Moving)
            EnterIdleState();
    }


    void EnterIdleState() {
        segmentLength = segmentLengthIdle;
        lineRenderer.colorGradient = gradientIdle;
        state = State.Idle;
    }
    
    void EnterMovingState() {
        segmentLength = segmentLengthMoving;
        lineRenderer.colorGradient = gradientMoving;
        state = State.Moving;
    }



    private void ResetLine()
    {
        segmentPoses[0] = transform.position;

        for (int i = 1; i < length; i++)
        {
            segmentPoses[i] = segmentPoses[i - 1] + transform.right * segmentLength;
        }

        lineRenderer.SetPositions(segmentPoses);
    }

    void OnDrawGizmos() {

        Gizmos.color = Color.yellow;
        Gizmos.matrix = transform.localToWorldMatrix;
        float width = length * segmentLength;

        Vector3 size = new Vector3(width, 0.4f, 0);
        Gizmos.DrawWireCube(new Vector3(width/2f, 0, 0), size);
    }
}
