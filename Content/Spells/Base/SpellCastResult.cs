namespace Spellwright.Content.Spells.Base
{
    internal enum SpellCastResult
    {
        Success,
        IncantationInvalid,
        SpellIsDisabled,
        ModifiersInvalid,
        ArgumentInvalid,
        LevelTooLow,
        NoResonatorToBind,
        AlreadyUnlocked,
        NotUnlocked,
        SpellUnlocked,
        NotEnoughReagents,
        CustomError
    }
}
