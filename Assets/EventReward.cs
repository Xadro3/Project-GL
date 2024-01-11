using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventReward : MonoBehaviour
{
    // Start is called before the first frame update
   // SpriteRenderer sprite;
    PendantManager pendant;
    GameObject award;
    public GameObject wrong;
    public GameObject correct;
    public GameObject back;
    public GameObject rewardscreen;
    void Start()
    {
        //sprite = gameObject.GetComponent<SpriteRenderer>();
        pendant = GameObject.FindGameObjectWithTag("PendantManager").GetComponent<PendantManager>();
        Debug.Log(pendant.name);
    }
    
    public void Award()
    {
        wrong.SetActive(false);
        correct.SetActive(false);
        rewardscreen.SetActive(true);
        back.SetActive(true);
        award = pendant.AwardRandomPendant();
        award.transform.position = transform.position;
    }
}
