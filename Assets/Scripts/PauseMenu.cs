using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseScreen;
    public GameObject targetPosition;
    public Vector3 velocity = Vector3.zero;
    public float smoothTime;
    public bool latch;
    public GameObject offscreenPosition;
    public Vector3 currentPosition;
    private GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }


    private void Update()
    {
        
       if (Input.GetKeyUp(KeyCode.Escape) && !latch)
        {
            //Debug.Log("escape move");
            StopAllCoroutines();
            StartCoroutine(move());
            latch = true;
        }
       else if (Input.GetKeyUp(KeyCode.Escape) && latch)
        {
            //Debug.Log("escape move back");
            StopAllCoroutines();
            StartCoroutine(moveBack());
            latch = false;
        }

    }
    public void Settings()
    {

    }
    public void BackToGame()
    {
        StopAllCoroutines();
        StartCoroutine(moveBack());
        latch = false;
        gm.PauseGame(latch);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void OpenPause()
    {
        StopAllCoroutines();
        StartCoroutine(move());
        latch = true;
        gm.PauseGame(latch);
    }
    IEnumerator move()
    {
        while (pauseScreen.transform.position != targetPosition.transform.position)
        {
            //Debug.Log("moving");
            pauseScreen.transform.position = Vector3.SmoothDamp(pauseScreen.transform.position, targetPosition.transform.position, ref velocity, smoothTime);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator moveBack()
    {
        while (pauseScreen.transform.position != offscreenPosition.transform.position)
        {
            //Debug.Log("moving back");
            pauseScreen.transform.position = Vector3.SmoothDamp(pauseScreen.transform.position, offscreenPosition.transform.position, ref velocity, smoothTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
