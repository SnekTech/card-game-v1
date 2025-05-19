using System.Threading.Tasks;
using CardGameV1.Character;
using CardGameV1.EffectSystem;
using CardGameV1.EventBus;

namespace CardGameV1.EnemyAI;

public abstract class EnemyAction
{
    public Enemy? Enemy { get; set; }
    public ITarget? Target { get; set; }

    protected static readonly EnemyEventBus EventBus = EventBusOwner.EnemyEventBus;

    public abstract Task PerformActionAsync();
}

public abstract class EnemyConditionalAction : EnemyAction
{
    public virtual bool IsPerformable() => false;
}

public abstract class EnemyChanceBasedAction : EnemyAction
{
    public float ChanceWeight { get; set; }
    public float AccumulatedWeight { get; set; }
}