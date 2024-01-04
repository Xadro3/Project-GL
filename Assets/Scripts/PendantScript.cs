using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendantScript : MonoBehaviour
{
    [Header("General Info")]
    public new string name;
    [TextAreaAttribute]
    public string description;
    public SpriteRenderer spriteRenderer;
    public PendantManager homeBase;

    [Header("Effect Info")]
    public int pendantEffectValue;
    public GameConstants.pendantEffect pendantEffectType;
    public bool isActive = false;
    public bool isInEffect = false;

    private void Awake()
    {
        homeBase = FindObjectOfType<PendantManager>();
        OverworldSceneChanger.SceneChangeEvent += EveryoneGetInHere;
        ButtonManager.SceneChangeEvent += EveryoneGetInHere;
    }

    private void EveryoneGetInHere()
    {
        //Bringing Pendants to the safety of the Wallet :>
        gameObject.transform.SetParent(homeBase.transform);
    }

    private void OnDestroy()
    {
        OverworldSceneChanger.SceneChangeEvent -= EveryoneGetInHere;
        ButtonManager.SceneChangeEvent -= EveryoneGetInHere;
    }

    private void Start()
    {
        
    }

    public void SetPendantActive(bool b)
    {
        isActive = b;
    }

    public void SetPendantInEffect(bool b)
    {
        isInEffect = b;
    }

}
