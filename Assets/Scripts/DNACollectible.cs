using System;
using System.Collections;
using UnityEngine;

public class DNACollectible : MonoBehaviour
{
    public DNAUpgrade dnaUpgrade;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.instance.ChangeDNA(dnaUpgrade);
            AudioManager.instance.PlaySfX(SoundEffects.CollectingDna);
            Destroy(gameObject);
        }
    }
}