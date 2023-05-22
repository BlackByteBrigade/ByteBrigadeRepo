using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public abstract class EnemyRanged : EnermyAttacking
{

    public GameObject Projectile;

    public void Fire()
    {
        //we call attack to handle cooldown
       Attack();
    }

   

    // Start is called before the first frame update
    void Start()
    {
        ((Enemy)this).Start();
    }

    // Update is called once per frame
    public void Update()
    {
        ((Enemy)this).Update();
    }
}
