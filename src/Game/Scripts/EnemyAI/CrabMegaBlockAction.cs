using System.Threading.Tasks;
using CardGameV1.EffectSystem;
using Godot;

namespace CardGameV1.EnemyAI;

public class CrabMegaBlockAction : EnemyConditionalAction
{
    private const int Block = 15;
    private const int Threshold = 6;

    private bool _alreadyUsed;

    public override bool IsPerformable()
    {
        if (Enemy == null || _alreadyUsed)
            return false;

        var healthIsLowEnough = Enemy.Stats.Health <= Threshold;

        _alreadyUsed = healthIsLowEnough;
        return healthIsLowEnough;
    }

    public override async Task PerformActionAsync()
    {
        if (Target == null || Enemy == null)
        {
            GD.Print("target or enemy is null");
            return;
        }

        var blockEffect = new BlockEffect(Block);
        blockEffect.Execute([Enemy]);

        await TaskUtility.DelayGd(0.6f);
        EventBus.EmitEnemyActionCompleted(Enemy);
    }
}