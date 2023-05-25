using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Author: Pascal

public class LenseSwap : MonoBehaviour
{

    [SerializeField] Animation anim;
    [SerializeField] GameObject titleMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] float t_animation = 1f;
    
    State state = State.Title;
    public enum State {
        Title = 0, Options = 1,
    }
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
            SwapLenses();
    }

    [ContextMenu("Swap Lenses")]
    void SwapLenses() {
        anim.Play();
        Invoke(nameof(ToggleMenuState), t_animation/ 2f);
    }

    void ToggleMenuState() {
        switch (state) {
            case State.Title:
                EnterOptionsState();
                break;
            case State.Options:
                EnterTitleState();
                break;
            default: break;
        }
    }

    void EnterTitleState() {
        state = State.Title;
        optionsMenu.SetActive(false);
        titleMenu.SetActive(true);
    }

    void EnterOptionsState() {
        state = State.Options;
        optionsMenu.SetActive(true);
        titleMenu.SetActive(false);
    }

}
