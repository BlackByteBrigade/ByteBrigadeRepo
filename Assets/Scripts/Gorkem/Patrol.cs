using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Patrol : MonoBehaviour
{
    private enum PatrolStates
    {
        Decide,
        Prep,
        Movement,
        Wait
    }
    [SerializeField] private float distanceToMove;
    [SerializeField] private float timeToReach;
    [SerializeField] private float timeBtwMoves;
    [SerializeField] private float maxDistance;
    [SerializeField] private AnimationCurve movementCurve;
    [SerializeField] private Transform anchorPoint;
    [SerializeField] private bool canMoveContinuously;
    [SerializeField] private bool isFloating;
    [SerializeField] private float floatingSpeed;

    private PatrolStates state;
    private Vector3 beforeMovementPosition;

    private Vector2 movementVector;
    private Vector3 oldDirectionVector;
    private Vector3 directionVector;

    private float movementCounter;
    private float waitTimeCounter;
    private bool isReached;

    private Quaternion toRotate;
    [SerializeField] float rotationSpeed;
    private void Awake()
    {
        state = PatrolStates.Decide;
    }

    private void Update()
    {
        print(state);
        switch (state)
        {
            case PatrolStates.Decide:
                DirectionDecider();
                break;
            case PatrolStates.Prep:
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, rotationSpeed * Time.deltaTime);

                if (transform.rotation == toRotate)
                {
                    beforeMovementPosition = transform.position;
                    state = PatrolStates.Movement;
                }
                break;

            case PatrolStates.Movement:
                transform.position = Vector2.Lerp(beforeMovementPosition, movementVector, movementCurve.Evaluate(movementCounter));
                movementCounter += Time.deltaTime / timeToReach;

                if (Vector2.Distance(transform.position, movementVector) < Mathf.Epsilon)
                {
                    movementCounter = 0;
                    state = PatrolStates.Wait;
                }
                break;

            case PatrolStates.Wait:
                waitTimeCounter += Time.deltaTime;
                if (isFloating)
                {
                    transform.position += oldDirectionVector * floatingSpeed * Time.deltaTime;
                }
                if (waitTimeCounter > timeBtwMoves)
                {
                    state = PatrolStates.Decide;
                }
                break;
        }
    }
    private void DirectionDecider()
    {
        if (Vector2.Distance(transform.position, anchorPoint.transform.position) > maxDistance)
        {
            var anchorDir = (anchorPoint.transform.position - transform.position).normalized;
            movementVector = transform.position + anchorDir * distanceToMove;
        }
        else
        {
            var RaycastHitUp = Physics2D.Raycast(transform.position, Vector2.up, distanceToMove);
            var RaycastHitDown = Physics2D.Raycast(transform.position, Vector2.down, distanceToMove);
            var RaycastHitLeft = Physics2D.Raycast(transform.position, Vector2.left, distanceToMove);
            var RaycastHitRight = Physics2D.Raycast(transform.position, Vector2.right, distanceToMove);

            var listOfDirections = new List<Vector2>();

            if (RaycastHitUp.collider == null)
            {
                listOfDirections.Add(Vector2.up);
            }
            if (RaycastHitDown.collider == null)
            {
                listOfDirections.Add(Vector2.down);
            }
            if (RaycastHitLeft.collider == null)
            {
                listOfDirections.Add(Vector2.left);
            }
            if (RaycastHitRight.collider == null)
            {
                listOfDirections.Add(Vector2.right);
            }

            if (listOfDirections.Count == 0)
            {
                movementVector = transform.position;                  //If there is no place to go
            }
            else
            {
                directionVector = (Vector3)listOfDirections[Random.Range(0, listOfDirections.Count)];
                toRotate = Quaternion.LookRotation(Vector3.forward, directionVector);

                movementVector = transform.position + directionVector * distanceToMove;
            }
            listOfDirections.Clear();
            if (directionVector == oldDirectionVector && canMoveContinuously)
            {
                waitTimeCounter = Mathf.Infinity;
            }
            else
            {
                waitTimeCounter = 0;
            }
                state = PatrolStates.Prep;
            oldDirectionVector = directionVector;
        }
    }
}