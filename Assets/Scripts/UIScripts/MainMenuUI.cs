using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button creditButton;

    private void Awake() {
        playButton.onClick.AddListener(() => {
            //Play game
            Debug.Log("Play Game");
        });
        settingsButton.onClick.AddListener(() => {
            //game settings
            Debug.Log("settings");
        });
        exitButton.onClick.AddListener(() => {
            //Exit Game
            Application.Quit();
            Debug.Log("Quit Game");
        });
        creditButton.onClick.AddListener(() => {
            //Game credits
            Debug.Log("Game credits");
        });
    }

}
