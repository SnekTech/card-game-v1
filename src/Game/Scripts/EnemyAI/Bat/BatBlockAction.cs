using System.Threading.Tasks;
using CardGameV1.EffectSystem;
using Godot;

namespace CardGameV1.EnemyAI.Bat;

public partial class BatBlockAction : EnemyChanceBasedAction
{
    [Export]
    public int Block { get; private set; } = 4;

    public override async Task PerformActionAsync()
    {
        if (Target == null || Enemy == null)
        {
            GD.Print("target or enemy is null");
            return;
        }

        var blockEffect = new BlockEffect(Block) { Sound = Sound };
        await blockEffect.ExecuteAllAsync([Enemy]);

        await TaskUtility.DelayGd(0.6f);
    }
}