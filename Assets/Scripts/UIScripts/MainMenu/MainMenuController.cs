using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    //Main Menu buttons
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditButton;
    // Settings menu
    [SerializeField] private Button settingsBackButton;
    // credit menu
    [SerializeField] private Button creditBackButton;


    private Animator animator;

    private void Awake() {
        //get animator
        
        animator = GetComponent<Animator>();
        
        //move from Main Menu to settings
        settingsButton.onClick.AddListener(() => {
            //Play animation of Main Menu exit
            animator.SetBool("Menu", false);
            //Play animation of settings entrance
            animator.SetBool("Settings", true);
        });

        //move from Main Menu to credits
        creditButton.onClick.AddListener(() => {
            //Play animation of Main Menu exit
            animator.SetBool("Menu", false);
            //Play animation of credits entrance
            animator.SetBool("Credits", true);
        });

        //move from settings to Main Menu
        settingsBackButton.onClick.AddListener(() => {
            //Play animation of settings exit
            animator.SetBool("Settings", false);
            //Play animation of Main Menu entrance
            animator.SetBool("Menu", true);
            
        });

        //move from credits to Main Menu 
        creditBackButton.onClick.AddListener(() => {
            //Play animation of credit exit
            animator.SetBool("Credits", false);
            //Play animation of Main Menu entrance
            animator.SetBool("Menu", true);
            
        });
    }

}
