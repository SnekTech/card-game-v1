﻿namespace CardGameV1.EffectSystem;

public class BlockEffect(int amount) : Effect
{
    public override Task ExecuteAllAsync(IEnumerable<ITarget> targets, CancellationToken cancellationToken)
    {
        foreach (var target in targets)
        {
            target.Stats.Block += amount;
            if (Sound != null)
            {
                Autoload.SoundManager.SFXPlayer.Play(Sound);
            }
        }

        return Task.CompletedTask;
    }
}