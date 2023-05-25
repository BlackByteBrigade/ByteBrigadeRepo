using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    
    public int storedEnemyParts;
    public List<int> collectedEnemyParts = new List<int>();

    //States of the game
    private enum State{
        PlayingGameSceneHUB,
        PlayingGameSceneSkinWound,
        GameWin,
        GameOver,
    }

    private State gameState;

    private void Awake() {
        //Instance check of _GameManager
        if (Instance == null){     
            Instance = this;        
        } else {
            Destroy(gameObject);    
            return;                 
        }

        // do not destroy me when a new scene loads
        DontDestroyOnLoad(gameObject); 


        //game starts in main menu
        gameState = State.PlayingGameSceneHUB;
    }


    private void Update() {
        //Game flow of states
        switch(gameState){
            case State.PlayingGameSceneHUB:
                break;
            case State.PlayingGameSceneSkinWound:
                break;
            case State.GameWin:
                break;
            case State.GameOver:
                break;
        }
    }

    //Function below are scripts to check game state

    //Check if game is in Scene HUB
    public bool IsPlayingGameSceneHUB(){
        return gameState == State.PlayingGameSceneHUB;
    }

    //Check if game is in scene skin wound
    public bool IsPlayingGameSceneSkinWound(){
        return gameState == State.PlayingGameSceneSkinWound;
    }

    //Check if game is over
    public bool IsGameWin(){
        return gameState == State.GameWin;
    }

    //Check if game is over
    public bool IsGameOver(){
        return gameState == State.GameOver;
    }
}