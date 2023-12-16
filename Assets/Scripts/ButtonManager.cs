using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void ButtonPressed(string scene)
    {
        SceneManager.LoadScene(scene);
        GameObject.FindGameObjectWithTag("NodeWallet").GetComponent<NodeLoader>().activeNode.GetComponent<Node>().isCompleted = true;
    }
    public void Exitgame()
    {
        Application.Quit();
    }
}
