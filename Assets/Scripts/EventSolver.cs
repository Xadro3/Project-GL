using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSolver : MonoBehaviour
{
   public GameObject backButton;
   public GameObject answer;
   public GameObject wrongAnswer;
   public GameObject question;
   public EventManager eventManager;
    private void Start()
    {
        eventManager = GameObject.FindGameObjectWithTag("Eventmanager").GetComponent<EventManager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Eventcard")
        {
            Debug.Log("Collision");
            collision.collider.GetComponent<Drag>().startPos = transform.position;
            if (collision.collider.GetComponent<EventCard>().isCorrectAnswer)
            {
                backButton.SetActive(true);
                answer.SetActive(true);
                question.SetActive(false);
                eventManager.CompleteEvent(eventManager.currentEvent);

            }
            else
            {
                wrongAnswer.SetActive(true);
                question.SetActive(false);
                backButton.SetActive(true);
                eventManager.CompleteEvent(eventManager.currentEvent);
            }
        }
    }
}
