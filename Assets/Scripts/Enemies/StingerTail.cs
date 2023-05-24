using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StingerTail : MonoBehaviour
{
    [SerializeField] float amplitude = 1f;
    [SerializeField] float frequency = 1f;
    [SerializeField] float speed = 1f;
    [SerializeField] bool alongXAxis = true;
    

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
            float ratio = (float)i / (numSegments - 1);
            float scale = Mathf.Pow(ratio, 0.5f);
            float t = ratio - dt;

            float x,y;

            if (alongXAxis) {
                x = Mathf.Sin(t * frequency * Mathf.PI * 2f) * amplitude* scale;
                y = pos.y;
            } else {
                x = pos.x;
                y = Mathf.Sin(t * frequency * Mathf.PI * 2f) * amplitude* scale;
            }

            line.SetPosition(i, new Vector3(x, y, 0f));
        }
    }
}
