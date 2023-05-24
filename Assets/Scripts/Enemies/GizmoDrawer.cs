using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GizmoDrawer : MonoBehaviour
{
    [SerializeField] DrawMode mode = DrawMode.WhenSelected;
    [SerializeField] Enemy enemy;
    [SerializeField] bool drawReactionDistance = true;
    [SerializeField] bool drawAlarmDistance = true;

    public enum DrawMode {
        None = 0, WhenSelected = 1, Always = 2
    }


    void OnDrawGizmos() {
        if (mode == DrawMode.Always) Draw();
    }

    void OnDrawGizmosSelected() {
        if (mode == DrawMode.WhenSelected) Draw();
    }


    void Draw() {
        if (drawReactionDistance) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemy.ReactsToPlayerDistance);
        }

        if (drawAlarmDistance) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, enemy.AlarmedByPlayerDistance);
        }
    }

}
