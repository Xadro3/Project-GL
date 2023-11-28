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
    SpriteRenderer sprite;
    
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
        switch (eventType)
        {
            case 3:
                SceneManager.LoadScene("Workshop");
                break;
            case 0:
                SceneManager.LoadScene("Shops");
                break;
            case 1:
                SceneManager.LoadScene("Event");
                break;
            case 2:
                SceneManager.LoadScene("Encounter");
                break;
        }
    }


}
