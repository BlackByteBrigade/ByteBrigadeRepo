using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private string sceneToLoadWhenPlayIsClicked;

    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;
    
    //For Settings and credit buttons go to the MainMenuController script
    private void Awake() {
        //Play game
        playButton.onClick.AddListener(() => {
            //Play Animation of exit buttons
            
            //Play button loads game scene
            SceneManager.LoadScene(sceneToLoadWhenPlayIsClicked);
        });
        
        //Exit Game
        exitButton.onClick.AddListener(() => {
            
            Application.Quit();
        });
        
        //Game credits
    }

}
