using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public double musicDuration;
    public double goalTime;
    bool isPlaying = false;

    [Header("------------ Audio Source ------------")]
    //[SerializeField] AudioSource startingLoopSource;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("------- Pause und Menü Sound -------")]
    public AudioClip pauseActivate;
    public AudioClip pauseDeactivate;
    public AudioClip sceneTransition;

    [Header("---------- Breach Audio Clip ----------")]
    public AudioClip breachStartingInterlude;
    public AudioClip breachBackgroundLoop;
    public AudioClip endTurn;
    public AudioClip breachLost;
    public AudioClip breachWon;
    public AudioClip breachStart;
    public AudioClip slotSnap;

    [Header("------------- Hauptmenü -------------")]
    public AudioClip backgroundmusicMenu;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
    {
        PlaySFX(sceneTransition);
    }

    private void Start()
    {
        // Listen to event CardDropped
        CardMovementHandler.CardDropped += HandleCardDropped;
        PauseMenu.OpenPauseEvent += HandleOpenPause;
        PauseMenu.ClosePauseEvent += HandleClosePause;
        //SceneManager.sceneUnloaded += HandleEndTransition;

        // Wenn die Aktive Scene der Encounter sit Startet Entsprechend die Musik, sollte obsolet sein, da der AudioManager nicht destroyed wird, ist noch zu testzwecken da
        if (SceneManager.GetActiveScene().name == "Encounter" | SceneManager.GetActiveScene().name == "ParticleTestScene")
        {
            musicSource.clip = breachStartingInterlude;
            musicSource.Play();
            musicDuration = (double)breachStartingInterlude.samples / breachStartingInterlude.frequency;
            goalTime = goalTime + musicDuration;

            /*
            goalTime = AudioSettings.dspTime + 0.5;
            musicSource.clip = breachStartingInterlude;
            musicSource.PlayScheduled(goalTime);
            musicDuration = (double)breachStartingInterlude.samples / breachStartingInterlude.frequency;
            */

            sfxSource.clip = breachStart;
            sfxSource.Play();   
        }
        //Clip Starten wenn man das Spiel gestartet hat
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            musicSource.clip = backgroundmusicMenu;
            musicSource.Play();
        }

    }


    private void Update()
    {

        //Sobald man in der "Encounter" Scene ankommt Startet Entsprechende Musik und der Soundeffeckt
        if (isPlaying == false && SceneManager.GetActiveScene().name == "Encounter")
        {
            
            musicSource.clip = breachStartingInterlude;
            musicSource.Play();
            isPlaying = true;
            musicDuration = (double)breachStartingInterlude.samples / breachStartingInterlude.frequency;
            goalTime = goalTime + musicDuration;
            

            //StartCoroutine(PlayBackgroundMusic());

            sfxSource.clip = breachStart;
            sfxSource.Play();
        }
        
        //noch nicht funktional. Soll den Loop Starten sobald der Interlude des Encounters abgeschlossen wurde
        
        if (isPlaying == true && SceneManager.GetActiveScene().name == "Encounter" && goalTime == 0)
        {
            musicSource.clip = breachBackgroundLoop;
            musicSource.PlayScheduled(goalTime);
        }
        

        //Es wird wieder die Musik abgespielt die bei der Overworld abgespielt werden soll
        if (isPlaying == true && SceneManager.GetActiveScene().name == "Overworld")
        {
            musicSource.clip = backgroundmusicMenu;
            musicSource.Play();
            isPlaying = false;
        }
        
        //Es wird wieder die Musik abgespielt die im Hauptmenü abgespielt werden soll
        if (isPlaying == true && SceneManager.GetActiveScene().name == "Menu")
        {
            musicSource.clip = backgroundmusicMenu;
            musicSource.Play();
            isPlaying = false;
        }
    
    }
    //Weiß nicht wie ich das wirklich richtig zum laufen bringe
    /*IEnumerator PlayBackgroundMusic()
    {
            musicSource.clip = breachStartingInterlude;
            musicSource.Play();
            yield return new WaitForSeconds(musicSource.clip.length);
            musicSource.clip = breachBackgroundLoop;
            musicSource.Play();
    }*/

   


    // Wird benötigt um Sound effekte zu spielen, es kann jeweils nur 1er gespielt werden, ansonsten müssen wir das noch anpassen, damit wir mehr AudioSources haben, die mit den Einstellungen auch verändert werden k
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    void HandleCardDropped()
    {
        PlaySFX(slotSnap); // Do something when this event is triggered
    }

    void HandleOpenPause()
    {
        PlaySFX(pauseActivate);
    }

    private void HandleClosePause()
    {
        PlaySFX(pauseDeactivate);
    }
        private void HandleEndTransition(Scene arg0)
    {
        PlaySFX(sceneTransition);
    }

}
