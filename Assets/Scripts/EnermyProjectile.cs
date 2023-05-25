using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnermyProjectile : MonoBehaviour
{

    public int Damage;
    public float MovementSpeed;
    private bool _isTriggered;
    private Vector3 _startPoint;
    private GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
        _startPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isTriggered)
        {
            transform.position += transform.up * Time.deltaTime * MovementSpeed;
            if (Vector2.Distance(transform.position, _startPoint) > 25)
            {
                Destroy(gameObject);
            }
        }
    }

    public void FireStraightLine()
    {
        _isTriggered = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _player.GetComponent<Player>().TakeDamage(Damage);
        }
    }
}
