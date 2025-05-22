namespace CardGameV1.EventBus;

public static class EventBusOwner
{
    public static readonly CardEventBus CardEventBus = new();
    public static readonly PlayerEventBus PlayerEventBus = new();
    public static readonly EnemyEventBus EnemyEventBus = new();
    public static readonly BattleEventBus BattleEvents = new();
}