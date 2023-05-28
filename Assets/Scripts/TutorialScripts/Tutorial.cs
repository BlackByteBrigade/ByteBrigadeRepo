using System.Collections;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    //use to start tutorial mode on and off
    public bool gameBeginsStartTutorial = false;


    //enemys to battle with
    [SerializeField] public GameObject firstEnemy;
    [SerializeField] public GameObject secondEnemy;
    [SerializeField] public GameObject EnemySpawnPos;

    //Narration wait times
    [SerializeField] private float waitTimeBeforeStartingNarrationOne;


    //tutorial text boxes
    [SerializeField] private GameObject textBox1;
    [SerializeField] private GameObject textBox2;
    [SerializeField] private GameObject textBox3;

    //allows player to leave
    [SerializeField] private GameObject bloodVesselEntrance;


    // Start is called before the first frame update
    private void Start()
    {
        //Make sure all parts are deactivated. Will bring them in order.
        firstEnemy.SetActive(false);
        secondEnemy.SetActive(false);
    }

    private void Update()
    {
        //check if tutorial mode is needed
        if (GameManager.Instance.IsPlayingIntroTutorial)
        {
            if (gameBeginsStartTutorial)
            {
                gameBeginsStartTutorial = false;
                StartCoroutine(IntroTutorial());
            }
        }
    }


    private IEnumerator IntroTutorial()
    {
        // [Black Screen]
        fadeInCanvasGroup.gameObject.SetActive(true);
        var player = GameObject.Find("Player");
        var startFollow = GameObject.Find("CameraStartPos").transform;
        var virtualCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = startFollow;
        //pause at beginning before jumping straight into voice
        yield return new WaitForSeconds(waitTimeBeforeStartingNarrationOne);

        //if tutorial player cannot leave spleen
        if (gameBeginsStartTutorial)
        {
            bloodVesselEntrance.SetActive(false);
        }

        //Voice narrator: “today we will have a look at...”
        //yield return new WaitForSeconds(PlayNarrationAndReturnWaitTime("Narration1"));
        PlayNarrationAndReturnWaitTime("Narration1");
        //[fades in, we see the hub area]
        do
        {
            FadeIn();
            yield return new WaitForFixedUpdate();
        } while (fadeInCanvasGroup.alpha >= 0.01f);
        fadeInCanvasGroup.gameObject.SetActive(false);

        //Voice narrator: “The Immune System, a bottomless pit of complexity; though from the view of a cell, life is essentially in 2 Dimensions.”
        yield return new WaitForSeconds(PlayNarrationAndReturnWaitTime("Narration2"));
        //Voice narrator: “We begin our journey in the Spleen; here Dendritic cells deposit a snapshot from an infection site; a battleground so to speak.”
        yield return new WaitForSeconds(PlayNarrationAndReturnWaitTime("Narration3"));


        //Voice narrator: “Now where is out little fella?”
        yield return new WaitForSeconds(PlayNarrationAndReturnWaitTime("Narration4"));


        //[INPUT PROMT: “Press Space to Dash”]
        //slight movement before start
        var playerMovementComponent = player.GetComponent<PlayerMovement>();
        //show dash text
        textBox1.SetActive(true);

        //Space bar ends dialogue, go to next step
        do
        {
            playerMovementComponent.Body.velocity = new Vector2(0, -0.3f) * Time.fixedDeltaTime;
            yield return null;
        } while (!Input.GetKeyDown(KeyCode.Space));
        virtualCamera.Follow = player.transform;
        //hide Dash text
        textBox1.SetActive(false);
        //resets

        //[Dendritic cell appears from the right]
        Debug.Log("player enters");

        //Voice narrator: “Ah there he is! Watch out little one, you could hurt someone with that!”
        yield return new WaitForSeconds(PlayNarrationAndReturnWaitTime("Narration5"));

        //[An Enemy (base enemy) appears]
        Debug.Log("base enemy enters");
        //todo spawn enemy
        //Instantiate(firstEnemy, EnemySpawnPos.transform);

        //Voice narrator: “This is very odd! Normally we don’t see [ENEMY] here; quickly get rid of him!”
        yield return new WaitForSeconds(PlayNarrationAndReturnWaitTime("Narration6"));

        //Tutorial text: “Move using WSAD; use Dash (space) to kill the enemy.”
        textBox2.SetActive(true);
        //first enemy appears
        firstEnemy.SetActive(true);

        //[Player kills enemy]
        while (firstEnemy != null && !firstEnemy.IsDestroyed())
        {
            yield return new WaitForSeconds(0.3f);
        }

        Debug.Log("killed enemy");
        textBox2.SetActive(false);


        //Voice narrator: “Very good! Now with that taken care of.....”
        yield return new WaitForSeconds(PlayNarrationAndReturnWaitTime("Narration7"));

        //[Another Enemy (dasher) Appears]
        secondEnemy.SetActive(true);

        //Voice narrator: “Ill be damned; both Bacterial and viruses at the same time; usually these two are arch enemies! Take care of him; but be careful; this one seems a lot more capable than the last one!”
        yield return new WaitForSeconds(PlayNarrationAndReturnWaitTime("Narration8"));

        textBox2.SetActive(true);

        //[Player Kills Enemy]
        while (secondEnemy != null && !secondEnemy.IsDestroyed())
        {
            yield return new WaitForSeconds(0.3f);
        }

        Debug.Log("killed enemy 2");
        textBox2.SetActive(false);

        //Voice narrator: “Well done! Well done! Now quickly, go and find out what causes this; there must be a source of the infection somewhere!”
        yield return new WaitForSeconds(PlayNarrationAndReturnWaitTime("Narration9"));

        //Tutorial Text: “Use the blood vessel to get to investigate the source of infection.”
        textBox3.SetActive(true);

        //allows player to leave
        bloodVesselEntrance.SetActive(true);

        //End of the tutorial
    }

    private float PlayNarrationAndReturnWaitTime(string narration)
    {
        var clipLenght = AudioManager.instance.PlayNarration(narration);
        Debug.Log(narration);
        return clipLenght > 0 ?clipLenght : 0.2f;
    }


    // Canvas fade in variables
    [SerializeField] private CanvasGroup fadeInCanvasGroup;
    [SerializeField] private float TimeToFadeIn;


    //Fade in from Black screen to the scene 
    private void FadeIn()
    {
        fadeInCanvasGroup.alpha -= TimeToFadeIn * Time.deltaTime;
    }
}