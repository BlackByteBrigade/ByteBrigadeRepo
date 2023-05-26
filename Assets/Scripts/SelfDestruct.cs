using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float timeUntilDeath = 1f;
    public bool paused = false;

    float t;

    void Update() {
        if (paused) return;
        t += Time.deltaTime;
        if (t >= timeUntilDeath)
            Destroy(gameObject);
    }
}
