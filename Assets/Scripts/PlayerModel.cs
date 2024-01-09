using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerModel : MonoBehaviour
{
    public HealthBar alphaBar;
    public HealthBar betaBar;
    public HealthBar gammaBar;
    public HealthBar healthBar;
    public TextMeshProUGUI alphaText;
    public TextMeshProUGUI betaText;
    public TextMeshProUGUI gammaText;

    public GameObject characterOne;
    public GameObject characterTwo;
    public GameObject characterThree;

    public Animator playerModelAnimator;
    public Animator playerAbilitySlotAnimator;
    public Animator playerRessourceTextAnimator;

    private void OnEnable()
    {
        GameManager.NotEnoughEnergyEvent += HandleNotEnoughEnergy;
    }
    private void OnDisable()
    {
        GameManager.NotEnoughEnergyEvent -= HandleNotEnoughEnergy;
    }

    private void HandleNotEnoughEnergy()
    {
        playerRessourceTextAnimator.SetTrigger("Energy0");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
