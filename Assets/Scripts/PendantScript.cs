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
    public Sprite artwork;

    [Header("Effect Info")]
    public int pendantEffectValue;
    public GameConstants.pendantEffect pendantEffectType;
    public bool isActive = false;
    public bool isInEffect = false;

    public void SendPendantInfoToManager(PendantManager manager)
    {
        manager.AddPendantInfo(pendantEffectType, pendantEffectValue, isActive);
    }

    public void SetPendantActive(bool b)
    {
        isActive = b;
    }

}
