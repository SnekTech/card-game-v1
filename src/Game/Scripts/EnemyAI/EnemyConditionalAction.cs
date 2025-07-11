namespace CardGameV1.EnemyAI;

public class EnemyConditionalAction : EnemyAction
{
    public required IPerformDictator PerformDictator { get; init; }
}

public interface IPerformDictator
{
    bool IsPerformable();
}