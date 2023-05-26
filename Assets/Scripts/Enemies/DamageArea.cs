using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Pascal

public class DamageArea : Enemy {

    [Header("Damage Area")]
    [SerializeField] Sound ambientSound;
    [SerializeField] Sound damageSound;
    
    AudioPlayer audioPlayer;


    public void Start() {
        base.Start();
        if (TryGetComponent<AudioPlayer>(out audioPlayer)) {
            audioPlayer.AddSound(ambientSound);
            audioPlayer.AddSound(damageSound);
            audioPlayer.Play(ambientSound);
        }
    }

    protected override void OnDealDamage() {
        Debug.Log("play damage sound");
        audioPlayer?.Play(damageSound);
        // TODO add vfx
    } 
}
