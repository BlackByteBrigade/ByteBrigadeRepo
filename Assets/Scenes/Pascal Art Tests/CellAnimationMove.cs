using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellAnimationMove : MonoBehaviour
{
    
    [SerializeField] float distance = 0.65f;
    [SerializeField] float angleOffset = 90f;
    Animator anim;
    bool isMoving;

    void Start() {
        anim = GetComponent<Animator>();
    }



    void Update() {
        

        

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput != 0f || verticalInput != 0f)
        {
            float targetRotation = Mathf.Atan2(verticalInput, horizontalInput) * Mathf.Rad2Deg;
            targetRotation += angleOffset;
            transform.rotation = Quaternion.Euler(0f, 0f, targetRotation);
            Move();
        }

    }

    public void Move() {
        if (isMoving) return;
        isMoving = true;
        anim.Play("Move");
    }

    public void MoveComplete() {
        Debug.Log("stop");
        isMoving = false;
        anim.Play("Idle");
        // transform.position += new Vector3(offset.x, offset.y, 0);


        float rotationAngle = transform.eulerAngles.z -90f; // move down
        float radians = rotationAngle * Mathf.Deg2Rad;

        Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;
        Vector2 movement = direction * distance;

        transform.position += (Vector3)movement;
    }
}
