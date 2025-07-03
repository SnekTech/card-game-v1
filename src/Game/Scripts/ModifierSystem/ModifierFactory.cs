namespace CardGameV1.ModifierSystem;

public static class ModifierFactory
{
    public static ModifierHandler CreatePlayerModifierHandler()
    {
        var handler = new ModifierHandler();
        var damageDealt = new Modifier { Type = ModifierType.DamageDealt };
        var damageTaken = new Modifier { Type = ModifierType.DamageTaken };
        var cardCost = new Modifier { Type = ModifierType.CardCost };
        handler.AddModifier(damageDealt);
        handler.AddModifier(damageTaken);
        handler.AddModifier(cardCost);
        return handler;
    }
}