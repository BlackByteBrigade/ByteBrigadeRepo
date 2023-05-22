using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    public int length;
    public LineRenderer lineRenderer;

    public float segmentLength;
    public float smoothSpeed;
    public float drag;

    private Vector3[] segmentPoses;
    private Vector3[] segmentVels;

    private void Start()
    {
        lineRenderer.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentVels = new Vector3[length];
    }

    private void Update()
    {
        segmentPoses[0] = transform.position;

        for (int i = 1; i < length; i++)
        {
            Vector3 targetDir = (transform.right + (segmentPoses[i] - segmentPoses[i - 1]).normalized * drag).normalized;
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentPoses[i - 1] + targetDir * segmentLength, ref segmentVels[i], smoothSpeed);
        }

        lineRenderer.SetPositions(segmentPoses);
    }
}
