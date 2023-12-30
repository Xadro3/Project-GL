using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CardPopup : MonoBehaviour
{
    public static event System.Action<bool> PauseGame;

    public TextMeshProUGUI cardInfoText;
    public Camera cam;
    public Canvas canvas;

    private void Awake()
    {
        cam = FindObjectOfType<Camera>();
        canvas.worldCamera = cam;
        canvas.planeDistance = 1;
        canvas.sortingLayerName = "CardPopups";
    }

    

    private void OnEnable()
    {
        PauseGame?.Invoke(true);
    }

    private void OnDisable()
    {
        cam = null;
        PauseGame?.Invoke(false);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {

    }

    private void Update()
    {
        // Check for a left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            // Destroy the popup GameObject
            gameObject.SetActive(false);
        }
    }

    public void SetCardInfo(string info)
    {
        // Update the text or UI elements to display card information
        if (cardInfoText != null)
        {
            cardInfoText.text = info;
            this.gameObject.SetActive(true);
        }
    }
}
