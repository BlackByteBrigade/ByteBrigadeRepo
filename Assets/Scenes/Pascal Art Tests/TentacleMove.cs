using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleMove : MonoBehaviour
{
    [SerializeField] float amplitude = 1f;
    [SerializeField] float frequency = 1f;
    [SerializeField] float speed = 1f;
    

    LineRenderer line;
    int numSegments = 1;
    float dt = 0;



    void Start() {
        line = GetComponent<LineRenderer>();
        numSegments = line.positionCount;
    }

    void Update() {

        dt += Time.deltaTime * speed;

        for (int i = 0; i < numSegments; i++) {
            Vector3 pos = line.GetPosition(i);
            float _i = (float)i / (numSegments - 1);
            float t = _i + dt;
            float x = Mathf.Sin(t * frequency * Mathf.PI * 2f) * amplitude* Mathf.Pow(_i,0.5f);
            // float y = Mathf.Sin(t * frequency * Mathf.PI * 2f) * amplitude;
            float y = pos.y;

            line.SetPosition(i, new Vector3(x, y, 0f));
        }
    }
}
