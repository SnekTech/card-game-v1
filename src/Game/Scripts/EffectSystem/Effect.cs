using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Godot;

namespace CardGameV1.EffectSystem;

public abstract class Effect
{
    public AudioStream? Sound { get; init; }
    
    public abstract Task ExecuteAllAsync(IEnumerable<ITarget> targets, CancellationToken cancellationToken);
}