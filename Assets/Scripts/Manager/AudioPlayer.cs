using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Author: Pascal
// local audio manager for spatial audio

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] List<Sound> sounds;


    void Awake() {
        createAudioSources(sounds);   
    }

    void createAudioSources(List<Sound> sounds) {
        foreach (Sound s in sounds) {
            CreateAudioComponent(s);
        }
    }


    void CreateAudioComponent(Sound sound) {
        Debug.Log($"create audio component {sound.name}");
        sound.source = gameObject.AddComponent<AudioSource>();
        sound.source.clip = sound.clip;
        sound.source.volume = sound.volume * AudioManager.VolumeSFX;
        sound.source.pitch = 1; 
        sound.source.spatialBlend = 1f; // important else no audio falloff in distance
        sound.source.loop = sound.loop;
    }

    public void AddSound(Sound sound) {
        sounds.Add(sound);
        CreateAudioComponent(sound);
    }

    public void Play(string soundName) => Play( FindSound(soundName) );

    public void Play(Sound sound) {
        if (sound == null) {
            Debug.LogError("Unable to play sound");
            return;
        }
        sound.source.volume = sound.volume * AudioManager.VolumeSFX;
        sound.source.Play();
    }


    public Sound FindSound(string soundName) {
        for (int i=0; i< sounds.Count; i++) {
            if (sounds[i].name == soundName) 
                return sounds[i];
        }
        Debug.LogError($"Sound not found: {soundName}");
        return null;
    }


}
