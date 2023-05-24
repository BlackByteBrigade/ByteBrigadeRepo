using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] private float distanceToMove;
    [SerializeField] private float timeToReach;
    [SerializeField] private float timeBtwMoves;
    [SerializeField] private float maxDistance;
    [SerializeField] private AnimationCurve movementCurve;
    [SerializeField] private Transform anchorPoint;

    private Vector3 beforeMovementPosition;
    private Vector2 movementVector;
    private Vector3 oldDirectionVector;
    private Vector3 directionVector;
    private float movementCounter;
    private float timeCounter;
    private bool isReached;

    private void Update()
    {
        Patrolling();
    }
    private void Patrolling()
    {
        if (isReached && timeBtwMoves < timeCounter)
        {
            DirectionDecider();
            isReached = false;
        }
        else if (!isReached)
        {
            transform.position = Vector2.Lerp(beforeMovementPosition, movementVector, movementCurve.Evaluate(movementCounter));
            movementCounter += Time.deltaTime / timeToReach;
            print(movementCounter);
            if (Vector2.Distance(transform.position, movementVector) < 0.1f)
            {
                movementCounter = 0;
                isReached = true;
            }
        }
        else
        {
            timeCounter += Time.deltaTime;
        }
    }
    private void DirectionDecider()
    {
        beforeMovementPosition = transform.position;

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
                movementVector = transform.position;
            }
            else
            {
                directionVector = (Vector3)listOfDirections[Random.Range(0, listOfDirections.Count)];

                if (directionVector == oldDirectionVector)
                {
                    timeCounter = Mathf.Infinity;
                    print(5);
                }
                else
                {
                    timeCounter = 0;
                }
                oldDirectionVector = directionVector;
                movementVector = transform.position + directionVector * distanceToMove;
            }
            listOfDirections.Clear();
        }
    }
}
