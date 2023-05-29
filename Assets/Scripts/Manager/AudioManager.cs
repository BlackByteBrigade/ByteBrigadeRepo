using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;      // store all our sounds (sfx)
    public Sound[] playlist;    // store all our music
    public Sound[] playlistAreas;    // store all our music
    public Sound[] Narration;    // store all our narration

    private int currentPlayingIndex = 999; // set high to signify no song playing

    // a play music flag so we can stop playing music during cutscenes etc
    private bool shouldPlayMusic = false;
    private bool shouldPlayAreaMusic = false;

    public static AudioManager instance; // will hold a reference to the first AudioManager created

    private float mvol; // Global music volume
    private float evol; // Global effects volume

    public static float VolumeMusic => instance == null ? 1f : instance.mvol;
    public static float VolumeSFX => instance == null ? 1f : instance.evol;

    private bool playingAreaMusic;


    private void Awake()
    {

        if (instance == null)
        {     // if the instance var is null this is first AudioManager
            instance = this;        //save this AudioManager in instance 
        }
        else
        {
            Destroy(gameObject);    // this isnt the first so destroy it
            return;                 // since this isn't the first return so no other code is run
        }

        DontDestroyOnLoad(gameObject); // do not destroy me when a new scene loads

        // get preferences
        mvol = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        evol = PlayerPrefs.GetFloat("EffectsVolume", 0.75f);

        createAudioSources(sounds, evol);     // create sources for effects
        createAudioSources(playlist, mvol);   // create sources for music
        createAudioSources(playlistAreas, mvol);   // create sources for combat music

    }

    // create sources
    private void createAudioSources(Sound[] sounds, float volume)
    {
        foreach (Sound s in sounds)
        {   // loop through each music/effect
            s.source = gameObject.AddComponent<AudioSource>(); // create anew audio source(where the sound splays from in the world)
            s.source.clip = s.clip;     // the actual music/effect clip
            s.source.volume = s.volume * volume; // set volume based on parameter
            //s.source.pitch = s.pitch;   // set the pitch
            s.source.pitch = 1;   // set the pitch
            s.source.loop = s.loop;     // should it loop
        }
    }

    /// <summary>
    /// First tries to find a sound with a matching name attribute; if not tries to find a clip with <see cref="name"/>
    /// </summary>
    /// <param name="sounds"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sound FindSound(Sound[] sounds, string name)
    {
        return Array.Find(sounds, sound => sound.name.Equals(name, StringComparison.CurrentCultureIgnoreCase)) ??
               Array.Find(sounds, sound => sound.clip.name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
    }

    public void PlaySfX(SoundEffects soundEffect)
    {
        // here we get the Sound from our array with the name passed in the methods parameters
        var s = FindSound(sounds, soundEffect.MapToName());
        if (s == null)
        {
            Debug.LogError("Unable to play sound " + soundEffect.MapToName());
            return;
        }
        s.source.Play(); // play the sound
    }

    public void PlaySound(string name)
    {
        // here we get the Sound from our array with the name passed in the methods parameters
        var s = FindSound(sounds, name);
        if (s == null)
        {
            Debug.LogError("Unable to play sound " + name);
            return;
        }
        s.source.Play(); // play the sound
    }

    /// <summary>
    /// Insert the <see cref="name"/> To play the narration with that given name
    /// </summary>
    /// <param name="name"></param>
    public float PlayNarration(string name)
    {
        // here we get the Sound from our array with the name passed in the methods parameters
        var s = FindSound(Narration, name);
        if (s == null)
        {
            Debug.LogError("Unable to play narration " + name);
            return 2f;
        }
        s.source.Play(); // play the sound
        return s.clip.length;
    }

    public void PlayMusic()
    {
        if (shouldPlayMusic == false)
        {
            shouldPlayMusic = true;
            shouldPlayAreaMusic = false;

            if (playingAreaMusic)
            {
                StartCoroutine(FadeOutsource(playlistAreas[currentPlayingIndex].source));
            }

            // pick a random song from our playlist
            currentPlayingIndex = UnityEngine.Random.Range(0, playlist.Length);
            playlist[currentPlayingIndex].source.volume = playlist[0].volume * mvol; // set the volume
            playlist[currentPlayingIndex].source.Play(); // play it
            
            playingAreaMusic = false;
        }

    }

    public IEnumerator FadeOutsource(AudioSource audioSource)
    {
        var origVolume = audioSource.volume;
        for (int i = 0; i < 15; i++)
        {
            audioSource.volume -= origVolume / 15f;
            yield return new WaitForSeconds(0.20f);
        }
        audioSource.Stop();
        audioSource.volume = origVolume;
    }

    private void PlayCombatMusic()
    {
        if (shouldPlayAreaMusic == false)
        {
            shouldPlayAreaMusic = true;
            shouldPlayMusic = false;

            if (!playingAreaMusic && currentPlayingIndex<999)
            {
                playlist[currentPlayingIndex].source.Stop(); // stop ambient music
            }

            // pick a random song from our combat playlist
            currentPlayingIndex = UnityEngine.Random.Range(0, playlistAreas.Length);
            playlistAreas[currentPlayingIndex].source.volume = playlistAreas[0].volume * mvol; // set the volume
            playlistAreas[currentPlayingIndex].source.Play(); // play it
            playingAreaMusic = true;
        }
    }

    public void PlayLoudAreaMusic()
    {
        if (!playingAreaMusic)
        {
            PlayCombatMusic();
        }
        if (currentPlayingIndex >= playlistAreas.Length-1)
        {
            // Debug.LogError("error");
            return;
        }

        StartCoroutine(FadeToVolume(playlistAreas[currentPlayingIndex].source, 1f, mvol));
    }

    public void PlayRegularVolumeAreaMusic(bool forcePlayAnother = false)
    {
        if (!playingAreaMusic)
        {
            PlayCombatMusic();
        }
        else if (forcePlayAnother)
        {
            //we can add this so if the player uses a teleporter also gets a new track
            // pick a random song from our combat playlist
            if (!playingAreaMusic && currentPlayingIndex < 999)
            {
                playlist[currentPlayingIndex].source.Stop(); // stop ambient music
            }
            else if(currentPlayingIndex < 999)
            {
                playlistAreas[currentPlayingIndex].source.Stop(); //stop area music
            }
            int newIndex;
            do
            {
                newIndex = UnityEngine.Random.Range(0, playlistAreas.Length);
            } while (newIndex == currentPlayingIndex);
            currentPlayingIndex = newIndex;
            playlistAreas[currentPlayingIndex].source.volume = playlistAreas[currentPlayingIndex].volume * mvol; // set the volume
            playlistAreas[currentPlayingIndex].source.Play(); // play it
            playingAreaMusic = true;
            return;
        }

        StartCoroutine(FadeToVolume(playlistAreas[currentPlayingIndex].source, playlistAreas[currentPlayingIndex].volume, mvol));
    }


    public IEnumerator FadeToVolume(AudioSource audioSource, float volume, float multiplier, float TimeToReachNewVolumeInSeconds = 3)
    {
        volume *= multiplier;
        //Debug.Log("Target volume:" +volume);
        var origVolume = audioSource.volume;

        //we only want to fade if we can hear a difference
        if ((Math.Abs(volume - origVolume) < 0.05)) 
            yield break;

        var diff = Math.Abs(volume - origVolume);
        var waittime = diff / TimeToReachNewVolumeInSeconds;
        var isLouder = volume > origVolume;
        do
        {
            if (isLouder) audioSource.volume += waittime *0.1f;
            else audioSource.volume -= waittime * 0.1f;
            yield return new WaitForSeconds(0.33f-waittime);
        } while (isLouder ? audioSource.volume >= volume : audioSource.volume <= volume);
    }

    // stop music
    public void StopMusic()
    {
        if (shouldPlayMusic)
        {
            shouldPlayMusic = false;
            currentPlayingIndex = 999; // reset playlist counter
        }
    }

    void Update()
    {
        if (playingAreaMusic)
        {
            // if we are playing a track from the playlist && it has stopped playing
            if (currentPlayingIndex != 999 && !playlistAreas[currentPlayingIndex].source.isPlaying)
            {
                currentPlayingIndex++; // set next index
                if (currentPlayingIndex >= playlistAreas.Length)
                { //have we went too high
                    currentPlayingIndex = 0; // reset list when max reached
                }
                playlistAreas[currentPlayingIndex].source.Play(); // play that funky music
            }
        }
        else
        {
            // if we are playing a track from the playlist && it has stopped playing
            if (currentPlayingIndex != 999 && !playlist[currentPlayingIndex].source.isPlaying)
            {
                currentPlayingIndex++; // set next index
                if (currentPlayingIndex >= playlist.Length)
                { //have we went too high
                    currentPlayingIndex = 0; // reset list when max reached
                }
                playlist[currentPlayingIndex].source.Play(); // play that funky music
            }
        }
      
    }

    // get the song name
    public String getSongName()
    {
        return playlist[currentPlayingIndex].name;
    }

    // if the music volume change update all the audio sources
    public void musicVolumeChanged()
    {
        foreach (Sound m in playlist)
        {
            mvol = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
            m.source.volume = playlist[0].volume * mvol;
        }
    }

    //if the effects volume changed update the audio sources
    public void effectVolumeChanged()
    {
        evol = PlayerPrefs.GetFloat("EffectsVolume", 0.75f);
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume * evol;
        }
        sounds[0].source.Play(); // play an effect so user can her effect volume
    }
}
