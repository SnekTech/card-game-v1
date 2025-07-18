﻿using CardGameV1.EffectSystem;

namespace CardGameV1.StatusSystem.BuiltinStatuses;

public class TrueStrengthForm : Status
{
    private const int StacksPerTurn = 2;

    public override string Tooltip => GenerateTooltip(StacksPerTurn);

    public override async Task ApplyStatusAsync(ITarget target, CancellationToken cancellationToken)
    {
        GD.Print($"applying {nameof(TrueStrengthForm)}");

        var addMuscleEffect = new AddStatusEffect(StatusFactory.CreateMuscle(StacksPerTurn));
        await addMuscleEffect.ExecuteAllAsync([target], cancellationToken);
    }

    private static string GenerateTooltip(int stacksPerTurn) =>
        $"{nameof(TrueStrengthForm)}: at the start of its turn, gains {stacksPerTurn} {nameof(Muscle)}";
}