using CardGameV1.Character;

namespace CardGameV1.EnemyAI.Crab;

public class PerformMegaBlockDictator : IPerformDictator
{
    public required Enemy Enemy { get; init; }
    private const int Threshold = 6;
    
    private bool _alreadyUsed;

    public bool IsPerformable()
    {
        if (_alreadyUsed)
            return false;
    
        var healthIsLowEnough = Enemy.Stats.Health <= Threshold;
    
        _alreadyUsed = healthIsLowEnough;
        return healthIsLowEnough;
    }
}