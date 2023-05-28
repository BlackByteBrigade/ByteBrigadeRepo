using System.Collections.Generic;
using UnityEngine;

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
    [Tooltip("Allowed maximum distance to move from the anchor point.")]
    [SerializeField] private float maxDistance;
    [Tooltip("Start point must be (0,0) and end point(1,1). You can set the movement with the slope of the curve.")]
    [SerializeField] private AnimationCurve movementCurve;
    [Tooltip("It keeps track of distance to that point. If it is further than maxDistance, turns back.")]
    [SerializeField] private Vector3 anchorPoint;
    [Tooltip("After the movement if it decides to move same direction wait time between moves becomes 0 and moves without delay")]
    [SerializeField] private bool canMoveContinuously;
    [Tooltip("When it is in waiting state, it continues to move(kinda make more sense for cells)")]
    [SerializeField] private bool isFloating;
    [Tooltip("Waiting state movement speed)")]
    [SerializeField] private float floatingSpeed;

    private PatrolStates state;

    private Vector3 beforeMovementPosition;
    private Vector2 movementVector;
    private Vector3 oldDirectionVector;
    private Vector3 directionVector;

    private float movementCounter;
    private float waitTimeCounter;

    private Quaternion toRotate;
    [SerializeField] float rotationSpeed;

    LayerMask mask;
    private void Awake()
    {
        state = PatrolStates.Decide;
        anchorPoint = transform.position;
        mask = LayerMask.GetMask("Level");

    }

    private void Update()
    {
        PatrolMovement(); //We should call this method in the enemy idle(patrol) state instead of here
    }
    public void PatrolMovement()
    {
        // print(state);
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
        if (Vector2.Distance(transform.position, anchorPoint) > maxDistance)
        {
            var anchorDir = (anchorPoint - transform.position).normalized;
            movementVector = transform.position + anchorDir * distanceToMove;
        }
        else
        {
            var RaycastHitUp = Physics2D.Raycast(transform.position, Vector2.up, distanceToMove, mask);
            var RaycastHitDown = Physics2D.Raycast(transform.position, Vector2.down, distanceToMove , mask);
            var RaycastHitLeft = Physics2D.Raycast(transform.position, Vector2.left, distanceToMove , mask);
            var RaycastHitRight = Physics2D.Raycast(transform.position, Vector2.right, distanceToMove, mask);

            var listOfDirections = new List<Vector2>();

            if (IsNullOrNotSpecifiedCollider(RaycastHitUp))
            {
                listOfDirections.Add(Vector2.up);
            }
            if (IsNullOrNotSpecifiedCollider(RaycastHitDown))
            {
                listOfDirections.Add(Vector2.down);
            }
            if (IsNullOrNotSpecifiedCollider(RaycastHitLeft))
            {
                listOfDirections.Add(Vector2.left);
            }
            if (IsNullOrNotSpecifiedCollider(RaycastHitRight))
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

    private  bool IsNullOrNotSpecifiedCollider(RaycastHit2D RaycastHitUp)
    {
        return RaycastHitUp.collider == null;
    }
}
