using System.Threading.Tasks;
using CardGameV1.EffectSystem;
using Godot;

namespace CardGameV1.EnemyAI.Bat;

public class BatBlockAction : EnemyChanceBasedAction
{
    public override Intent Intent { get; } = new("", "res://art/tile_0101.png");
    protected override AudioStream? Sound { get; } = SnekUtility.LoadSound("res://art/block.ogg");
    public override float ChanceWeight => 1;

    private const int Block = 4;

    public override async Task PerformActionAsync()
    {
        if (Target == null || Enemy == null)
        {
            GD.Print("target or enemy is null");
            return;
        }

        var blockEffect = new BlockEffect(Block) { Sound = Sound };
        await blockEffect.ExecuteAllAsync([Enemy]);

        await SnekUtility.DelayGd(0.6f);
    }
}