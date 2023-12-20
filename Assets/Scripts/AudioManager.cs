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

        if (SceneManager.GetActiveScene().name == "Menu")
        {
            musicSource.clip = backgroundmusicMenu;
            musicSource.Play();
        }

    }

    private void Update()
    {


        if (isPlaying == false && SceneManager.GetActiveScene().name == "Encounter")
        {

            musicSource.clip = breachStartingInterlude;
            musicSource.Play();
            isPlaying = true;
            musicDuration = (double)breachStartingInterlude.samples / breachStartingInterlude.frequency;

            sfxSource.clip = breachStart;
            sfxSource.Play();
        }

        if (isPlaying == true && musicDuration == 0)
        {
            musicSource.clip = breachBackgroundLoop;
            musicSource.Play();
        }

        if (isPlaying == true && SceneManager.GetActiveScene().name == "Overworld")
        {
            musicSource.clip = backgroundmusicMenu;
            musicSource.Play();
            isPlaying = false;
        }
        
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



    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
