using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    public int length;
    public LineRenderer lineRenderer;

    [Header("Tentacle Feel")]
    public float segmentLength;
    public float smoothSpeed;
    public float drag;

    [Header("Tentacle Wiggle")]
    public float wiggleMagnitude;
    public float wiggleSpeed;

    private Vector3[] segmentPoses;
    private Vector3[] segmentVels;
    private float startAngle;

    private void Start()
    {
        // set it to worldspace here instead of inspector so that we can have the tentacle look good in the unity scene
        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentVels = new Vector3[length];
        startAngle = Random.Range(0, 360);

        ResetLine();
    }

    private void Update()
    {
        segmentPoses[0] = transform.position;

        for (int i = 1; i < length; i++)
        {
            Vector3 targetDir = Quaternion.Euler(0, 0, Mathf.Sin(startAngle + Time.time * wiggleSpeed - (wiggleSpeed * i / length)) * wiggleMagnitude) * (transform.right + (segmentPoses[i] - segmentPoses[i - 1]).normalized * (drag * Mathf.Max(segmentVels[i].magnitude - wiggleMagnitude / 10, 0))).normalized;
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentPoses[i - 1] + targetDir * segmentLength, ref segmentVels[i], smoothSpeed);
        }

        lineRenderer.SetPositions(segmentPoses);
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
}
