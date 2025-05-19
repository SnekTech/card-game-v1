using System.Threading.Tasks;
using CardGameV1.EffectSystem;
using Godot;

namespace CardGameV1.EnemyAI;

public class CrabBlockAction : EnemyChanceBasedAction
{
    public int Block { get; init; }
    
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