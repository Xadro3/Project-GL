using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventReward : MonoBehaviour
{
    // Start is called before the first frame update
   // SpriteRenderer sprite;
    PendantManager pendant;
    public GameObject award;
    public GameObject wrong;
    public GameObject correct;
    public GameObject back;
    public GameObject rewardscreen;
    public GameObject weiter;
    public GameObject rewardPosition;
    public TextMeshProUGUI rewardDescription;
    void Start()
    {
        //sprite = gameObject.GetComponent<SpriteRenderer>();
        pendant = GameObject.FindGameObjectWithTag("PendantManager").GetComponent<PendantManager>();
        Debug.Log(pendant.name);
    }
    
    public void Award()
    {
        weiter.SetActive(false);
        wrong.SetActive(false);
        correct.SetActive(false);
        rewardscreen.SetActive(true);
        back.SetActive(true);
        award = pendant.AwardRandomPendant();
        rewardDescription.gameObject.SetActive(true);
        rewardDescription.text = award.GetComponent<PendantScript>().description;
        award.transform.position = rewardPosition.transform.position;
        award.transform.localScale = new Vector3(15f, 15f, 15f);
        award.GetComponent<Animator>().enabled = false;
        award.GetComponent<PendantScript>().SetSpriteRendererActive(true);
        award.GetComponent<BoxCollider2D>().enabled = true;
    }
}
