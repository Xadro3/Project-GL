using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    // List to store the names of all available scenes
    private List<string> sceneNames = new List<string>();
    
    [SerializeField] Animator transitionAnim;

    void Start()
    {
        // Call a method to populate the sceneNames list with all available scenes
        PopulateSceneList();

        // Display the names of all available scenes in the console (for demonstration purposes)
        /**
        foreach (string sceneName in sceneNames)
        {
            Debug.Log("Scene Name: " + sceneName);
        }
        **/
    }

    void PopulateSceneList()
    {
        // Loop through all scenes in the build settings
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            // Get the scene path
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);

            // Extract the scene name from the path (without the file extension)
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            // Add the scene name to the list
            sceneNames.Add(sceneName);
        }
    }

    public void ChangeScene(string targetScene)
    {
        StartCoroutine(LoadLevel(targetScene));
           // SceneManager.LoadScene(targetScene);
    }

    IEnumerator LoadLevel(string targetScene)
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(targetScene);
        transitionAnim.SetTrigger("Start");
    }

}
