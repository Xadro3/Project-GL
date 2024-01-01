using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class NodeLoader : MonoBehaviour
{
    GameObject overworldgenerator;
    public List<GameObject> nodes;
    public GameObject[] nodeInstancer;
    public GameObject[] visitednodes;
    bool maptGenerated=false;
    public Scene scene;
    public GameObject activeNode;
    public bool generatedDesertMap = false;
    public bool disabledFirstNode = false;
    public bool disabledFirstNode2 = false;
 
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
        foreach(GameObject node in nodes)
        {
            if (node != null && node.GetComponent<Node>().isActive&&!node.GetComponent<Node>().isCompleted)
            {
                activeNode = node;
            }
            else if(node != null && node.GetComponent<Node>().isCompleted && node.GetComponent<Node>().isActive)
            {
                node.GetComponent<Node>().isActive = false;
            }
        }

        Debug.Log("NodeLoader Running");
        Debug.Log(nodes.Count+" nodes.");

        if (SceneManager.GetActiveScene().name == "Overworld")
        {
            overworldgenerator = GameObject.FindGameObjectWithTag("Nodegenerator");
            nodeInstancer = GameObject.FindGameObjectsWithTag("Nodeinstancer");
            string currentSceneName = SceneManager.GetActiveScene().name;
            //Check wether we already have nodes, otherwise generate a new map
            if (nodes.Count == 0)
            {
                Debug.Log("Generating Map");
                foreach (GameObject nodeinst in nodeInstancer)
                {
                    nodeinst.GetComponent<NodeInstancer>().InstantiateNode();
                }

                overworldgenerator.GetComponent<OverworldGenerator>().GenerateMap();
                nodes = overworldgenerator.GetComponent<OverworldGenerator>().nodes;
                //Instantiate nodes before we wake them up 
                foreach(GameObject node in nodes)
                {
                    node.GetComponent<Node>().WakeUp();
                }
                maptGenerated = true;
            }
            else //setup nodes if we already have a list of nodes.
            {
                overworldgenerator.GetComponent<OverworldGenerator>().nodes = nodes;
            }
            //Check wether we have on Completed node, if yes we disable the firstnodes of the map
            if (!disabledFirstNode)
            {
                foreach (GameObject gameObject in nodes)
                {
                    if (gameObject.GetComponent<Node>().isCompleted)
                    {
                        if (gameObject.GetComponent<Node>().isFirstNode)
                        {
                            disabledFirstNode = true;
                            gameObject.GetComponent<Node>().isUnlocked = false;
                            gameObject.GetComponent<Node>().isPastNode = true;
                        }

                    }
                }
            }
            //check what nodes are completed and what nodes are open and set their parameters accordingly
            foreach (GameObject gameObject in nodes)
            {
                gameObject.SetActive(true);
                if (gameObject.GetComponent<Node>().isCompleted && !gameObject.GetComponent<Node>().isNextNode)
                {
                    gameObject.GetComponent<Node>().isUnlocked = false;
                    gameObject.GetComponent<Node>().isPastNode = true;
                }
            }
        }
        else if(SceneManager.GetActiveScene().name == "DesertMap")
        {
            overworldgenerator = GameObject.FindGameObjectWithTag("Nodegenerator");
            nodeInstancer = GameObject.FindGameObjectsWithTag("Nodeinstancer");
            string currentSceneName = SceneManager.GetActiveScene().name;

            //if we haven generated the DesertMap yet, we generate one. First clear the Array
            if (!generatedDesertMap)
            {
                generatedDesertMap = true;
                foreach(GameObject node in nodes)
                {
                    Destroy(node);
                }
                nodes.Clear();
            }
            //Generate Map
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
                foreach (GameObject node in nodes)
                {
                    node.GetComponent<Node>().WakeUp();
                }
                maptGenerated = true;
            }
            else
            {
                overworldgenerator.GetComponent<OverworldGenerator>().nodes = nodes;
            }
            //Disable first nodes on the Desert map
            if (!disabledFirstNode2)
            {
                foreach (GameObject gameObject in nodes)
                {
                    if (gameObject.GetComponent<Node>().isCompleted)
                    {
                        if (gameObject.GetComponent<Node>().isFirstNode)
                        {
                            disabledFirstNode = true;
                            gameObject.GetComponent<Node>().isUnlocked = false;
                            gameObject.GetComponent<Node>().isPastNode = true;
                        }

                    }
                }
            }
         

                foreach (GameObject gameObject in nodes)
                {
                gameObject.SetActive(true);
                
                if (gameObject.GetComponent<Node>().isCompleted && !gameObject.GetComponent<Node>().isNextNode)
                {
                    gameObject.GetComponent<Node>().isUnlocked = false;
                    gameObject.GetComponent<Node>().isPastNode = true;
                }
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
