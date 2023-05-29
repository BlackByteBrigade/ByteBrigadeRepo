using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int totalEnemyPartsOnPlayer = 0;
    public int totalEnemyPartsToBeCollectedTotal = 20; //todo !!! set this !!!

    public int storedEnemyParts;
    public List<int> collectedEnemyParts = new List<int>();

    public int storedplasmaCoins;

    public int NumberOfEnemiesFocusedOnPlayer { get; set; }

    //States of the game
    private enum State
    {
        IntroTutorial,
        PlayingGame,
        Paused,
        GameWin,
        GameOver,
    }

    //game starts in IntroTutorial
    private State gameState = State.IntroTutorial;

    private string _lastLoadedLevel { get; set; }
    private DateTime _lastLoadedLevelDateTime { get; set; }
    private Scene _currScene { get; set; }

    #region Narration

    public bool narrationFirstTimeMainScene { get; set; }
    public bool narrationHasSeenEnemyPart { get; set; }
    public bool narrationHasPickedUpEnemyPart { get; set; }

    public bool narrationHasSeenPowerUp { get; set; }
    public bool narrationHasTakenWorldDmg { get; set; }

    private DateTime _lastEnemyPartDropOff { get; set; }
    private short _lastEnemyPartDropOffNarrationPlayed = 0;

    #endregion

    public IEnumerator PlayerEntersMainScene()
    {
        gameState = State.PlayingGame;
        if (!narrationFirstTimeMainScene)
        {
            narrationFirstTimeMainScene = true;
            Player.instance.Movement.enabled = false;
            Player.instance.Movement.Body.velocity = Vector3.zero;
            Debug.Log("a");
            yield return new WaitForSeconds(AudioManager.instance.PlayNarration("narrationFirstTimeMainScene"));
            Player.instance.Movement.enabled = true;
        }
    }

    public void PlayerCloseToEnemyPart()
    {
        if (narrationHasSeenEnemyPart)
            return;

        narrationHasSeenEnemyPart = true;
        AudioManager.instance.PlayNarration("narrationFirstSeenEnemyPart");
    }

    public void EnemyDropsPowerUp()
    {
        if (narrationHasSeenPowerUp)
            return;

        narrationHasSeenPowerUp = true;
        AudioManager.instance.PlayNarration("narrationFirstSeenPowerUp");
    }

    public void PlayerTakesWorldDmg()
    {
        if (narrationHasTakenWorldDmg)
            return;
        narrationHasTakenWorldDmg = true;
        AudioManager.instance.PlayNarration("narrationFirstTakenWorldDmg");
    }

    public void PlayerPicksUpEnemyPart()
    {
        //Play sound effect
        AudioManager.instance.PlaySfX(SoundEffects.CollectingDna);

        if (narrationHasPickedUpEnemyPart)
            return;
        narrationHasPickedUpEnemyPart = true;
        //Invoke("MyFunction", 3);
        AudioManager.instance.PlayNarration("narrationFirstPickedUpEnemyPart");
    }

    public void PlayerDropsOffEnemyParts(int amount)
    {
        //Add to number of part dropped off
        storedEnemyParts += amount;
        totalEnemyPartsOnPlayer -= amount;
        Debug.Log($"EnermyParts stored In T-Cell: {storedEnemyParts}");

        //check if all enemy parts have been collected
        if (storedEnemyParts >= totalEnemyPartsToBeCollectedTotal)
        {
            Debug.Log("Game Complete");
            //disable HUD
            var playerHud = GameObject.Find("PlayerHUD");
            playerHud.SetActive(false);

            // Game over; player has collected all enemy parts
            AudioManager.instance.PlayNarration($"narrationLastDropOffEnemyPart");
            gameState = State.GameWin;
            GameObject.Find("VideoManager").GetComponent<VideoPlayer>().PlayVictoryVideo();
            return;
        }

        if (amount>0 && (_lastEnemyPartDropOff == default || (DateTime.Now - _lastEnemyPartDropOff).TotalMinutes > 1))
        {
            //Play sound effect
            AudioManager.instance.PlaySfX(SoundEffects.DropingOffEnememyParts);

            if (narrationHasPickedUpEnemyPart)
                return;
            narrationHasPickedUpEnemyPart = true;

            //loop narrations
            AudioManager.instance.PlayNarration($"narrationDropOffEnemyPart_{++_lastEnemyPartDropOffNarrationPlayed}");
            if (_lastEnemyPartDropOffNarrationPlayed >= 4)
                _lastEnemyPartDropOffNarrationPlayed = 0;
        }
    }

   

    private void Awake()
    {
        //Instance check of _GameManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // do not destroy me when a new scene loads
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _currScene = SceneManager.GetActiveScene();
        //HandleCurrentScene();
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        _currScene = SceneManager.GetActiveScene();
        if (!HasHandledCurrLevelChange())
            HandleCurrentScene();
    }

    private bool HasHandledCurrLevelChange()
    {
        //OnLevelWasLoaded is triggered multiple times, this makes sure that we only handle it once
        //first we check if the scene name has changed, if so the case is clear
        if (!_currScene.name.Equals(_lastLoadedLevel))
        {
            _lastLoadedLevel = _currScene.name;
            _lastLoadedLevelDateTime = DateTime.Now;
            return false;
        }

        //otherwise we check if enough time has passed that we can consider it a scene transition to the same scene (but different location?)
        if ((DateTime.Now - _lastLoadedLevelDateTime).TotalSeconds > 10)
        {
            _lastLoadedLevel = _currScene.name;
            _lastLoadedLevelDateTime = DateTime.Now;
            return false;
        }

        return true;
    }


    public void HandleCurrentScene()
    {
        switch (_currScene.name.ToLower())
        {
            case "mainscene":
            case "coderedtest":
                AudioManager.instance.PlayRegularVolumeAreaMusic(true);
                StartCoroutine(PlayerEntersMainScene());
                break;
            case "spleenhub":
                AudioManager.instance.PlayMusic();
                break;
        }
    }


    private void Update()
    {
        //Game flow of states
        switch (gameState)
        {
            case State.IntroTutorial:
                break;
            case State.PlayingGame:
                break;
            case State.GameWin:
                break;
            case State.GameOver:
                break;
            case State.Paused:
                break;
        }
    }

    //Function below are scripts to check game state

    //Check if game is in Scene HUB
    public bool IsPlayingIntroTutorial => gameState == State.IntroTutorial;

    //Check if game is in scene skin wound
    public bool IsPlayingGame => gameState == State.PlayingGame;

    //Check if game is over
    public bool IsGameWin => gameState == State.GameWin;

    //Check if game is over
    public bool IsGameOver => gameState == State.GameOver;

    //Check if game is paused
    public bool IsPaused => gameState == State.Paused;


    public void RegisterEnemyNoticed()
    {
        //for the tutorial we don't want to play combat music when the player engages the 2 "dummy" enemies
        if (gameState != State.PlayingGame)
        {
            return;
        }

        if (NumberOfEnemiesFocusedOnPlayer < 0)
            NumberOfEnemiesFocusedOnPlayer = 0;
        if (NumberOfEnemiesFocusedOnPlayer == 0)
            AudioManager.instance.PlayLoudAreaMusic();
        NumberOfEnemiesFocusedOnPlayer++;
    }

    public void UnregisterNoticedEnemy()
    {
        if (NumberOfEnemiesFocusedOnPlayer == 1)
            AudioManager.instance.PlayRegularVolumeAreaMusic();
        NumberOfEnemiesFocusedOnPlayer--;
    }
}