using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    //States of the game
    private enum State{
        IntroTutorial,
        PlayingGame,
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


        //game starts in IntroTutorial
        gameState = State.IntroTutorial;
    }


    private void Update() {
        //Game flow of states
        switch(gameState){
            case State.IntroTutorial:
                break;
            case State.PlayingGame:
                break;
            case State.GameWin:
                break;
            case State.GameOver:
                break;
        }
    }

    //Function below are scripts to check game state

    //Check if game is in Scene HUB
    public bool IsPlayingIntroTutorial(){
        return gameState == State.IntroTutorial;
    }

    //Check if game is in scene skin wound
    public bool IsPlayingGame(){
        return gameState == State.PlayingGame;
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