using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardEffectEventHandler : MonoBehaviour
{
    //Triggered by CardMovementHandler                              //Handled by
    public static event Action<int> DamageReductionFlat;            //Card
    public static event Action<int> DamageReductionPercent;         //Card
    public static event Action<int> RadiationReductionFlat;         //Enemy
    public static event Action<int> RadiationReductionPercent;      //Enemy
    public static event Action<int> RadiationBlock;                 //Enemy
    public static event Action<int> RadiationImmunity;              //Slot
    public static event Action<int> RadiationOrderChange;           //Enemy
    public static event Action<int> ShieldRepair;                   //Slot
    public static event Action<int> ShieldBuff;                     //Slot
    public static event Action<int> ShieldDissolve;                 //Slot
    public static event Action<int> ResistanceReductionFlat;        //Player
    public static event Action<int> ResistanceReductionPercent;     //Player
    public static event Action<int> ResistanceEffectReduction;      //Player
    public static event Action<int> PlayerHealFlat;                 //Player
    public static event Action<int> PlayerHealPercent;              //Player
    public static event Action<int> TimerReductionFlat;             //Enemy
    public static event Action<int> DrawCard;                       //GameManager
    public static event Action<int> Discard;                        //Card? GameManager?
    public static event Action<int> EnergyGet;                      //GameManager
    public static event Action<int> EnergyCostReduction;            //Card


    public static void TriggerDamageReductionFlat(int value)
    {
        Debug.Log("Event: Trigger Damage Reduction Flat");
        DamageReductionFlat?.Invoke(value);
    }
    public static void TriggerDamageReductionPercent(int value)
    {
        DamageReductionPercent?.Invoke(value);
    }
    public static void TriggerRadiationReductionFlat(int value)
    {
        RadiationReductionFlat?.Invoke(value);
    }
    public static void TriggerRadiationReductionPercent(int value)
    {
        RadiationReductionPercent?.Invoke(value);
    }
    public static void TriggerRadiationBlock(int value)
    {
        RadiationBlock?.Invoke(value);
    }
    public static void TriggerRadiationImmunity(int value)
    {
        RadiationImmunity?.Invoke(value);
    }
    public static void TriggerRadiationOrderChange(int value)
    {
        RadiationOrderChange?.Invoke(value);
    }
    public static void TriggerShieldRepair(int value)
    {
        ShieldRepair?.Invoke(value);
    }
    public static void TriggerShieldBuff(int value)
    {
        ShieldBuff?.Invoke(value);
    }
    public static void TriggerShieldDissolve(int value)
    {
        ShieldDissolve?.Invoke(value);
    }
    public static void TriggerResistanceReductionFlat(int value)
    {
        ResistanceReductionFlat?.Invoke(value);
    }
    public static void TriggerResistanceReductionPercent(int value)
    {
        ResistanceReductionPercent?.Invoke(value);
    }
    public static void TriggerResistanceEffectReduction(int value)
    {
        ResistanceEffectReduction?.Invoke(value);
    }
    public static void TriggerPlayerHealFlat(int value)
    {
        PlayerHealFlat?.Invoke(value);
    }
    public static void TriggerPlayerHealPercent(int value)
    {
        PlayerHealPercent?.Invoke(value);
    }
    public static void TriggerTimerReductionFlat(int value)
    {
        TimerReductionFlat?.Invoke(value);
    }
    public static void TriggerDrawCard(int value)
    {
        DrawCard?.Invoke(value);
    }
    public static void TriggerDiscard(int value)
    {
        Discard?.Invoke(value);
    }
    public static void TriggerEnergyGet(int value)
    {
        EnergyGet?.Invoke(value);
    }
    public static void TriggerEnergyCostReduction(int value)
    {
        EnergyCostReduction?.Invoke(value);
    }

}
