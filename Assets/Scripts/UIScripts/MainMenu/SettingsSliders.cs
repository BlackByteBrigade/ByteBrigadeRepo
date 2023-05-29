using UnityEngine;
using UnityEngine.UI;

public class SettingsSliders : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider effectsVolumeSlider;
    void Start()
    {
        musicVolumeSlider.onValueChanged.AddListener((v) => { PlayerPrefs.SetFloat("MusicVolume", v); print(PlayerPrefs.GetFloat("MusicVolume")); });
        effectsVolumeSlider.onValueChanged.AddListener((ev) => { PlayerPrefs.SetFloat("EffectsVolume", ev); });
    }
}
