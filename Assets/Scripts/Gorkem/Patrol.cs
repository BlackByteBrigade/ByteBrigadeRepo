using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] float distanceToMove;
    [SerializeField] float speed;
    [SerializeField] float timeBtwMoves;
    private Vector2 movementVector;
    float timeCounter;
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
            timeCounter = 0;
        }
        else if (!isReached)
        {
            transform.position = Vector2.MoveTowards(transform.position, movementVector, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, movementVector) < 0.1f)
            {
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
            movementVector = transform.position + (Vector3)listOfDirections[Random.Range(0, listOfDirections.Count)] * distanceToMove;
        }

        listOfDirections.Clear();
    }
}
