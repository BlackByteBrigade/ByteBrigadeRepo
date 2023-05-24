using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    public AnimationCurve rotationSpeedCurve;
    public float rotationDuration = 5f; // Total duration of the rotation

    private float rotationStartTime;
    private Transform spriteTransform;

    private void Start()
    {
        spriteTransform = GetComponent<Transform>();
        rotationStartTime = Time.time;
    }

    private void Update()
    {
        // Calculate the normalized time based on the duration
        float normalizedTime = (Time.time - rotationStartTime) / rotationDuration;

        // Evaluate the Animation Curve to get the rotation speed
        float rotationSpeed = rotationSpeedCurve.Evaluate(normalizedTime % 1f);

        // Rotate the sprite based on the rotation speed
        spriteTransform.Rotate(Vector3.forward, rotationSpeed* speed* Time.deltaTime);
    }
}
