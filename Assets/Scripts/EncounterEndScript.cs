using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EncounterEndScript : MonoBehaviour
{
    public static event System.Action CardRewardScreenEvent;
    
    public TextMeshProUGUI victoryText;
    public TextMeshProUGUI defeatText;
    public Image shieldTokenPng;
    public TextMeshProUGUI shieldTokenAmount;
    public Camera cam;
    public Canvas canvas;
    public GameObject tablet;
    public Vector3 velocity = Vector3.zero;
    public float smoothTime;
    public GameObject targetPosition;
    public Button btn_weiter;
    public Button btn_beenden;
    public GameObject shieldTokenGrp;

    private void Awake()
    {
        cam = FindObjectOfType<Camera>();
        canvas.worldCamera = cam;
        canvas.planeDistance = 10;
        canvas.sortingLayerName = "Menu";
    }

    public void SetupScreen(bool encounterWon, int rewardAmount, GameObject offscreenPosition)
    {
        tablet.transform.position = offscreenPosition.transform.position;
        tablet.SetActive(true);
        switch (encounterWon)
        {
            case true:
                victoryText.gameObject.SetActive(true);
                defeatText.gameObject.SetActive(false);
                shieldTokenGrp.SetActive(true);
                UpdateRewardAmount(rewardAmount);
                break;

            case false:
                defeatText.gameObject.SetActive(true);
                victoryText.gameObject.SetActive(false);
                shieldTokenGrp.SetActive(false);
                btn_weiter.gameObject.SetActive(false);
                break;
        }
        StartCoroutine(move());
        
    }
    
    public void UpdateRewardAmount(int value)
    {
        shieldTokenAmount.text = value.ToString();
    }

    private void Start()
    {

    }

    public void StartCardReward()
    {
        CardRewardScreenEvent?.Invoke();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        
    }

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    IEnumerator move()
    {
        while (tablet.transform.position != targetPosition.transform.position)
        {
            //Debug.Log("moving");
            tablet.transform.position = Vector3.SmoothDamp(tablet.transform.position, targetPosition.transform.position, ref velocity, smoothTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
