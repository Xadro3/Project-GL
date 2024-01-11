using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Node : MonoBehaviour
{
    public static event System.Action<bool, string> EnteringNodeEvent;
    
    public int eventType = 2;
    public Sprite eventSprite;
    public Sprite shopSprite;
    public Sprite workshopSprite;
    public Sprite encounterSprite;
    public GameObject nextNode;
    public GameObject nextNode2;
    public bool isUnlocked = false;
    public bool isLastNode = false;
    public bool isCompleted = false;
    public bool isFirstNode = false;
    public bool isActive = false;
    public bool isNextNode = false;
    public bool isPastNode = false;
    public string nextMap;
    Collider collider;
    SpriteRenderer sprite;
    
    public void WakeUp()
    {
        collider = GetComponent<BoxCollider>();
        if(nextNode != null)
        {
            nextNode = nextNode.GetComponent<NodeInstancer>().instantiatedNode;
        }
    
        if(nextNode2 != null)
        {
            nextNode2 = nextNode2.GetComponent<NodeInstancer>().instantiatedNode;
        }
      
        collider.enabled = false;
        DontDestroyOnLoad(this);
    }

    private void Update()
    {

        DisplayNode();

        if (isUnlocked && !isCompleted)
        {
            collider.enabled = true;
            sprite.color = new Color(1, 1, 1, 1);
        }
        if (!isUnlocked)
        {
            sprite.color = Color.gray;
            collider.enabled = false;
        }
        if (isLastNode&&isCompleted)
        {
            SceneManager.LoadScene(nextMap);
            isCompleted = false;
        }
        if (isFirstNode)
        {
            eventType = 2;
        }
        if (isCompleted)
        {

                isUnlocked = false;
                isNextNode = false;
                isPastNode = true;
                sprite.color = Color.gray;
                collider.enabled = false;
        }

        if (isPastNode)
        {
            sprite.color = Color.gray;
            collider.enabled = false;
        }
    }

    public void UnlockNextNode()
    {
        if (nextNode != null)
        {
            nextNode.GetComponent<Node>().isUnlocked = true;
        }
       

        if(nextNode2 != null)
        {
            nextNode2.GetComponent<Node>().isUnlocked = true;
        }
        

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
        if (isUnlocked)
        {
            isUnlocked = false;
            Debug.Log("Entering");
            EnteringNodeEvent?.Invoke(isLastNode, nextMap);
            Invoke("LoadNode", 1f);
        }
    }

    public void LoadNode()
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
