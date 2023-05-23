﻿using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class Sound
{
    public string name;     //Store the name of our music/effect
    public AudioClip clip;  //Store the actual music/effect
    // ! Unity bugged: resets every script reload
    // [Range(0f, 1f)]         //limit the range in the Unity editor 
    public float volume;    //Store our volume
    // ! Unity bugged: resets every script reload
    // [Range(0.1f, 3f)]       //Limit the Range again
    public float pitch;     // set the picth for our music/effect
    [HideInInspector]       //Hide this variable from the Editor
    public AudioSource source;// the source that will play the sound
    public bool loop = false;// should this sound loop
    public Areas Area;  // The Area where the sound should be played
}

public enum Areas
{

    DoesNotApply,
    Menu,
    Hub,
    Area1,

}

public enum SoundEffects
{
    PlayerDash,
    EnemyDash,
    PlayerMoving,
    PlayerTakingDamage,
    EnemyDeath,
    PlayerDeath,
    CollectingEnemyPart,
    CollectingDna,
    Collision,
    DropingOffEnememyParts,
    MainMenuSelection,
    LogoSound,
    PewPew,
}

public static class AudioHelper
{
    public static string MapToName(this SoundEffects soundEffect)
    {
        switch (soundEffect)
        {
            case SoundEffects.PlayerDash:
            case SoundEffects.EnemyDash:
                return "Dash";
            case SoundEffects.PlayerMoving:
                return "Player Moving";
            case SoundEffects.PlayerTakingDamage:
                return "Take_Damage";
            case SoundEffects.EnemyDeath:
                var enemyDieSounds = AudioManager.instance.sounds.Where(o => o.clip.name.IndexOf("Enemy_Death_",StringComparison.CurrentCultureIgnoreCase)>=0).ToList();
                return enemyDieSounds[Random.Range(0, enemyDieSounds.Count - 1)].name;
            case SoundEffects.PlayerDeath:
                return "Player Death"; //todo
            case SoundEffects.CollectingEnemyPart:
                return "Pick_Up_Enemy_Part";
            case SoundEffects.CollectingDna:
                return "Pick_Up_Enemy_DNA_Upgrade";
            case SoundEffects.Collision:
                return "Collision"; //todo
            case SoundEffects.DropingOffEnememyParts:
                return "DropingOffEnememyParts"; //todo
            case SoundEffects.MainMenuSelection:
                return "MainMenuSelection"; //todo
            case SoundEffects.LogoSound:
                return "LogoSound"; //todo
            case SoundEffects.PewPew:
                return "Harpoony_Thing";
        }
        return soundEffect.ToString("G");
    }
}