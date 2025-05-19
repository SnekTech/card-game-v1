using System.Threading;
using System.Threading.Tasks;
using CardGameV1.EffectSystem;
using Godot;
using GTweens.Builders;
using GTweens.Easings;
using GTweensGodot.Extensions;

namespace CardGameV1.EnemyAI;

public class CrabAttackAction : EnemyChanceBasedAction
{
    public int Damage { get; init; }

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
        var damageEffect = new DamageEffect(Damage);

        var tween = GTweenSequenceBuilder.New()
            .Append(Enemy.TweenGlobalPosition(endPosition, 0.4f))
            .AppendCallback(() => damageEffect.Execute([Target]))
            .AppendTime(0.25f)
            .Append(Enemy.TweenGlobalPosition(startPosition, 0.4f))
            .Build();
        tween.SetEasing(Easing.OutQuint);
        await tween.PlayAsync(CancellationToken.None);

        EventBus.EmitEnemyActionCompleted(Enemy);
    }
}