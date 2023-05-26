using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


// Author: Pascal
// TEMP script

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] Image healthbarFillImage;
    [SerializeField] TMP_Text plasmaCounter;

    public static PlayerHUD instance;

    // TEMP: counts plasma coins collected
    int plasma;

    void Awake() {

        // SINGLETON
        if (instance == null) {
            instance = this;
            UpdateHealthbar(1f);
        } else {
            Destroy(gameObject); 
            return;            
        }
    }


    public void UpdateHealthbar(float value) {
        healthbarFillImage.fillAmount = value;
    }

    public void AddPlasma(int amount) {
        plasma += amount;
        plasmaCounter.text = plasma.ToString();
    }
}
