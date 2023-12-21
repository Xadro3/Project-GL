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

    [Header("------- Pause und Men� Sound -------")]
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

    [Header("-------------- Hauptmen� -------------")]
    public AudioClip backgroundmusicMenu;

    [Header("----------------- Map -----------------")]
    public AudioClip nodeClick;

    [Header("---------------- Shop ----------------")]
    public AudioClip backgroundmusicShop;

    [Header("---------------- Workshop ----------------")]
    public AudioClip backgroundmusicWorkshop;
   
    [Header("---------------- Ereignis ----------------")]
    public AudioClip backgroundmusicEreignis;

    private void Awake()
    {
        //GameObject wird ab dem Menu mitgegeben, damit Musik kontinuiirlich durchlaufen kann
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }
    
    //Sobald eine Scene Geladen wurde Passiert das:
    private void OnSceneLoad(Scene scene, LoadSceneMode arg1)
    {
        //Sobald Scene gestartet ist spielt der sceneTransitionOpen Sound ab
        PlaySFX(sceneTransitionOpen);

        //Nach checken in welcher scene man sich befindet startet entsprechende Musik
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
            case "Shops":
                musicSource.clip=backgroundmusicShop;
                musicSource.Play();
                break;
        }
    }

    private void Start()
    {
        // Listen to events
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
        //Zum Abspielen des BreachBackgroundLoops, nachdem der Anfangs Interlude durchgelaufen ist
        if (!musicSource.isPlaying)
        {
            musicSource.clip = breachBackgroundLoop;
            musicSource.Play();
            musicSource.loop = true;
        }

    }

    // Wird ben�tigt um Sound effekte zu spielen
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    //Pause und Button Handler
    private void HandleButtonPress()
    {

    }
    void HandleOpenPause()
    {
        PlaySFX(pauseActivate);
    }

    private void HandleClosePause()
    {
        PlaySFX(pauseDeactivate);
    }

    //Breach Sound Handler
    void HandleCardDropped()
    {
        PlaySFX(slotSnap); // Do something when this event is triggered
    }

    private void HandleEndTurn()
    {
        PlaySFX(endTurn);
    }

    //Map Sound Handler
    private void HandleNoteClick()
    {
        PlaySFX(nodeClick);
    }
}
