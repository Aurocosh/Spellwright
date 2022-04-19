namespace Spellwright.Content.Spells.Base
{
    internal enum SpellCastResult
    {
        Success,
        IncantationInvalid,
        ModifiersInvalid,
        ArgumentInvalid,
        LevelTooLow,
        NoTomeToBind,
        AlreadyUnlocked,
        NotUnlocked,
        SpellUnlocked,
        NotEnoughReagents,
        CustomError
    }
}
