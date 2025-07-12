using CardGameV1.EffectSystem;
using CardGameV1.ModifierSystem;
using GTweens.Easings;
using GTweensGodot.Extensions;

namespace CardGameV1.EnemyAI.ActionPerformers;

public class Attack(int damage, int times = 1) : ActionPerformer
{
    private string SoundPath { get; init; } = "res://art/enemy_attack.ogg";

    private const int AttackOffset = 32;

    public override async Task PerformActionAsync(CancellationToken cancellationToken)
    {
        if (Target == null)
        {
            GD.Print("target or enemy is null");
            return;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var startPosition = Enemy.GlobalPosition;
        var endPosition = Target.GlobalPosition + Vector2.Right * AttackOffset;
        var damageEffect = new DamageEffect(damage) { Sound = SnekUtility.LoadSound(SoundPath) };

        await Enemy.TweenGlobalPosition(endPosition, 0.4f).SetEasing(Easing.OutQuint).PlayAsync(cancellationToken);
        await damageEffect.ExecuteAllAsync([Target], cancellationToken);
        await SnekUtility.DelayGd(0.35f, cancellationToken);
        await Enemy.TweenGlobalPosition(startPosition, 0.4f).SetEasing(Easing.OutQuint)
            .PlayAsync(cancellationToken);
    }

    public override string DisplayText
    {
        get
        {
            if (Target == null)
            {
                return "";
            }

            var modifiedDamage = Target.ModifierHandler.GetModifiedValue(damage, ModifierType.DamageTaken);
            var timesText = times > 1 ? $"{times}x" : "";
            return $"{timesText}{modifiedDamage}";
        }
    }
}