using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSolver : MonoBehaviour
{
    GameObject backButton;
    GameObject answer;
    GameObject wrongAnswer;
    GameObject question;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Eventcard")
        {
            if (collision.collider.GetComponent<EventCard>().isCorrectAnswer)
            {
                backButton.SetActive(true);
                answer.SetActive(true);
                question.SetActive(false);

            }
            else
            {
                wrongAnswer.SetActive(true);
                question.SetActive(false);
                backButton.SetActive(true);
            }
        }
    }
}
