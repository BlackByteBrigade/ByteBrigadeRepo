using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyRanged : Enemy
{

    public GameObject Projectile;
    public int RangedDmg;

    public abstract void Fire();

   

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
