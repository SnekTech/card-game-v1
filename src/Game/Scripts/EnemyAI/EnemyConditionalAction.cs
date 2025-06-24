namespace CardGameV1.EnemyAI;

public abstract class EnemyConditionalAction : EnemyAction
{
    public virtual bool IsPerformable() => false;
}