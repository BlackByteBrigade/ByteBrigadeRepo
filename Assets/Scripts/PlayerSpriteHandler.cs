using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerSpriteHandler : SpriteHandler {
    
    const string blinkID = "_BlinkFrequency";
    [SerializeField] float blinkFrequency = 10f;
    [SerializeField] Material playerMaterial;
    [SerializeField] SpriteRenderer nucleus;

    Player player;

    protected override void Awake() {
        material = playerMaterial;
        material.SetFloat(blinkID, 0);

        player = (Player) owner;

        player.OnDamageTaken += HandleDamage;
        player.OnDeath += HandleDeath;
        player.OnInvulnerableChange += HandleInvulnerableChange;
        player.OnDashReady += HandleDashReady;

    }



    protected override void HandleDamage(int damageTaken) {
        if (damageTaken <= 0) return;
        base.HandleDamage(damageTaken);
    }


    void HandleInvulnerableChange(bool isInvulnerable) {
        float frequency = isInvulnerable ? blinkFrequency : 0;
        material.SetFloat(blinkID, frequency);
    }

    // Change Nucleus color based on if dash is available
    void HandleDashReady(bool isDashReady) {
        Color c = nucleus.color;
        c.a = isDashReady ? 1f : 0.2f;
        nucleus.color = c;
    }


}
