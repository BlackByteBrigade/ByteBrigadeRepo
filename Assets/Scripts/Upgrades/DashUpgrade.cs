using System.Collections;
using UnityEngine;

public class DashUpgrade : DNAUpgrade
{
    public GameObject infoText;
    public float textLastTime;

    public float movementSpeed;
    public float movementMaxSpeed;
    public float dashSpeed;
    public float afterDashSpeed;
    public float dashTime;
    public float dashCooldown;

    private float oldMovementSpeed;
    private float oldMovementMaxSpeed;
    private float oldDashSpeed;
    private float oldAfterDashSpeed;
    private float oldDashTime;
    private float oldDashCooldown;

    private void DestroyInfoText()
    {
        Destroy(infoText);
    }

    public override void ApplyUpgrade(Player player, bool firstTime)
    {
        if (firstTime)
        {
            Invoke(nameof(DestroyInfoText), textLastTime);
        }
        else
        {
            DestroyInfoText();
        }

        oldMovementSpeed = player.Movement.movementSpeed;
        oldMovementMaxSpeed = player.Movement.movementMaxSpeed;
        oldDashSpeed = player.Movement.dashSpeed;
        oldAfterDashSpeed = player.Movement.afterDashSpeed;
        oldDashTime = player.Movement.dashTime;
        oldDashCooldown = player.Movement.dashCooldown;

        player.Movement.movementSpeed = movementSpeed;
        player.Movement.movementMaxSpeed = movementMaxSpeed;
        player.Movement.dashSpeed = dashSpeed;
        player.Movement.afterDashSpeed = afterDashSpeed;
        player.Movement.dashTime = dashTime;
        player.Movement.dashCooldown = dashCooldown;
        player.Movement.isDashCancelable = false;
    }

    public override void RemoveUpgrade(Player player)
    {
        player.Movement.movementSpeed = oldMovementSpeed;
        player.Movement.movementMaxSpeed = oldMovementMaxSpeed;
        player.Movement.dashSpeed = oldDashSpeed;
        player.Movement.afterDashSpeed = oldAfterDashSpeed;
        player.Movement.dashTime = oldDashTime;
        player.Movement.dashCooldown = oldDashCooldown;
        player.Movement.isDashCancelable = true;
    }
}