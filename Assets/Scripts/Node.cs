using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Node : MonoBehaviour
{
    public int eventType;
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
            case 1:
                sprite.sprite = workshopSprite;
                break;
            case 2:
                sprite.sprite = shopSprite;
                break;
            case 3:
                sprite.sprite = eventSprite;
                break;
            case 4:
                sprite.sprite = encounterSprite;
                break;
        }

    }
    public void EnterNode()
    {
        switch (eventType)
        {
            case 1:
                SceneManager.LoadScene("Workshop");
                break;
            case 2:
                SceneManager.LoadScene("Shops");
                break;
            case 3:
                SceneManager.LoadScene("Event");
                break;
            case 4:
                SceneManager.LoadScene("Encounter");
                break;
        }
    }


}
