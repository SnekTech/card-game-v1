using CardGameV1.EffectSystem;
using CardGameV1.ModifierSystem;
using GTweens.Easings;
using GTweensGodot.Extensions;

namespace CardGameV1.EnemyAI.Crab;

public class CrabAttackAction : EnemyChanceBasedAction
{
    public override Intent Intent { get; } = new($"{Damage}", "res://art/tile_0103.png");
    public override float ChanceWeight => 1;

    private const int Damage = 7;
    private const int AttackOffset = 32;
    private static readonly AudioStream CrabAttackSound = SnekUtility.LoadSound("res://art/enemy_attack.ogg");

    public override async Task PerformActionAsync(CancellationToken cancellationToken)
    {
        if (Target == null || Enemy == null)
        {
            GD.Print("target or enemy is null");
            return;
        }

        var startPosition = Enemy.GlobalPosition;
        var endPosition = Target.GlobalPosition + Vector2.Right * AttackOffset;
        var damageEffect = new DamageEffect(Damage) { Sound = CrabAttackSound };

        await Enemy.TweenGlobalPosition(endPosition, 0.4f).SetEasing(Easing.OutQuint).PlayAsync(cancellationToken);
        await damageEffect.ExecuteAllAsync([Target], cancellationToken);
        await SnekUtility.DelayGd(0.25f, cancellationToken);
        await Enemy.TweenGlobalPosition(startPosition, 0.4f).SetEasing(Easing.OutQuint)
            .PlayAsync(cancellationToken);
    }

    public override void UpdateIntentText()
    {
        if (Target == null)
        {
            GD.Print("target is null");
            return;
        }

        var modifiedDamage = Target.ModifierHandler.GetModifiedValue(Damage, ModifierType.DamageTaken);
        Intent.CurrentText = $"{modifiedDamage}";
    }
}