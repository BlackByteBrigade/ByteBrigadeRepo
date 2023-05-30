using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpleenDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text partCounter;

    int totalParts;
    int parts = -1;

    void Start() {
        totalParts = GameManager.Instance.totalEnemyPartsToBeCollectedTotal;
    }   

    void Update() {
        int p = GameManager.Instance.storedEnemyParts;
        if (p != parts) {
            parts = p;
            partCounter.text = $"{parts}/ {totalParts}";
        }
    }
}
