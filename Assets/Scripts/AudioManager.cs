using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    bool inEncounter = false;

    [Header("------------ Audio Source ------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("------- Pause und Menü Sound -------")]
    public AudioClip buttonPress;
    public AudioClip pauseActivate;
    public AudioClip pauseDeactivate;
    public AudioClip sceneTransitionOpen;
    public AudioClip sceneTransitionClose;

    [Header("---------- Breach Audio Clip ----------")]
    public AudioClip breachStartingInterlude;
    public AudioClip breachBackgroundLoop;
    public AudioClip endTurn;
    public AudioClip breachLost;
    public AudioClip breachWon;
    public AudioClip breachStart;
    public AudioClip slotSnap;

    [Header("-------------- Hauptmenü -------------")]
    public AudioClip backgroundmusicMenu;

    [Header("----------------- Map -----------------")]
    public AudioClip nodeClick;


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
        PlaySFX(sceneTransitionOpen);

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

    private void HandleEndTurn()
    {
        PlaySFX(endTurn);
    }

    private void HandleNoteClick()
    {
        PlaySFX(nodeClick);
    }
}
