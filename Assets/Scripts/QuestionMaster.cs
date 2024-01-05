using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionMaster : MonoBehaviour
{
    public string questionText;
    public string answerText;
    public string wrongAnswerText;
    public EventSolver solver;
    public GameObject answer;
    public GameObject wrongAnswer;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Question").GetComponent<TextMeshProUGUI>().text = questionText;
        solver = GameObject.FindGameObjectWithTag("Eventslot").GetComponent<EventSolver>();
        answer = solver.answer;
        wrongAnswer = solver.wrongAnswer;
        answer.GetComponent<TextMeshProUGUI>().text = answerText;
        wrongAnswer.GetComponent<TextMeshProUGUI>().text = wrongAnswerText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
