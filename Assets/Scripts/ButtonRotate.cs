using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRotate : MonoBehaviour
{
    [SerializeField]
    private GameObject drehKnopf;
    public GameObject PlayerTurn;
    public GameObject EnemyTurn;

    public void RotateKnopf()
    {
        PlayerTurn.SetActive(false);
        drehKnopf.transform.Rotate(0f, 0f, -84);
        EnemyTurn.SetActive(true);
    }

    public void RotateKnopfBack()
    {
        EnemyTurn.SetActive(false);
        drehKnopf.transform.Rotate(0f, 0f, 84);
        PlayerTurn.SetActive(true);
    }
}
