using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private string sceneToLoadWhenPlayIsClicked;

    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button creditButton;

    private void Awake() {
        //Play game
        playButton.onClick.AddListener(() => {
            //Play Animation of exit buttons
            
            //Play button loads game scene
            SceneManager.LoadScene(sceneToLoadWhenPlayIsClicked);
        });
        //game settings
        settingsButton.onClick.AddListener(() => {
            
            
            
        });
        //Exit Game
        exitButton.onClick.AddListener(() => {
            
            Application.Quit();
        });
        
        //Game credits
        creditButton.onClick.AddListener(() => {
            //Play animation of menu button exit

            //pause

            //play entrance of credits 
            
            
        });
    }

}
