using System;
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
    public GameObject continuebutton;
    public List<GameObject> allAnsweres;

    private void Awake()
    {
        allAnsweres.Clear();
        EventCard.AnswerSpawned += HandleAnswerSpawned;
    }

    private void OnDisable()
    {
        EventCard.AnswerSpawned -= HandleAnswerSpawned;
    }

    private void HandleAnswerSpawned(GameObject answer)
    {
        allAnsweres.Add(answer);
    }

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
                CORRECTANSWER?.Invoke();
                continuebutton.SetActive(true);
                answer.SetActive(true);
                question.SetActive(false);
                eventManager.CompleteEvent();

            }
            else
            {
                WRONGANSWER?.Invoke();
                wrongAnswer.SetActive(true);
                question.SetActive(false);
                backButton.SetActive(true);
                eventManager.CompleteEvent();
            }
            foreach (GameObject answer in allAnsweres)
            {
                if (answer != collision.gameObject)
                {
                    answer.SetActive(false);
                }
            }
        }
    }

    public static System.Action CORRECTANSWER;

    public static System.Action WRONGANSWER;
}
