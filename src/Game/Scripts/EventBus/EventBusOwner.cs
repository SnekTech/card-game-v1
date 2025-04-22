namespace CardGameV1.EventBus;

public static class EventBusOwner
{
    public static readonly CardEventBus CardEventBus = new();
}