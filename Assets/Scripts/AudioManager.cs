using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public double musicDuration;
    public double goalTime;
    bool inEncounter = false;

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

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode arg1)
    {
        PlaySFX(sceneTransition);

        switch (scene.name)
        {
            case "Encounter" when inEncounter == false:
                musicSource.loop = false;
                inEncounter = true;
                musicSource.clip = breachStartingInterlude;
                musicSource.Play();
                break;
            case "Overworld" when inEncounter != false:
            case "Menu" when inEncounter != false:
                musicSource.loop = true;
                inEncounter = false;
                musicSource.clip = backgroundmusicMenu;
                musicSource.Play();
                break;
        }
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
        if (!musicSource.isPlaying)
        {
            musicSource.clip = breachBackgroundLoop;
            musicSource.Play();
            musicSource.loop = true;
        }

    }


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
