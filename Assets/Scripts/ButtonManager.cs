using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public string scene2;
    public float timer;
    // Start is called before the first frame update
    public void ButtonPressed(string scene)
    {
        SceneManager.LoadScene(scene);
        GameObject.FindGameObjectWithTag("NodeWallet").GetComponent<NodeLoader>().activeNode.GetComponent<Node>().isCompleted = true;
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

    public void ChangeScene2()
    {
        SceneManager.LoadScene(scene2);
    }
}
