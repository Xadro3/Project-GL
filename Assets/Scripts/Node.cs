using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Node : MonoBehaviour
{
    public int eventType = 2;
    public Sprite eventSprite;
    public Sprite shopSprite;
    public Sprite workshopSprite;
    public Sprite encounterSprite;
    public GameObject nextNode;
    public bool isUnlocked = false;
    public bool isLastNode = false;
    public bool isCompleted = false;
    public bool isFirstNode = false;
    public bool isActive = false;
    public string nextMap;
    Collider collider;
    SpriteRenderer sprite;

    public void WakeUp()
    {
        collider = GetComponent<BoxCollider>();
        nextNode = nextNode.GetComponent<NodeInstancer>().instantiatedNode;
        collider.enabled = false;
        DontDestroyOnLoad(this);
    }

    private void Update()
    {

        DisplayNode();

        if (isUnlocked)
        {
            collider.enabled = true;
            sprite.color = new Color(1, 1, 1, 1);
        }
        if (!isUnlocked)
        {
            sprite.color = Color.gray;
        }
        if (isLastNode&&isCompleted)
        {
            SceneManager.LoadScene(nextMap);
        }
        if (isFirstNode)
        {
            eventType = 0;
        }
        if (isCompleted)
        {
            nextNode.GetComponent<Node>().isUnlocked = true;
        }
    }

    public void UnlockNextNode(GameObject nextNode)
    {
        nextNode.GetComponent<Node>().isUnlocked = true;
        

    }


    public void DisplayNode()
    {
        sprite = this.GetComponent<SpriteRenderer>();

        switch (eventType)
        {
            case 0:
                sprite.sprite = shopSprite;
                break;
            case 1:
                sprite.sprite = eventSprite;
                break;
            case 2:
                sprite.sprite = encounterSprite;
                break;
            case 3:
                sprite.sprite = workshopSprite;
                break;
        }

    }
    public void EnterNode()
    {
        isActive = true;
        switch (eventType)
        {
            case 0:
                SceneManager.LoadScene("Shops");
                break;
            case 1:
                SceneManager.LoadScene("Event");
                break;
            case 2:
                SceneManager.LoadScene("Encounter");
                break;
            case 3:
                SceneManager.LoadScene("Workshop");
                break;
            


        }
            
    }


}
