using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zap : MonoBehaviour
{
    public float delay = 0;
    public float damageTime = 0.1f;
    public float interval = 1f;
    public Sound zapSound;

    private Animator anim;
    private Collider2D col;
    private AudioPlayer audioPlayer;

    public virtual void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        audioPlayer = GetComponent<AudioPlayer>();

        if (audioPlayer != null)
        {
            audioPlayer.AddSound(zapSound);
        }

        SetColliderActive(false);
        StartCoroutine(StartFirstStrike());
    }

    private void SetColliderActive(bool isActive)
    {
        if (col == null) return;
        col.enabled = isActive;
    }

    private IEnumerator StartFirstStrike()
    {
        yield return new WaitForSeconds(5);
        yield return new WaitForSeconds(delay);
        StartCoroutine(StrikeRoutine());
    }

    private IEnumerator StrikeRoutine()
    {
        anim.Play("Strike");
        if (audioPlayer != null) audioPlayer.Play(zapSound);
        SetColliderActive(true);

        yield return new WaitForSeconds(damageTime);
        SetColliderActive(false);

        yield return new WaitForSeconds(interval - damageTime);
        StartCoroutine(StrikeRoutine());
    }
}
