using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRotate : MonoBehaviour
{
    [SerializeField]
    private GameObject drehKnopf;

    public void RotateKnopf()
    {
        drehKnopf.transform.Rotate(0f, 0f, -42);
    }
}
