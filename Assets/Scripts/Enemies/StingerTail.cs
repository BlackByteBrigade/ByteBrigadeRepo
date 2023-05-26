using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StingerTail : MonoBehaviour
{

    [System.Serializable]
    public struct TentacleParameters {
        public float amplitude;
        public float speed;
        public float frequency;
        public float stretch;
    }

    [SerializeField] TentacleParameters parameters_Idle;
    [SerializeField] TentacleParameters parameters_WindUp;
    [SerializeField] TentacleParameters parameters_Dash;


    [SerializeField] Axis alongAxis = Axis.X;
    

    LineRenderer line;
    int numSegments = 1;
    Vector3[] segmentPositions;
    float dt = 0;
    TentacleParameters parameters;

    public enum Axis {X = 0, Y = 1 }


    void Start() {
        parameters = parameters_Idle;
        line = GetComponent<LineRenderer>();
        numSegments = line.positionCount;
        segmentPositions = new Vector3[numSegments];
        line.GetPositions(segmentPositions);
    }



    void Update() {

        dt += Time.deltaTime * parameters.speed;

        for (int i = 0; i < numSegments; i++) {
            Vector3 pos = segmentPositions[i]; //line.GetPosition(i);
            float ratio = (float)i / (numSegments - 1);
            float scale = Mathf.Pow(ratio, 0.5f);
            float t = ratio - dt;

            float x,y;

            if (alongAxis == Axis.X) {
                x = pos.x * parameters.stretch;
                y = Mathf.Sin(t * parameters.frequency * Mathf.PI * 2f) * parameters.amplitude* scale;
            } else {
                x = Mathf.Sin(t * parameters.frequency * Mathf.PI * 2f) * parameters.amplitude* scale;
                y = pos.y * parameters.stretch;
            }

            line.SetPosition(i, new Vector3(x, y, 0f));
        }
    }



    public void SetIdle() {
        parameters = parameters_Idle;
    }

    public void WindUp() {
        parameters = parameters_WindUp;
    }

    public void Dash() {
        parameters = parameters_Dash;
    }
}
