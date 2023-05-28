using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpriteHandler : MonoBehaviour {
    
    [SerializeField] protected Cell owner;
    [SerializeField] protected SpriteRenderer renderer;
    [SerializeField] protected float glowDuration = 0.2f;
    [SerializeField] protected ParticleSystem vfx;
    [SerializeField] protected ParticleSystem deathVFXPrefab;

    protected Material material;
    protected const string glowID = "_Glow";



    protected virtual void Awake() {
        CopyMaterial();
        owner.OnDamageTaken += HandleDamage;
        owner.OnDeath += HandleDeath;
        Debug.Log("awake "+owner.name);
    }

    protected virtual void HandleDamage(int damageTaken) {
        if (damageTaken <= 0) return;
        vfx.Play();
        Flash();
    }

    protected virtual void HandleDeath(Cell _) {
        ParticleSystem deathVFX = Instantiate(deathVFXPrefab);
        deathVFX.transform.position = transform.position;
        deathVFX.Play();
    }

    protected void CopyMaterial() {
        Material copiedMaterial = new Material(renderer.material);
        renderer.material = copiedMaterial;
        material = copiedMaterial;
    }

    public virtual void Flash() {
        material.SetFloat(glowID, 1f);
        material.DOFloat(0, glowID, glowDuration);
    }


}
