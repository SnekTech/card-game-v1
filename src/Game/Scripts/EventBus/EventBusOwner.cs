namespace CardGameV1.EventBus;

public static class EventBusOwner
{
    public static readonly CardEventBus CardEvents = new();
    public static readonly PlayerEventBus PlayerEvents = new();
    public static readonly EnemyEventBus EnemyEvents = new();
    public static readonly BattleEventBus BattleEvents = new();
}