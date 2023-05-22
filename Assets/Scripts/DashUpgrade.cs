using System.Collections;
using UnityEngine;

public class DashUpgrade : DNAUpgrade
{
    public float movementSpeed;
    public float movementMaxSpeed;
    public float dashSpeed;
    public float dashTime;

    private float oldMovementSpeed;
    private float oldMovementMaxSpeed;
    private float oldDashSpeed;
    private float oldDashTime;

    public override void ApplyUpgrade(Player player)
    {
        oldMovementSpeed = player.Movement.movementSpeed;
        oldMovementMaxSpeed = player.Movement.movementMaxSpeed;
        oldDashSpeed = player.Movement.dashSpeed;
        oldDashTime = player.Movement.dashTime;

        player.Movement.movementSpeed = movementSpeed;
        player.Movement.movementMaxSpeed = movementMaxSpeed;
        player.Movement.dashSpeed = dashSpeed;
        player.Movement.dashTime = dashTime;
    }

    public override void RemoveUpgrade(Player player)
    {
        player.Movement.movementSpeed = oldMovementSpeed;
        player.Movement.movementMaxSpeed = oldMovementMaxSpeed;
        player.Movement.dashSpeed = oldDashSpeed;
        player.Movement.dashTime = oldDashTime;
    }
}