using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialDialogue : MonoBehaviour
{
    //text dialogue variable
    [SerializeField] private TextMeshProUGUI dialogueTextBox;
    [SerializeField] private GameObject[] tutorialDialogueBox;
    [SerializeField] private string[] whatDialogueBoxSays;

    private int dialogueIndex;

    private void Start() {
        HideDialogueBox();
    }

    public void HideDialogueBox(){
        //empty string ready for next text
        dialogueTextBox.text = string.Empty;
        //deactivates the dialogue box
        foreach (GameObject box in tutorialDialogueBox){
            box.SetActive(false);
        }
        
    }

    //activates the dialogue box
    public void ShowDialogueBox(int boxIndex){
        //set background
        tutorialDialogueBox[boxIndex].SetActive(true);
        //set text
        dialogueTextBox.text = whatDialogueBoxSays[boxIndex];
    }
}
