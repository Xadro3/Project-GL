using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class CardRewardScript : MonoBehaviour
{
    public TextMeshProUGUI chooseText;
    public GameObject rewardArea;
    public Camera cam;
    public Canvas canvas;
    public Button btn_endEncounter;

    private void Awake()
    {
        cam = FindObjectOfType<Camera>();
        canvas.worldCamera = cam;
        canvas.planeDistance = 10;
        canvas.sortingLayerName = "Menu";
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleButton(bool b)
    {
        btn_endEncounter.gameObject.SetActive(b);
    }    
}
