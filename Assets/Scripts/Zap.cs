using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zap : MonoBehaviour
{
    public float damageTime = 0.1f;
    public float interval = 1f;
    private Animator anim;
    private Collider2D col;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        col.enabled = false;
        StartCoroutine(StrikeRoutine());
    }

    private IEnumerator StrikeRoutine()
    {
        anim.Play("Strike");
        col.enabled = true;
        
        yield return new WaitForSeconds(damageTime);
        col.enabled = false;

        yield return new WaitForSeconds(interval - damageTime);
        StartCoroutine(StrikeRoutine());
    }
}
