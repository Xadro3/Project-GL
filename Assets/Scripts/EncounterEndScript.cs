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

    private void Awake()
    {
        cam = FindObjectOfType<Camera>();
        canvas.worldCamera = cam;
        canvas.planeDistance = 10;
        canvas.sortingLayerName = "Menu";
    }

    public void SetupScreen(bool encounterWon, int rewardAmount)
    {
        switch (encounterWon)
        {
            case true:
                victoryText.gameObject.SetActive(true);
                break;

            case false:
                defeatText.gameObject.SetActive(true);
                break;
        }
        UpdateRewardAmount(rewardAmount);
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

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
