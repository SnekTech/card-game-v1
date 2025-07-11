using CardGameV1.Character;
using CardGameV1.EffectSystem;
using GTweens.Easings;
using GTweensGodot.Extensions;

namespace CardGameV1.EnemyAI;

public abstract class EnemyAction
{
    public required Intent Intent { get; init; }
    public required ActionPerformer ActionPerformer { get; init; }
    public void UpdateTarget(ITarget target) => ActionPerformer.Target = target;

    public Task PerformActionAsync(CancellationToken cancellationToken) =>
        ActionPerformer.PerformActionAsync(cancellationToken);

    // todo: fix dynamic intent text
    public virtual void UpdateIntentText()
    {
        Intent.CurrentText = Intent.BaseText;
    }
}

public abstract class ActionPerformer
{
    public required Enemy Enemy { get; init; }
    public ITarget? Target { get; set; }

    public abstract Task PerformActionAsync(CancellationToken cancellationToken);
}

public class Attack(int damage) : ActionPerformer
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
}

public class Block(int blockAmount) : ActionPerformer
{
    private string SoundPath { get; init; } = "res://art/block.ogg";
    public override async Task PerformActionAsync(CancellationToken cancellationToken)
    {
        var blockEffect = new BlockEffect(blockAmount) { Sound = SnekUtility.LoadSound(SoundPath) };
        await blockEffect.ExecuteAllAsync([Enemy], cancellationToken);

        await SnekUtility.DelayGd(0.6f, cancellationToken);
    }
}