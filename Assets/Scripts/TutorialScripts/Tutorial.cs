using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class Tutorial : MonoBehaviour
{
    
    
    //use to start tutorial mode on and off
    public bool gameBeginsStartTutorial = false;

    //index for steps in tutorial
    private int index = 0;

    //pause tutorial between steps. reduces checks for lag.
    private bool pauseTutorial = false;

    //tutorial dialogue
    private bool firstTimeDialoguePlaying = true;
    
    //enemys to battle with
    [SerializeField] private GameObject firstEnemy;
    [SerializeField] private GameObject secondEnemy;

    //Narration wait times
    [SerializeField] private float waitTimeBeforeStartingNarrationOne;
    [SerializeField] private float narrationOneWaitTime;
    [SerializeField] private float narrationTwoWaitTime;
    [SerializeField] private float narrationThreeWaitTime;
    [SerializeField] private float narrationFourWaitTime;
    [SerializeField] private float narrationFiveWaitTime;
    [SerializeField] private float narrationSixWaitTime;
    [SerializeField] private float narrationSevenWaitTime;
    [SerializeField] private float narrationEightWaitTime;
    [SerializeField] private float narrationNineWaitTime;

    //tutorial text boxes
    [SerializeField] private GameObject textBox1;
    [SerializeField] private GameObject textBox2;
    [SerializeField] private GameObject textBox3;

    //allows player to leave
    [SerializeField] private GameObject bloodVesselEntrance;

    //camera traget in opening tutorial
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject TargetForCameraOpenTutorial;
    


    // Start is called before the first frame update
    private void Start()
    {   
        //Make sure all parts are deactivated. Will bring them in order.
        firstEnemy.SetActive(false);
        secondEnemy.SetActive(false);

    }

    private void Update() {
        //check if tutorial mode is needed
        if(GameManager.Instance.IsPlayingIntroTutorial == true){
            gameBeginsStartTutorial = true;
        }

        //Fade in from Black screen to the scene  
        if(fadeIn ==true){
            FadeIn();
        }

        //timer for one of the index steps
        if(startTimer == true){
            StopClock();
        }

        //check if the game needs to run the tutorial
        if(gameBeginsStartTutorial == true){

            //check if need ready for next steps in tutorial. reduces lag.
            if(startTimer == false || pauseTutorial == true){
                //start or continue tutorial steps
                IntroTutorial();
            }
        }
    }


    private void IntroTutorial(){
        // [Black Screen]
        if (index == 0){
            //pause at beginning before jumping straight into voice
            timerMax = waitTimeBeforeStartingNarrationOne;
            var cinemachineCamera = camera.GetComponent<CinemachineVirtualCamera>();
            cinemachineCamera.Follow = TargetForCameraOpenTutorial.transform;
            startTimer = true;

        }

        //Voice narrator: “today we will have a look at...”
        if (index == 1){
            //if tutorial player cannot leave spleen
            if(gameBeginsStartTutorial == true){
                bloodVesselEntrance.SetActive(false);
            }
            
            //play narration
            string narrationStringName = "NarraionOne";
            AudioManager.instance.PlayNarration(narrationStringName);
            //wait till narration is finished
            timerMax = narrationOneWaitTime;
            startTimer = true;
        }
        
        //[fades in, we see the hub area]
        if (index == 2){
            FadeIn();
        }
        
        //Voice narrator: “The Immune System, a bottomless pit of complexity; though from the view of a cell, life is essentially in 2 Dimensions.”
        if (index == 3){
            //play narration
            string narrationStringName = "NarraionTwo";
            AudioManager.instance.PlayNarration(narrationStringName);
            //wait till narration is finished
            timerMax = narrationTwoWaitTime;
            startTimer = true;
        }
        
        //Voice narrator: “We begin our journey in the Spleen; here Dendritic cells deposit a snapshot from an infection site; a battleground so to speak.”
        if (index == 4){
            //play narration
            string narrationStringName = "NarraionThree";
            AudioManager.instance.PlayNarration(narrationStringName);
            //wait till narration is finished
            timerMax = narrationThreeWaitTime;
            startTimer = true;
        }
        
        //[Zooms out]
        if (index == 5){
            //TODO; zoom - start camera
            index++;
        }        
        
        //Voice narrator: “Now where is out little fella?”
        if (index == 6){
            //play narration
            string narrationStringName = "NarraionFour";
            AudioManager.instance.PlayNarration(narrationStringName);
            //wait till narration is finished
            timerMax = narrationFourWaitTime;
            startTimer = true;           
        }        
        

        //[INPUT PROMT: “Press Space to Dash”]
        if (index == 7){
             
            if(firstTimeDialoguePlaying == true){
                //show dash text
                textBox1.SetActive(true);
                firstTimeDialoguePlaying = false;
            }
            //Space bar ends dialogue, go to next step
            if(Input.GetKeyDown(KeyCode.Space)){
                //camera follow player now
                var player = GameObject.Find("Player");
                var cinemachineCamera = camera.GetComponent<CinemachineVirtualCamera>();
                cinemachineCamera.Follow = player.transform;
                //hide Dash text
                textBox1.SetActive(false);
                //resets
                firstTimeDialoguePlaying = true;
                index++;
            }
        }
        
        //[Dendritic cell appears from the right]
        if (index == 8){
            index++; 
        }        
        
        //Voice narrator: “Ah there he is! Watch out little one, you could hurt someone with that!”
        if (index == 9){
            //play narration
            string narrationStringName = "NarraionFive";
            AudioManager.instance.PlayNarration(narrationStringName);
            //wait till narration is finished
            timerMax = narrationFiveWaitTime;
            startTimer = true;
        }        
        
        //[An Enemy (base enemy) appears]
        if (index == 10){
            //first enemy appears
            firstEnemy.SetActive(true);
            var cinemachineCamera = camera.GetComponent<CinemachineVirtualCamera>();
            cinemachineCamera.Follow = firstEnemy.transform;
            index++;
        }        
        
        //Voice narrator: “This is very odd! Normally we don’t see [ENEMY] here; quickly get rid of him!”
        if (index == 11){
            //play narration
            string narrationStringName = "NarraionSix";
            AudioManager.instance.PlayNarration(narrationStringName);
            //wait till narration is finished
            timerMax = narrationSixWaitTime;
            startTimer = true;
        }
        
        //Tutorial text: “Move using WSAD; use Dash (space) to kill the enemy.”
        if (index == 12){
            if(firstTimeDialoguePlaying == true){
                textBox2.SetActive(true);
                firstTimeDialoguePlaying = false;

                //look back to player from enemy 1
                var player = GameObject.Find("Player");
                var cinemachineCamera = camera.GetComponent<CinemachineVirtualCamera>();
                cinemachineCamera.Follow = player.transform;
            }
            //Kill first enemy
            if(firstEnemy == null){
                textBox2.SetActive(false);
                index++;

                //reset firstTimeDialoguePlaying for next dialogue
                firstTimeDialoguePlaying = true;
            }
        }
        
        //[Player kills enemy]
        if (index == 13){
            Debug.Log("kill enemy");
            index++;            
        }
        
        //Voice narrator: “Very good! Now with that taken care of.....”
        if (index == 14){
            //play narration
            string narrationStringName = "NarraionSeven";
            AudioManager.instance.PlayNarration(narrationStringName);
            //wait till narration is finished
            timerMax = narrationSevenWaitTime;
            startTimer = true;
       
        }
        
        //[Another Enemy (dasher) Appears]
        if (index == 15){
            index++;
            //second enemy appears
            secondEnemy.SetActive(true);

            //look at second enemy
            var cinemachineCamera = camera.GetComponent<CinemachineVirtualCamera>();
            cinemachineCamera.Follow = secondEnemy.transform;
        }
        
        //Voice narrator: “Ill be damned; both Bacterial and viruses at the same time; usually these two are arch enemies! Take care of him; but be careful; this one seems a lot more capable than the last one!”
        if (index == 16){
            //play narration
            string narrationStringName = "NarraionEight";
            AudioManager.instance.PlayNarration(narrationStringName);
            //wait till narration is finished
            timerMax = narrationEightWaitTime;
            startTimer = true;         
        }
        
        //[Player Kills Enemy]
        if (index == 17){
            if(firstTimeDialoguePlaying == true){
                textBox2.SetActive(true);
                firstTimeDialoguePlaying = false;

                //look back to player from enemy 2
                var player = GameObject.Find("Player");
                var cinemachineCamera = camera.GetComponent<CinemachineVirtualCamera>();
                cinemachineCamera.Follow = player.transform;
            }
            //Kill first enemy
            if(secondEnemy == null){
                textBox2.SetActive(false);
                index++;

                //reset firstTimeDialoguePlaying for next dialogue
                firstTimeDialoguePlaying = true;
            }         
        }
        
        //Voice narrator: “Well done! Well done! Now quickly, go and find out what causes this; there must be a source of the infection somewhere!”
        if (index == 18){
            //play narration
            string narrationStringName = "NarraionNine";
            AudioManager.instance.PlayNarration(name);
            //wait till narration is finished
            timerMax = narrationNineWaitTime;
            startTimer = true;                   
        }
        
        //Tutorial Text: “Use the blood vessel to get to investigate the source of infection.”
        if (index == 19){
            if(firstTimeDialoguePlaying == true){
                textBox3.SetActive(true);
                firstTimeDialoguePlaying = false;
                //allows player to leave
                bloodVesselEntrance.SetActive(true);
            }
            //Stops the tutorial
            gameBeginsStartTutorial = false;
        }
    }




    // Canvas fade in variables
    [SerializeField] private CanvasGroup fadeInCanvasGroup;
    [SerializeField] private float TimeToFadeIn;
    private bool fadeIn = false; 

    //timer variables
    private bool startTimer = false;
    private float timer = 0;
    private float timerMax; 

    //StopClock till next step in Tutorial
    private void StopClock()
    {
        timer += Time.deltaTime;
        if(timer >= timerMax){
            //turn off timer
            startTimer = false;
            //reset timer
            timer = 0;
            //increase index to go to next step in tutorial
            index++;
        }
    }

    //Fade in from Black screen to the scene 
    private void FadeIn()
    {
        fadeInCanvasGroup.alpha -= TimeToFadeIn * Time.deltaTime;
        if (fadeInCanvasGroup.alpha == 0)
        {
            fadeIn = false;
            index++;
        }
    }

}
