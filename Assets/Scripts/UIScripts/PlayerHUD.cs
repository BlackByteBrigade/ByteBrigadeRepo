using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Author: Pascal
// TEMP script

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] Image healthbarFillImage;

    public static PlayerHUD instance;

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
}
