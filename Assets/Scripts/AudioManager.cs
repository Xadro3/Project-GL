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


    private void Start()
    {
        // Wenn die Aktive Scene der Encounter sit Startet Entsprechend die Musik, sollte obsolet sein, da der AudioManager nicht destroyed wird, ist noch zu testzwecken da
        if (SceneManager.GetActiveScene().name == "Encounter" | SceneManager.GetActiveScene().name == "ParticleTestScene")
        {
            musicSource.clip = breachStartingInterlude;
            musicSource.Play();
            musicDuration = (double)breachStartingInterlude.samples / breachStartingInterlude.frequency;
            //goalTime = goalTime + musicDuration;

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

            sfxSource.clip = breachStart;
            sfxSource.Play();
        }
        
        //noch nicht funktional. Soll den Loop Starten sobald der Interlude des Encounters abgeschlossen wurde
        if (isPlaying == true && musicDuration == 0)
        {
            musicSource.clip = breachBackgroundLoop;
            musicSource.Play();
        }

        //Es wird wieder die Musik abgespielt die bei der Overworld abgespielt werden soll
        if (isPlaying == true && SceneManager.GetActiveScene().name == "Overworld")
        {
            yield return new WaitForSeconds(34);
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




        /* if (musicDuration <= goalTime)
        {
            musicSource.clip = breachBackgroundLoop;
            musicSource.Play();

        }

        
        if (AudioSettings.dspTime > goalTime)
        {
            musicSource.clip = breachBackgroundLoop;
            musicSource.PlayScheduled(goalTime);
        }
        */

    }


    // Wird benötigt um Sound effekte zu spielen, es kann jeweils nur 1er gespielt werden, ansonsten müssen wir das noch anpassen, damit wir mehr AudioSources haben, die mit den Einstellungen auch verändert werden können
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
