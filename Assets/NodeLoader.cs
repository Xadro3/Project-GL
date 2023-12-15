using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NodeLoader : MonoBehaviour
{
    GameObject overworldgenerator;
    public List<GameObject> nodes;
    public GameObject[] nodeInstancer;
    bool maptGenerated=false;
    public Scene scene;
 
    private void Awake()
    {
        // It is save to remove listeners even if they
        // didn't exist so far.
        // This makes sure it is added only once
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // Add the listener to be called when a scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;

        DontDestroyOnLoad(gameObject);

        scene = SceneManager.GetActiveScene();
    }

    private void OnDestroy()
    {
        // Always clean up your listeners when not needed anymore
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Listener for sceneLoaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("NodeLoader Running");
        Debug.Log(nodes.Count+" nodes.");
        if (SceneManager.GetActiveScene().name == "Overworld")
        {
            overworldgenerator = GameObject.FindGameObjectWithTag("Nodegenerator");
            nodeInstancer = GameObject.FindGameObjectsWithTag("Nodeinstancer");
            string currentSceneName = SceneManager.GetActiveScene().name;

            if (nodes.Count == 0)
            {
                Debug.Log("Generating Map");
                foreach (GameObject nodeinst in nodeInstancer)
                {
                    nodeinst.GetComponent<NodeInstancer>().InstantiateNode();
                }

                overworldgenerator.GetComponent<OverworldGenerator>().GenerateMap();
                nodes = overworldgenerator.GetComponent<OverworldGenerator>().nodes;
                maptGenerated = true;
            }
            else
            {
                overworldgenerator.GetComponent<OverworldGenerator>().nodes = nodes;
            }

            foreach (GameObject gameObject in nodes)
            {
                gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject gameObject in nodes)
            {
                gameObject.SetActive(false);
            }
        }
        

        

        Debug.Log("Scene changed.");
        Debug.Log(SceneManager.GetActiveScene().name);
    }

}
