using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wurmBodyCollid : MonoBehaviour
{
    public EnemyWurm parent;
    // Start is called before the first frame update
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        parent.RegisterCollisionFromBody(collision);
    }

}
