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
            case "Encounter" when inEncounter == false: //start der Musik die im Breach spielt, wir in Update fortgesetzt
                musicSource.loop = false;
                inEncounter = true;
                musicSource.clip = breachStartingInterlude;
                musicSource.Play();
                break;
            case "Overworld" when inEncounter != false: //Musik die auf der Map spielt
            case "Menu" when inEncounter != false: //Musik die im Haupmenü spielt
                musicSource.loop = true;
                inEncounter = false;
                musicSource.clip = backgroundmusicMenu;
                musicSource.Play();
                break;
            case "Shops" when inEncounter == false: //Musik die im Laden spielt
            case "Event" when inEncounter == false: //Musik die im Ereignis spielt
            case "Workshop" when inEncounter == false: //Musik die in der Werkstadt spielt
                inEncounter = true;
                musicSource.clip = backgroundmusicShop;
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
        if (!musicSource.isPlaying && inEncounter == true)
        {
            musicSource.clip = breachBackgroundLoop;
            musicSource.Play();
            musicSource.loop = true;
        }

    }

    // Wird benötigt um Sound effekte zu spielen
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    //Pause und Button Handler
    void HandleButtonPress()
    {
        PlaySFX(buttonPress);
    }
    void HandleOpenPause()
    {
        PlaySFX(pauseActivate);
    }

    void HandleClosePause()
    {
        PlaySFX(pauseDeactivate);
    }

    //Breach Sound Handler
    void HandleCardDropped()
    {
        PlaySFX(slotSnap); // Do something when this event is triggered
    }

    void HandleEndTurn()
    {
        PlaySFX(endTurn);
    }

    //Map Sound Handler
    void HandleNodeClick()
    {
        PlaySFX(nodeClick);
    }

}
