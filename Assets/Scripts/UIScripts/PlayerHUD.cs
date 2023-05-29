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
    [SerializeField] TMP_Text healReminderText;
    public GameObject plasmaObject;
    public static PlayerHUD instance;
    public float test = 0.2f;
    public float magnitude = 1f;
    public float shackingduretion = 1f;
    private bool isShaking = false;
    Vector3 originalPos ;
    bool blinkon = false;



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
        healReminderText.color = new Vector4(255,255,255,0);

    }
    private void Update()
    {
        renderHud();
    }

    public void UpdateHealthbar(float value)
    {
        playerHealth = value;
    }

    private void renderHud()
    {
        healthbarFillImage.fillAmount = Mathf.Lerp(healthbarFillImage.fillAmount, playerHealth, healthbarFillSpeed * Time.deltaTime);
        plasmaCounter.text = GameManager.Instance.storedplasmaCoins.ToString();
        //partCounter.text = part.ToString()+" / "+totalParts.ToString()+"  ";
        plasmaCounter.fontSize = Mathf.Lerp(plasmaCounter.fontSize, 36, healthbarFillSpeed * Time.deltaTime);
        if(isShaking)
        {
            shacking();
        }
        healingReminder();
    }

    public void AddPlasma(int amount)
    {
        GameManager.Instance.storedplasmaCoins += amount;
        plasmaCounter.fontSize = 70;
        
    }

    public void deletePlasma(int amount)
    {
        GameManager.Instance.storedplasmaCoins -= amount;
        isShaking = true;
        if (GameManager.Instance.storedplasmaCoins < 0) { GameManager.Instance.storedplasmaCoins = 0; }
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
            plasmaCounter.text = GameManager.Instance.storedplasmaCoins.ToString();
            elapsed += Time.deltaTime;
        }
        else
        {
            elapsed = 0;
            isShaking = false;
            plasmaObject.transform.position = originalPos;
        }
        
    }

    public void healingReminder()
    {
        float alpha = 0f;
        float blinkspeed = 3f;
        
        if(GameManager.Instance.storedplasmaCoins > 0 && playerHealth<=0.3f)
        {
            if (blinkon)
            {
                alpha=Mathf.Lerp(healReminderText.color.a, 1, blinkspeed * Time.deltaTime);
                healReminderText.color = new Vector4(255, 255, 255, alpha);
                if (healReminderText.color.a >= 0.9f)
                {
                    blinkon = false;
                }
            }
            else
            {
                alpha=Mathf.Lerp(healReminderText.color.a, 0, blinkspeed * Time.deltaTime);
                healReminderText.color = new Vector4(255, 255, 255, alpha);
                if (healReminderText.color.a <= 0.1f)
                {
                    blinkon = true;
                }
                
            }
            
        }
        else
        {
            healReminderText.color = new Vector4(255, 255, 255, 0);
        }
    }
}

