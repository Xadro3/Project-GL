using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public static event System.Action SceneChangeEvent;
    
    public string scene2;
    public float timer;
    // Start is called before the first frame update
    public void ButtonPressed()
    {
        SceneChangeEvent?.Invoke();
        GameObject.FindGameObjectWithTag("NodeWallet").GetComponent<NodeLoader>().activeNode.GetComponent<Node>().isCompleted = true;
        //GameObject.FindGameObjectWithTag("NodeWallet").GetComponent<NodeLoader>().activeNode.GetComponent<Node>().nextNode.GetComponent<Node>().isNextNode = true;
        //if(GameObject.FindGameObjectWithTag("NodeWallet").GetComponent<NodeLoader>().activeNode.GetComponent<Node>().nextNode2 != null)
        //{
        //    GameObject.FindGameObjectWithTag("NodeWallet").GetComponent<NodeLoader>().activeNode.GetComponent<Node>().nextNode2.GetComponent<Node>().isNextNode = true;
        //}
        Debug.Log(GameObject.FindGameObjectWithTag("NodeWallet").GetComponent<NodeLoader>().originScene + " thats what I am trying to load");
        GameObject.FindGameObjectWithTag("NodeWallet").GetComponent<NodeLoader>().LoadOverworld();
    }

    public void ChangeScene(string scene)
    {
        scene2 = scene;
        Invoke("ChangeScene2",timer);
    }
    public void Exitgame()
    {
        Application.Quit();
    }

    public void ResetGame()
    {
        NodeLoader node = GameObject.FindGameObjectWithTag("NodeWallet").GetComponent<NodeLoader>();
        Destroy(GameObject.FindGameObjectWithTag("Wallet"));
        Destroy(GameObject.FindGameObjectWithTag("Audio"));
        
        foreach(GameObject game in node.nodes)
        {
            Destroy(game);
        }

        SceneManager.LoadScene("Menu");
    }

    public void ChangeScene2()
    {
        SceneManager.LoadScene(scene2);
    }
}
