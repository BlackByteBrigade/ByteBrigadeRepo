using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class DNACollectible : MonoBehaviour
{
    public DNAUpgrade dnaUpgrade;
    public float launchDist = 2;
    public float cantPickupTime = 1;

    private bool canPickup = false;

    private void Start()
    {
        if (Player.instance != null)
        {
            transform.DOMove(transform.position + (transform.position - Player.instance.transform.position).normalized * launchDist, cantPickupTime).SetEase(Ease.OutCubic);
        }
        Invoke(nameof(CanPickup), cantPickupTime);
    }

    private void CanPickup()
    {
        canPickup = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canPickup && collision.gameObject.CompareTag("Player"))
        {
            Player.instance.ChangeDNA(dnaUpgrade);
            AudioManager.instance.PlaySfX(SoundEffects.CollectingDna);
            Destroy(gameObject);
        }
    }
}