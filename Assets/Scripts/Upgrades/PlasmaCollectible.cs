using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Author: Pascal

public class PlasmaCollectible : MonoBehaviour
{
    [SerializeField] int value = 1;
    bool canPickup = true;

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player") && canPickup) {
            canPickup = false;
            PlayerHUD.instance.AddPlasma(value);
            AudioManager.instance.PlaySfX(SoundEffects.CollectingEnemyPart);
            Destroy(gameObject);
        }
    }
}
