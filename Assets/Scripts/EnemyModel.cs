using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyModel : MonoBehaviour
{
    public TextMeshProUGUI roundTimerText;
    public TextMeshProUGUI alphaText;
    public TextMeshProUGUI betaText;
    public TextMeshProUGUI gammaText;

    public Animator armDisplayAnimator;
    // Start is called before the first frame update
    void Start()
    {
        armDisplayAnimator.SetTrigger("StartBreach");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
