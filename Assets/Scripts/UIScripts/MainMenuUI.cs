using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    private void Awake() {
        playButton.onClick.AddListener(() => {
            //Play game

        });
        settingsButton.onClick.AddListener(() => {
            //game settings

        });
        quitButton.onClick.AddListener(() => {
            //Exit Game
            Application.Quit();
        });

    }

}
