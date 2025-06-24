using System.Threading;
using System.Threading.Tasks;
using CardGameV1.EffectSystem;
using Godot;
using GTweens.Easings;
using GTweensGodot.Extensions;

namespace CardGameV1.EnemyAI.Bat;

public class BatAttackAction : EnemyChanceBasedAction
{
    public override Intent Intent { get; } = new($"2x{Damage}", "res://art/tile_0103.png");
    protected override AudioStream? Sound { get; } = SnekUtility.LoadSound("res://art/enemy_attack.ogg");
    public override float ChanceWeight => 3;

    private const int Damage = 4;
    private const int AttackOffset = 32;

    public override async Task PerformActionAsync()
    {
        if (Target == null || Enemy == null)
        {
            GD.Print("target or enemy is null");
            return;
        }

        var startPosition = Enemy.GlobalPosition;
        var endPosition = Target.GlobalPosition + Vector2.Right * AttackOffset;
        var damageEffect = new DamageEffect(Damage) { Sound = Sound };

        await Enemy.TweenGlobalPosition(endPosition, 0.4f).SetEasing(Easing.OutQuint).PlayAsync(CancellationToken.None);
        await damageEffect.ExecuteAllAsync([Target]);
        await SnekUtility.DelayGd(0.35f);
        await damageEffect.ExecuteAllAsync([Target]);
        await SnekUtility.DelayGd(0.25f);
        await Enemy.TweenGlobalPosition(startPosition, 0.4f).SetEasing(Easing.OutQuint)
            .PlayAsync(CancellationToken.None);
    }
}