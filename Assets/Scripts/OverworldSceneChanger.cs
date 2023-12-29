using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OverworldSceneChanger : MonoBehaviour
{
    public Animator animator;
    public AudioManager audio;
    public AudioClip clip;
    private void Start()
    {
        audio = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("click");
            Debug.DrawRay(Input.mousePosition, Vector3.forward, Color.green,100);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                audio.PlaySFX(clip);
                Debug.Log(hit.transform.name);
                hit.collider.gameObject.GetComponent<Node>().EnterNode();
                animator.Play("TransitionClose");
}
        }
       
    }
}
