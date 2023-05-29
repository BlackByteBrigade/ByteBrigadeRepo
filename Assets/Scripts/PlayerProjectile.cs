using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public int Damage;
    public float Offset;
    public float MovementSpeed;
    public GameObject destroyPatricles;

    private Vector3 _startPoint;

    // Start is called before the first frame update
    void Start()
    {
        transform.position += transform.up * Offset;
        _startPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * Time.deltaTime * MovementSpeed;
        if (Vector2.Distance(transform.position, _startPoint) > 25)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(destroyPatricles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
