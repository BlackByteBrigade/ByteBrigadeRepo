using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqueezeUpgrade : DNAUpgrade
{
    public float newSoftbodyFlex;
    public float newRadius;

    private float oldRadius;
    private float oldSoftbodyRadius;
    private float oldSoftbodyFlex;

    public override void ApplyUpgrade(Player player)
    {
        oldRadius = player.mainCollider.radius;
        oldSoftbodyFlex = player.Softbody.flex;
        oldSoftbodyRadius = player.Softbody.radius;

        player.mainCollider.radius = newRadius;
        player.Softbody.radius = newRadius;
        player.Softbody.flex = newSoftbodyFlex;
    }

    public override void RemoveUpgrade(Player player)
    {
        player.mainCollider.radius = oldRadius;
        player.Softbody.flex = oldSoftbodyFlex;
        player.Softbody.radius = oldSoftbodyRadius;
    }
}
