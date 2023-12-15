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
    void Start()
    {
        Debug.Log("NodeLoader Running");
        overworldgenerator = GameObject.FindGameObjectWithTag("Nodegenerator");
        nodeInstancer = GameObject.FindGameObjectsWithTag("Nodeinstancer");
        string currentSceneName = SceneManager.GetActiveScene().name;

            if (!maptGenerated)
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

    }

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
        // return if not the start calling scene
        if (!string.Equals(scene.path, this.scene.path)){
            return;
        } 
        Debug.Log("Re-Initializing", this);
        Start();
    }

}
