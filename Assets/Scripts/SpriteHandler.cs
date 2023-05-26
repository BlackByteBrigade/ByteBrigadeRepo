using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpriteHandler : MonoBehaviour {
    
    [SerializeField] Cell owner;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] float glowDuration = 0.2f;
    [SerializeField] ParticleSystem vfx;
    [SerializeField] ParticleSystem deathVFXPrefab;
    Material material;
    const string glowID = "_Glow";



    void Awake() {
        CopyMaterial();
        owner.a_TakeDamage += HandleDamage;
        owner.OnDeath += HandleDeath;
    }

    void HandleDamage(int damageTaken) {
        if (damageTaken <= 0) return;
        vfx.Play();
        Flash();
    }

    void HandleDeath(Cell _) {
        ParticleSystem deathVFX = Instantiate(deathVFXPrefab);
        deathVFX.transform.position = transform.position;
        deathVFX.Play();
    }

    public void CopyMaterial() {
        Material copiedMaterial = new Material(renderer.material);
        renderer.material = copiedMaterial;
        material = copiedMaterial;
    }

    public void Flash() {
        material.SetFloat(glowID, 1f);
        material.DOFloat(0, glowID, glowDuration);
    }


}
