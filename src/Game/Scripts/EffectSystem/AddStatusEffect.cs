﻿using System.Threading;
using System.Threading.Tasks;
using CardGameV1.StatusSystem;

namespace CardGameV1.EffectSystem;

public class AddStatusEffect(Status status) : Effect
{
    public override Task ExecuteAllAsync(IEnumerable<ITarget> targets, CancellationToken cancellationToken)
    {
        foreach (var target in targets)
        {
            target.StatusHandler.AddStatus(status);
        }
        return Task.CompletedTask;
    }
}