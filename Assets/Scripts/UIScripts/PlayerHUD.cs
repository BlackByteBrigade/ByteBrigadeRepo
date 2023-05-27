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
    [SerializeField] float healthbarFillSpeed = 3f; //i.e lerp speed
    [SerializeField] float playerHealth = 0f;
    [SerializeField] TMP_Text partCounter;
    public GameObject plasmaObject;
    public static PlayerHUD instance;
    public float test = 0.2f;
    public float magnitude = 1f;
    public float shackingduretion = 1f;
    private bool isShaking = false;
    Vector3 originalPos ;

    // TEMP: counts plasma coins collected
    int plasma =0;
    public float elapsed = 0f;
    // TEMP: counts parts collected
    int part=0;
    int totalParts = 8;

    void Awake()
    {

        // SINGLETON
        if (instance == null)
        {
            instance = this;
            UpdateHealthbar(1f);
            
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }

    private void Start()
    {
        originalPos = plasmaObject.transform.position;
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            test += 0.1f;
            UpdateHealthbar(test);
            AddPlasma(1);
            addPart(1);
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            test -= 0.1f;
            UpdateHealthbar(test);
            deletePlasma(1);
            deletePart(1);
        }
        renderHud();
    }

    public void UpdateHealthbar(float value)
    {
        playerHealth = value;
    }

    private void renderHud()
    {
        healthbarFillImage.fillAmount = Mathf.Lerp(healthbarFillImage.fillAmount, playerHealth, healthbarFillSpeed * Time.deltaTime);
        plasmaCounter.text = plasma.ToString();
        partCounter.text = part.ToString()+" / "+totalParts.ToString()+"  ";
        plasmaCounter.fontSize = Mathf.Lerp(plasmaCounter.fontSize, 36, healthbarFillSpeed * Time.deltaTime);
        if(isShaking)
        {
            shacking();
        }
    }

    public void AddPlasma(int amount)
    {
        plasma += amount;
        plasmaCounter.fontSize = 70;
        
    }

    public void deletePlasma(int amount)
    {
        plasma -= amount;
        isShaking = true;
        if (plasma < 0) { plasma = 0; }
    }
    public void addPart(int amount)
    {
        part += amount;
        if (part>totalParts) part = totalParts;
    }

    public void deletePart(int amount)
    {
        part -= amount;
        if (part < 0) part = 0;
    }

    public void shacking() { 
        
        
        if (elapsed<shackingduretion)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            plasmaObject.transform.position = new Vector3(x,y,0)+originalPos;
            plasmaCounter.text = plasma.ToString();
            elapsed += Time.deltaTime;
        }
        else
        {
            elapsed = 0;
            isShaking = false;
            plasmaObject.transform.position = originalPos;
        }
        
        

        
    }
}

