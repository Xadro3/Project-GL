using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveCardSlots : MonoBehaviour
{
    public List<Slot> activeCardSlots;


    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        activeCardSlots.AddRange(GetComponentsInChildren<Slot>(true));
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Encounter")
        {
            foreach (var entry in activeCardSlots)
            {
                entry.gameObject.SetActive(true);
            }
        }
        
    }

    private void OnSceneUnloaded(Scene scene)
    {

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }


    void Start()
    {
        foreach (var entry in activeCardSlots)
        {
            entry.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
