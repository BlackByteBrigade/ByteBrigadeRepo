using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyPartCollection : MonoBehaviour
{
    //EnemyPart counting variables
    private const string ENEMY_PART_TAG = "EnemyPart";
    private int enemyPartCollected = 0;
    private int totalEnemyPartsToBeCollected;


    //UI link to change enemy part counter
    [SerializeField] private TMP_Text enemyPartUICounter;


    private void Start() 
    {
        CountNumberofEnemyPartsInLevel();
        UpdateEnemyPartUICounter();
    }

    //Collision detection of enemy part
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag(ENEMY_PART_TAG))
        {
            Destroy(collision.gameObject);
            enemyPartCollected++;
            AudioManager.instance.PlaySfX(SoundEffects.CollectingEnemyPart);
        }

        UpdateEnemyPartUICounter();
    }

    private void CountNumberofEnemyPartsInLevel(){
        GameObject[] enemyParts = GameObject.FindGameObjectsWithTag(ENEMY_PART_TAG);
        foreach (GameObject enemyPart in enemyParts){
            totalEnemyPartsToBeCollected++;
        }
    }

    //updates the counter on the player HUD for enemy parts
    private void UpdateEnemyPartUICounter()
    {
        enemyPartUICounter.text = enemyPartCollected + "/" + totalEnemyPartsToBeCollected;
    }
}
