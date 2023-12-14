public static class GameConstants
{
    public enum radiationTypes
    {
        Alpha,
        Beta,
        Gamma,
        Pure
    }

    public enum effectTypes
    {
        DamageReductionFlat,
        DamageReductionPercent,
        RadiationReductionFlat,
        RadiationReductionPercent,
        RadiationBlock,
        RadiationImmunity,
        RadiationOrderChange,
        ShieldRepairPapier,
        ShieldRepairAlu,
        ShieldRepairBlei,
        ShieldRepair,
        ShieldBuff,
        ShieldMaxBuff,
        ShieldDissolve,
        ResistanceReductionFlat,
        ResistanceReductionPercent,
        ResistanceEffectReduction,
        PlayerHealFlat,
        PlayerHealPercent,
        TimerReductionFlat,
        DrawCard,
        Discard,
        EnergyGet,
        EnergyCostReduction,
        HealthDamageReductionPercent,
        EnergyLose,
        ShieldDissolvePapier,
        ShieldDissolveAlu,
        ShieldDissolveBlei

    }

    public enum abilityTargets
    {
        AbilityPlayer,
        AbilityEnemy,
        AbilityShield

    }

    public enum cardRarity
    {
        Common,
        Uncommon,
        Rare
    }

    public enum cardType
    {
        FähigkeitGegner,
        Fähigkeit,
        SchildAuflösung,
        SchildBuff,
        Schild,
        SchildReparatur
    }

    public enum cardUpgrades
    {
        EnergyCost,
        Effect,
        Schild,
        Duration
    }
}
