namespace CardGameV1.EventBus;

public static class EventBusOwner
{
    public static readonly CardEvents CardEvents = new();
    public static readonly PlayerEvents PlayerEvents = new();
    public static readonly EnemyEvents EnemyEvents = new();
    public static readonly BattleEvents BattleEvents = new();
    public static readonly Events Events = new();
}