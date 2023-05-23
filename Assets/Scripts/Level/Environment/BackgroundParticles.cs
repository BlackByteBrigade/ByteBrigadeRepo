using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParticles : MonoBehaviour
{

    [SerializeField] float speed = 0.1f;
    
    Vector3 initialPosition;


    void Start() {
        initialPosition = transform.position;
    }


    void Update() {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

}
