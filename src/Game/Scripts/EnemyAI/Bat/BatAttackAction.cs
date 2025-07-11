using CardGameV1.EffectSystem;
using CardGameV1.ModifierSystem;
using GTweens.Easings;
using GTweensGodot.Extensions;

namespace CardGameV1.EnemyAI.Bat;

public class BatAttackAction : EnemyChanceBasedAction
{
    public override Intent Intent { get; } = new($"2x{Damage}", "res://art/tile_0103.png");
    public override float ChanceWeight => 3;

    private const int Damage = 4;
    private const int AttackOffset = 32;
    private static readonly AudioStream AttackSound = SnekUtility.LoadSound("res://art/enemy_attack.ogg");

    public override async Task PerformActionAsync(CancellationToken cancellationToken)
    {
        if (Target == null || Enemy == null)
        {
            GD.Print("target or enemy is null");
            return;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var startPosition = Enemy.GlobalPosition;
        var endPosition = Target.GlobalPosition + Vector2.Right * AttackOffset;
        var damageEffect = new DamageEffect(Damage) { Sound = AttackSound };

        await Enemy.TweenGlobalPosition(endPosition, 0.4f).SetEasing(Easing.OutQuint).PlayAsync(cancellationToken);
        await damageEffect.ExecuteAllAsync([Target], cancellationToken);
        await SnekUtility.DelayGd(0.35f, cancellationToken);
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
        Intent.CurrentText = $"2x{modifiedDamage}";
    }
}