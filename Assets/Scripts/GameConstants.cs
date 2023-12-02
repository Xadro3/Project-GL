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
        ShieldRepair,
        ShieldBuff,
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
        EnergyCostReduction

    }

    public enum abilityTargets
    {
        AbilityPlayer,
        AbilityEnemy,
        AbilityShield

    }

    public enum cardTypes
    {
        Aggro,
        Control
    }
}
