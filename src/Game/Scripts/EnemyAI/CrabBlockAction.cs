using System.Threading.Tasks;
using CardGameV1.EffectSystem;
using Godot;

namespace CardGameV1.EnemyAI;

public partial class CrabBlockAction : EnemyChanceBasedAction
{
    [Export]
    public int Block { get; private set; } = 6;
    
    public override async Task PerformActionAsync()
    {
        if (Target == null || Enemy == null)
        {
            GD.Print("target or enemy is null");
            return;
        }

        var blockEffect = new BlockEffect(Block);
        await blockEffect.ExecuteAllAsync([Enemy]);

        await TaskUtility.DelayGd(0.6f);
    }
}