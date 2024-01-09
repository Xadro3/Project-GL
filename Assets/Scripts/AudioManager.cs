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
    public AudioClip enemyTurn;
    public AudioClip playerTurn;
    public AudioClip breachLost;
    public AudioClip breachWon;
    public AudioClip breachStart;
    public AudioClip slotSnap;
    public AudioClip cardRepair;
    public AudioClip cardBuff;
    public AudioClip cardDestroy;
    public AudioClip infoOpen;
    public AudioClip takeReward;
    public AudioClip attackStart;
    public AudioClip alphaRadiation;
    public AudioClip betaRadiation;
    public AudioClip gammaRadiation;
    public AudioClip noEnergy;

    [Header("-------------- Hauptmenü -------------")]
    public AudioClip backgroundmusicMenu;

    [Header("----------------- Map -----------------")]
    public AudioClip backgroundmusicMap;
    public AudioClip nodeClick;

    [Header("---------------- Shop ----------------")]
    public AudioClip backgroundmusicShop;
    public AudioClip cardBuy;

    [Header("---------------- Workshop ----------------")]
    public AudioClip backgroundmusicWorkshop;
    public AudioClip cardUpgrade;
    public AudioClip workshopCardEntfern;
    public AudioClip heal;
   
    [Header("---------------- Ereignis ----------------")]
    public AudioClip backgroundmusicEreignis;
    public AudioClip correctAnswer;
    public AudioClip incorrectAnswer;

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
                musicSource.loop = true;
                inEncounter = false;
                musicSource.clip = backgroundmusicMap;
                musicSource.Play();
                break;
            case "Menu" when inEncounter != false: //Musik die im Haupmenü spielt
                musicSource.loop = true;
                inEncounter = false;
                musicSource.clip = backgroundmusicMenu;
                musicSource.Play();
                break;
            case "Shops" when inEncounter == false: //Musik die im Laden spielt
                inEncounter = true;
                musicSource.clip = backgroundmusicShop;
                musicSource.Play();
                break;
            case "Event" when inEncounter == false: //Musik die im Ereignis spielt
                inEncounter = true;
                musicSource.clip = backgroundmusicEreignis;
                musicSource.Play();
                break;
            case "Workshop" when inEncounter == false: //Musik die in der Werkstadt spielt
                inEncounter = true;
                musicSource.clip = backgroundmusicWorkshop;
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
        EndTurnButtonEventScript.EndTurnEvent += HandleEndTurn;
        TurnMaster.AlphaDamageEvent += HandleAlphaRadiation;
        TurnMaster.BetaDamageEvent += HandleBetaRadiation;
        TurnMaster.GammaDamageEvent += HandleGammaRadiation;
        TurnMaster.AttackStartEvent += HandleAttackStart;
        GameManager.CardRewardChosenSoundEvent += HandleReward;
        CardMovementHandler.ShowCardPopupEvent += HandleCardInfo;
        CardMovementHandler.CardMoveToDiscardPileEvent += HandleCardDestroy;
        Slot.ShieldRepairEvent += HandleCardRepair;
        Slot.ShieldBuffEvent += HandleCardBuff;
        TurnMaster.StartTurnEvent += HandleStartTurn;
        PlayerHealthManager.EncounterEnd += HandleLostGame;


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
        PlaySFX(enemyTurn);
    }

    void HandleStartTurn()
    {
        PlaySFX(endTurn);
        PlaySFX(playerTurn);
    }

    void HandleCardRepair()
    {
        PlaySFX(cardRepair);
    }

    void HandleCardBuff()
    {
        PlaySFX(cardBuff);
    }

    void HandleCardDestroy()
    {
        PlaySFX(cardDestroy);
    }

    void HandleCardInfo()
    {
        PlaySFX(infoOpen);
    }

    void HandleReward()
    {
        PlaySFX(takeReward);
    }
    void HandleAttackStart()
    {
        PlaySFX(attackStart);
    }

    void HandleAlphaRadiation()
    {
        PlaySFX(alphaRadiation);
    }

    void HandleBetaRadiation()
    {
        PlaySFX(betaRadiation);
    }
    void HandleGammaRadiation()
    {
        PlaySFX(gammaRadiation);
    }

    void HandleLostGame()
    {
        PlaySFX(breachLost);
    }

    void HandleNoEnergy()
    {
        PlaySFX(noEnergy);
    }


    //Workshop Sound Handler
    void HandleCardUpgrade()
    {
        PlaySFX(cardUpgrade);
    }

    void HandleWorkshopCardEntfern()
    {
        PlaySFX(workshopCardEntfern);
    }

    //Shop Sound Handler
    void HandleCardBuy()
    {
        PlaySFX(cardBuy);
    }

}
