namespace CardGameV1.EnemyAI;

public abstract partial class EnemyConditionalAction : EnemyAction
{
    public virtual bool IsPerformable() => false;
}