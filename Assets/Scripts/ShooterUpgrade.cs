using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShooterUpgrade : DNAUpgrade
{
    public float spread = 30f;
    public int bulletNum = 3;
    public float shootInterval = 1f;
    public GameObject projectilePrefab;

    private bool canShoot = true;

    void Update()
    {
        if (canShoot && Input.GetMouseButton(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        canShoot = false;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;

        AudioManager.instance.PlaySfX(SoundEffects.PewPew);

        for (int i = 0; i < bulletNum; i++)
        {
            GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            proj.transform.up = Quaternion.Euler(0, 0, spread * (i - bulletNum / 2)) * (Camera.main.ScreenToWorldPoint(mousePos) - transform.position).normalized;
        }

        Invoke(nameof(Reload), shootInterval);
    }

    private void Reload()
    {
        canShoot = true;
    }

    public override void ApplyUpgrade(Player player)
    {

    }

    public override void RemoveUpgrade(Player player)
    {

    }
}
