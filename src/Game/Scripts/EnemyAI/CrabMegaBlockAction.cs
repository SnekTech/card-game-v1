using System.Threading.Tasks;
using CardGameV1.EffectSystem;
using Godot;

namespace CardGameV1.EnemyAI;

public partial class CrabMegaBlockAction : EnemyConditionalAction
{
    [Export]
    private int block = 15;

    [Export]
    private int threshold = 6;

    private bool _alreadyUsed;

    public override bool IsPerformable()
    {
        if (Enemy == null || _alreadyUsed)
            return false;

        var healthIsLowEnough = Enemy.Stats.Health <= threshold;

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

        var blockEffect = new BlockEffect(block) { Sound = Sound };
        await blockEffect.ExecuteAllAsync([Enemy]);

        await TaskUtility.DelayGd(0.6f);
    }
}