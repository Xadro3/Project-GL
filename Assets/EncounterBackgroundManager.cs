using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EncounterBackgroundManager : MonoBehaviour
{
    public List<GameObject> greenBackgrounds;
    public List<GameObject> desertBackgrounds;

    private void Awake()
    {
        GameManager.SetEncounterBackgroundEvent += SetEncounterBackground;
    }
    private void OnDisable()
    {
        GameManager.SetEncounterBackgroundEvent -= SetEncounterBackground;
    }

    private void SetEncounterBackground(int encounterCompleted)
    {
        switch (encounterCompleted)
        {
            case <= 10:
                switch(encounterCompleted % 2)
                {
                    case 0:
                        greenBackgrounds[0].SetActive(true);
                        break;
                    case 1:
                        greenBackgrounds[1].SetActive(true);
                        break;
                }                
                //completely random background
                //randomIndex = Random.Range(0, greenBackgrounds.Count);
                //greenBackgrounds[randomIndex].SetActive(true);
                break;
            case > 10:
                switch (encounterCompleted % 2)
                {
                    case 0:
                        desertBackgrounds[0].SetActive(true);
                        break;
                    case 1:
                        desertBackgrounds[1].SetActive(true);
                        break;
                }
                //completely random background
                //randomIndex = Random.Range(0, desertBackgrounds.Count);
                //desertBackgrounds[randomIndex].SetActive(true);
                break;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
