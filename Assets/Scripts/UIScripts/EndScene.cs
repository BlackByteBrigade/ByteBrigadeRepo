using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EndScene : MonoBehaviour
{
    [SerializeField] Image blackScreen;
    [SerializeField] GameObject graphic;
    [SerializeField] float fadeDuration = 2f;

    [ContextMenu("Fade")]
    public void FadeToBlack() {
        HideGraphic();
        blackScreen.DOFade(0, 0);
        blackScreen.DOFade(1f, fadeDuration)
            .OnComplete(() => ShowGraphic());
    }


    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
            FadeToBlack();
    }

    public void ShowGraphic() {
        graphic.SetActive(true);
    }

    public void HideGraphic() {
        graphic.SetActive(false);
    }
}
