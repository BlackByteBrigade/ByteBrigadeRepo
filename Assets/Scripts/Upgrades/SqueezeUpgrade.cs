using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqueezeUpgrade : DNAUpgrade
{
    public float newSoftbodyFlex;
    public float newRadius;
    public PhysicsMaterial2D newMat;
    public GameObject infoText;
    public float textLastTime = 10;

    private float oldRadius;
    private float oldSoftbodyRadius;
    private float oldSoftbodyFlex;
    private PhysicsMaterial2D oldMat;


    private void Start()
    {
        Invoke(nameof(DestroyInfoText), textLastTime);
    }

    private void DestroyInfoText()
    {
        Destroy(infoText);
    }

    public override void ApplyUpgrade(Player player)
    {
        oldRadius = player.mainCollider.radius;
        oldSoftbodyFlex = player.Softbody.flex;
        oldSoftbodyRadius = player.Softbody.radius;
        oldMat = player.mainCollider.attachedRigidbody.sharedMaterial;

        player.mainCollider.radius = newRadius;
        player.Softbody.radius = newRadius;
        player.Softbody.flex = newSoftbodyFlex;
        player.mainCollider.attachedRigidbody.sharedMaterial = newMat;
    }

    public override void RemoveUpgrade(Player player)
    {
        player.mainCollider.radius = oldRadius;
        player.Softbody.flex = oldSoftbodyFlex;
        player.Softbody.radius = oldSoftbodyRadius;
        player.mainCollider.attachedRigidbody.sharedMaterial = oldMat;
    }
}
