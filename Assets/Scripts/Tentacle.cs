using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    public int length;
    public LineRenderer lineRenderer;
    public Vector3[] linePositions;

    private void Start()
    {
        lineRenderer.positionCount = length;
        linePositions = new Vector3[length];
    }

    private void Update()
    {
        linePositions[0] = transform.position;

        for (int i = 1; i < length; i++)
        {

        }
    }
}
