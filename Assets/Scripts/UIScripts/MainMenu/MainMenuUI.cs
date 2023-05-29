using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private string sceneToLoadWhenPlayIsClicked;

    [SerializeField] public Button playButton;
    [SerializeField] public Button exitButton;

    private Animator animator;
    
    //For Settings and credit buttons go to the MainMenuController script
    private void Awake() {
        
        //get animator
        animator = GetComponent<Animator>();

        //Play game
        playButton.onClick.AddListener(() => {
            //Play Animation of exit buttons
            animator.SetBool("Menu", false);
            //Play button loads game scene after timer
            startGame = true;
        });
        
        //Exit Game
        exitButton.onClick.AddListener(() => {   
            Application.Quit();
        });
    }

    //timer variables
    private bool startTimer = false;
    private bool startGame = false;
    private float timer = 0;
    private float timerMax = 1;     
    
    private void Update() {     
        if(startGame == true){
            timer += Time.deltaTime;
            if(timer >= timerMax){
                SceneManager.LoadScene(sceneToLoadWhenPlayIsClicked);
            }
        }
    }







}
