using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_RotateMitoChondria : MonoBehaviour
{
    [SerializeField] Transform tf_mitchondria;
    [SerializeField] float speed;


    void Update() {
        tf_mitchondria.Rotate(Vector3.forward * speed * Time.deltaTime);
    }
}
