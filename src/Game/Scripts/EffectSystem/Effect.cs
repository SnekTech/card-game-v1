using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;

namespace CardGameV1.EffectSystem;

public abstract class Effect
{
    public AudioStream? Sound { get; set; }
    
    public abstract Task ExecuteAllAsync(IEnumerable<ITarget> targets);
}