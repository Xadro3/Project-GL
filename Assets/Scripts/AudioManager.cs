using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("--------- Audio Source ---------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("--------- Breach Audio Clip ---------")]
    public AudioClip breachStartingInterlude;
    public AudioClip breachBackgroundLoop;
    public AudioClip endTurn;
    public AudioClip pauseActivate;
    public AudioClip pauseDeactivate;
    public AudioClip breachLost;
    public AudioClip breachWon;
    public AudioClip sceneStart;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Encounter")
        {
            musicSource.clip = breachStartingInterlude;
            musicSource.Play();      
        }

    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
