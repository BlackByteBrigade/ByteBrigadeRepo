using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPartDropOff : MonoBehaviour
{
    [SerializeField] ParticleSystem vfx;
  
    public void Animate() {
        vfx.Play();
    }
}
